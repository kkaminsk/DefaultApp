using System.Diagnostics;
using System.Text;
using DefaultApp.BackgroundTasks;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace DefaultApp.Services;

/// <summary>
/// Service status states.
/// </summary>
public enum ServiceStatus
{
    Stopped,
    Starting,
    Running,
    Stopping,
    Error
}

/// <summary>
/// Types of packaged services.
/// </summary>
public enum ServiceType
{
    BackgroundTask,
    AppService,
    FullTrustProcess
}

/// <summary>
/// Controller for managing packaged service lifecycle, status, and uptime.
/// </summary>
public sealed class ServiceController : IDisposable
{
    private const string BackgroundTaskName = "TimerBackgroundTask";
    private const string AppServiceName = "com.contoso.defaultapp.eventlogservice";

    private readonly ILogger<ServiceController>? _logger;
    private readonly DispatcherQueue _dispatcherQueue;
    private readonly Stopwatch _uptimeStopwatch;
    private DispatcherQueueTimer? _timer;
    private DispatcherQueueTimer? _uptimeTimer;
    private ApplicationTrigger? _applicationTrigger;
    private IBackgroundTaskRegistration? _taskRegistration;

    // App Service fields
    private AppServiceConnection? _appServiceConnection;
    private DispatcherQueueTimer? _appServiceRefreshTimer;

    // Full Trust Process fields
    private NamedPipeClient? _namedPipeClient;
    private DispatcherQueueTimer? _fullTrustRefreshTimer;
    private string _fullTrustPipeName = string.Empty;
    private int _fullTrustEventsLogged;

    private ServiceStatus _status = ServiceStatus.Stopped;
    private ServiceType _currentServiceType = ServiceType.BackgroundTask;
    private bool _isDisposed;

    /// <summary>
    /// Event raised when the service status changes.
    /// </summary>
    public event EventHandler<ServiceStatus>? StatusChanged;

    /// <summary>
    /// Event raised when the uptime updates.
    /// </summary>
    public event EventHandler<TimeSpan>? UptimeUpdated;

    /// <summary>
    /// Event raised when the service content updates (e.g., current time for timer).
    /// </summary>
    public event EventHandler<string>? ContentUpdated;

    /// <summary>
    /// Event raised when an error occurs.
    /// </summary>
    public event EventHandler<string>? ErrorOccurred;

    public ServiceController(DispatcherQueue dispatcherQueue)
    {
        _dispatcherQueue = dispatcherQueue;
        _logger = App.LoggerFactory?.CreateLogger<ServiceController>();
        _uptimeStopwatch = new Stopwatch();
    }

    /// <summary>
    /// Gets the current service status.
    /// </summary>
    public ServiceStatus Status => _status;

    /// <summary>
    /// Gets the current service type.
    /// </summary>
    public ServiceType CurrentServiceType => _currentServiceType;

    /// <summary>
    /// Gets the current uptime.
    /// </summary>
    public TimeSpan Uptime => _uptimeStopwatch.Elapsed;

    /// <summary>
    /// Gets the status color for the current status.
    /// </summary>
    public string StatusColor => _status switch
    {
        ServiceStatus.Starting => "Yellow",
        ServiceStatus.Running => "Green",
        ServiceStatus.Stopping => "Yellow",
        ServiceStatus.Error => "Red",
        _ => "Gray"
    };

    /// <summary>
    /// Registers the background task on app launch.
    /// </summary>
    public async Task RegisterBackgroundTaskAsync()
    {
        try
        {
            _logger?.LogInformation("Registering background task");

            // Unregister existing task first
            UnregisterBackgroundTask();

            // Request access if needed
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            _logger?.LogDebug("Background access status: {Status}", status);

            // Create the trigger
            _applicationTrigger = new ApplicationTrigger();

            // Build and register the task
            var builder = new BackgroundTaskBuilder
            {
                Name = BackgroundTaskName,
                TaskEntryPoint = "DefaultApp.BackgroundTasks.TimerBackgroundTask"
            };
            builder.SetTrigger(_applicationTrigger);
            builder.IsNetworkRequested = false;

            _taskRegistration = builder.Register();
            _taskRegistration.Completed += OnBackgroundTaskCompleted;

            _logger?.LogInformation("Background task registered successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to register background task");
        }
    }

    /// <summary>
    /// Starts the specified service type.
    /// </summary>
    public async Task StartServiceAsync(ServiceType serviceType)
    {
        if (_status == ServiceStatus.Running || _status == ServiceStatus.Starting)
        {
            _logger?.LogWarning("Service is already running or starting");
            return;
        }

        _currentServiceType = serviceType;
        _logger?.LogInformation("Starting service: {ServiceType}", serviceType);

        SetStatus(ServiceStatus.Starting);

        try
        {
            switch (serviceType)
            {
                case ServiceType.BackgroundTask:
                    await StartBackgroundTaskServiceAsync();
                    break;

                case ServiceType.AppService:
                    await StartAppServiceAsync();
                    break;

                case ServiceType.FullTrustProcess:
                    await StartFullTrustProcessAsync();
                    break;
            }

            SetStatus(ServiceStatus.Running);
            StartUptimeTracking();

            _logger?.LogInformation("Service started: {ServiceType}", serviceType);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to start service: {ServiceType}", serviceType);
            SetStatus(ServiceStatus.Error);
            ErrorOccurred?.Invoke(this, $"Failed to start {serviceType}: {ex.Message}");
        }
    }

    /// <summary>
    /// Stops the current service.
    /// </summary>
    public async Task StopServiceAsync()
    {
        if (_status == ServiceStatus.Stopped || _status == ServiceStatus.Stopping)
        {
            _logger?.LogWarning("Service is already stopped or stopping");
            return;
        }

        _logger?.LogInformation("Stopping service: {ServiceType}", _currentServiceType);

        SetStatus(ServiceStatus.Stopping);
        StopUptimeTracking();

        try
        {
            switch (_currentServiceType)
            {
                case ServiceType.BackgroundTask:
                    StopBackgroundTaskService();
                    break;

                case ServiceType.AppService:
                    StopAppService();
                    break;

                case ServiceType.FullTrustProcess:
                    await StopFullTrustProcessAsync();
                    break;
            }

            // Small delay for UI feedback
            await Task.Delay(100);

            SetStatus(ServiceStatus.Stopped);
            ContentUpdated?.Invoke(this, string.Empty);

            _logger?.LogInformation("Service stopped: {ServiceType}", _currentServiceType);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error stopping service: {ServiceType}", _currentServiceType);
            SetStatus(ServiceStatus.Error);
            ErrorOccurred?.Invoke(this, $"Error stopping service: {ex.Message}");
        }
    }

    /// <summary>
    /// Switches to a different service type, stopping the current service first if needed.
    /// </summary>
    public async Task SwitchServiceAsync(ServiceType newServiceType)
    {
        if (_currentServiceType == newServiceType && _status == ServiceStatus.Running)
        {
            return;
        }

        _logger?.LogInformation("Switching service from {CurrentType} to {NewType}", _currentServiceType, newServiceType);

        // Stop current service if running
        if (_status == ServiceStatus.Running || _status == ServiceStatus.Starting)
        {
            await StopServiceAsync();
        }

        // Update service type
        _currentServiceType = newServiceType;
    }

    #region Background Task Service

    private async Task StartBackgroundTaskServiceAsync()
    {
        // Trigger the background task if registered
        if (_applicationTrigger is not null)
        {
            try
            {
                var result = await _applicationTrigger.RequestAsync();
                _logger?.LogDebug("Background task trigger result: {Result}", result);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Could not trigger background task, using timer fallback");
            }
        }

        // Start the DispatcherTimer for UI updates
        _timer = _dispatcherQueue.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;
        _timer.Start();

        // Initial update
        UpdateTimerContent();
    }

    private void StopBackgroundTaskService()
    {
        _timer?.Stop();
        _timer = null;
    }

    private void OnTimerTick(DispatcherQueueTimer sender, object args)
    {
        UpdateTimerContent();
        TimerBackgroundTask.RaiseTimerTick(DateTime.Now);
    }

    private void UpdateTimerContent()
    {
        var currentTime = DateTime.Now.ToString("HH:mm:ss");
        ContentUpdated?.Invoke(this, $"Current Time: {currentTime}");
    }

    #endregion

    #region App Service

    private async Task StartAppServiceAsync()
    {
        _logger?.LogInformation("Starting App Service connection");

        // Create and open the connection
        _appServiceConnection = new AppServiceConnection
        {
            AppServiceName = AppServiceName,
            PackageFamilyName = Windows.ApplicationModel.Package.Current.Id.FamilyName
        };

        _appServiceConnection.ServiceClosed += OnAppServiceClosed;

        var status = await _appServiceConnection.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
            _logger?.LogError("Failed to open App Service connection: {Status}", status);
            throw new InvalidOperationException($"Failed to connect to App Service: {status}");
        }

        _logger?.LogInformation("App Service connection opened successfully");

        // Query initial data
        await QueryAppServiceEventsAsync();

        // Set up periodic refresh (every 30 seconds)
        _appServiceRefreshTimer = _dispatcherQueue.CreateTimer();
        _appServiceRefreshTimer.Interval = TimeSpan.FromSeconds(30);
        _appServiceRefreshTimer.Tick += OnAppServiceRefreshTick;
        _appServiceRefreshTimer.Start();
    }

    private void StopAppService()
    {
        _appServiceRefreshTimer?.Stop();
        _appServiceRefreshTimer = null;

        if (_appServiceConnection is not null)
        {
            _appServiceConnection.ServiceClosed -= OnAppServiceClosed;
            _appServiceConnection.Dispose();
            _appServiceConnection = null;
        }

        _logger?.LogInformation("App Service connection closed");
    }

    private async void OnAppServiceRefreshTick(DispatcherQueueTimer sender, object args)
    {
        await QueryAppServiceEventsAsync();
    }

    private async Task QueryAppServiceEventsAsync()
    {
        if (_appServiceConnection is null)
        {
            return;
        }

        try
        {
            var request = new ValueSet { ["Command"] = "QueryEvents" };
            var response = await _appServiceConnection.SendMessageAsync(request);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                var message = response.Message;

                if (message.TryGetValue("Status", out var status) && status?.ToString() == "OK")
                {
                    var content = FormatEventLogResponse(message);
                    ContentUpdated?.Invoke(this, content);
                }
                else
                {
                    var errorMsg = message.TryGetValue("Message", out var msg) ? msg?.ToString() : "Unknown error";
                    ContentUpdated?.Invoke(this, $"Error: {errorMsg}");
                    _logger?.LogWarning("App Service returned error: {Error}", errorMsg);
                }
            }
            else
            {
                _logger?.LogWarning("App Service request failed: {Status}", response.Status);
                ContentUpdated?.Invoke(this, $"Connection error: {response.Status}");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error querying App Service");
            ContentUpdated?.Invoke(this, $"Error: {ex.Message}");
        }
    }

    private static string FormatEventLogResponse(ValueSet message)
    {
        var sb = new StringBuilder();

        if (message.TryGetValue("EventCount", out var countObj) && countObj is int count)
        {
            sb.AppendLine($"Event Log: {count} Warning/Error events (last 24h)");
            sb.AppendLine();

            for (int i = 0; i < count && i < 10; i++)
            {
                if (message.TryGetValue($"Event_{i}_Timestamp", out var timestamp) &&
                    message.TryGetValue($"Event_{i}_Level", out var level) &&
                    message.TryGetValue($"Event_{i}_Source", out var source) &&
                    message.TryGetValue($"Event_{i}_Message", out var msg))
                {
                    var levelStr = level?.ToString() ?? "Unknown";
                    var levelIcon = levelStr == "Error" ? "[!]" : "[W]";

                    sb.AppendLine($"{levelIcon} {timestamp}");
                    sb.AppendLine($"    {source}: {msg}");
                    sb.AppendLine();
                }
            }

            if (count == 0)
            {
                sb.AppendLine("No warning or error events found.");
            }
        }
        else
        {
            sb.AppendLine("Event Log: Unable to retrieve event count");
        }

        return sb.ToString().TrimEnd();
    }

    private void OnAppServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
    {
        _logger?.LogWarning("App Service connection closed: {Reason}", args.Status);

        _dispatcherQueue.TryEnqueue(() =>
        {
            if (_status == ServiceStatus.Running && _currentServiceType == ServiceType.AppService)
            {
                SetStatus(ServiceStatus.Error);
                ErrorOccurred?.Invoke(this, $"App Service connection closed: {args.Status}");
            }
        });
    }

    #endregion

    #region Full Trust Process

    private async Task StartFullTrustProcessAsync()
    {
        _logger?.LogInformation("Starting Full Trust Process");

        try
        {
            // Launch the Full Trust Process using FullTrustProcessLauncher
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("ContinuousLogger");

            // Wait a moment for the process to start and create its pipe
            await Task.Delay(1000);

            // Find the Full Trust Process and get its pipe name
            var pipeName = await FindFullTrustProcessPipeNameAsync();

            if (string.IsNullOrEmpty(pipeName))
            {
                throw new InvalidOperationException("Could not find Full Trust Process pipe");
            }

            _fullTrustPipeName = pipeName;
            _logger?.LogInformation("Found Full Trust Process pipe: {PipeName}", pipeName);

            // Connect to the Named Pipe
            _namedPipeClient = new NamedPipeClient();
            _namedPipeClient.ConnectionStateChanged += OnNamedPipeConnectionStateChanged;

            var connected = await _namedPipeClient.ConnectAsync(pipeName);

            if (!connected)
            {
                throw new InvalidOperationException("Failed to connect to Full Trust Process");
            }

            _logger?.LogInformation("Connected to Full Trust Process");

            // Get initial status
            await QueryFullTrustStatusAsync();

            // Set up periodic status refresh (every 5 seconds)
            _fullTrustRefreshTimer = _dispatcherQueue.CreateTimer();
            _fullTrustRefreshTimer.Interval = TimeSpan.FromSeconds(5);
            _fullTrustRefreshTimer.Tick += OnFullTrustRefreshTick;
            _fullTrustRefreshTimer.Start();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to start Full Trust Process");
            throw;
        }
    }

    private async Task<string?> FindFullTrustProcessPipeNameAsync()
    {
        // Look for DefaultApp.FullTrustProcess processes
        var processes = Process.GetProcessesByName("DefaultApp.FullTrustProcess");

        foreach (var process in processes)
        {
            var pipeName = $"DefaultApp_{process.Id}";
            _logger?.LogDebug("Checking for pipe: {PipeName}", pipeName);

            // Try to connect briefly to verify the pipe exists
            try
            {
                using var testClient = new System.IO.Pipes.NamedPipeClientStream(".", pipeName, System.IO.Pipes.PipeDirection.InOut);
                using var cts = new CancellationTokenSource(2000);
                await testClient.ConnectAsync(cts.Token);

                // Found a valid pipe
                return pipeName;
            }
            catch
            {
                // This pipe doesn't exist or isn't ready, try next
            }
        }

        return null;
    }

    private async Task StopFullTrustProcessAsync()
    {
        _logger?.LogInformation("Stopping Full Trust Process");

        _fullTrustRefreshTimer?.Stop();
        _fullTrustRefreshTimer = null;

        if (_namedPipeClient is not null)
        {
            try
            {
                // Send shutdown command
                await _namedPipeClient.SendShutdownAsync();
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error sending shutdown to Full Trust Process");
            }

            _namedPipeClient.ConnectionStateChanged -= OnNamedPipeConnectionStateChanged;
            _namedPipeClient.Dispose();
            _namedPipeClient = null;
        }

        _fullTrustPipeName = string.Empty;
        _fullTrustEventsLogged = 0;

        _logger?.LogInformation("Full Trust Process stopped");
    }

    private async void OnFullTrustRefreshTick(DispatcherQueueTimer sender, object args)
    {
        await QueryFullTrustStatusAsync();
    }

    private async Task QueryFullTrustStatusAsync()
    {
        if (_namedPipeClient is null || !_namedPipeClient.IsConnected)
        {
            return;
        }

        try
        {
            var (success, eventsLogged, uptimeSeconds) = await _namedPipeClient.GetStatusAsync();

            if (success)
            {
                _fullTrustEventsLogged = eventsLogged;
                UpdateFullTrustContent(eventsLogged, uptimeSeconds);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error querying Full Trust Process status");
        }
    }

    private void UpdateFullTrustContent(int eventsLogged, double uptimeSeconds)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Full Trust Process - Event Logger");
        sb.AppendLine();
        sb.AppendLine($"Connection: Connected");
        sb.AppendLine($"Pipe: {_fullTrustPipeName}");
        sb.AppendLine($"Events Logged: {eventsLogged}");
        sb.AppendLine($"Process Uptime: {TimeSpan.FromSeconds(uptimeSeconds):hh\\:mm\\:ss}");

        ContentUpdated?.Invoke(this, sb.ToString().TrimEnd());
    }

    private void OnNamedPipeConnectionStateChanged(object? sender, bool connected)
    {
        _dispatcherQueue.TryEnqueue(() =>
        {
            if (!connected && _status == ServiceStatus.Running && _currentServiceType == ServiceType.FullTrustProcess)
            {
                _logger?.LogWarning("Full Trust Process connection lost");
                SetStatus(ServiceStatus.Error);
                ErrorOccurred?.Invoke(this, "Full Trust Process connection lost");
            }
        });
    }

    /// <summary>
    /// Logs a test event to the Full Trust Process event logger.
    /// </summary>
    public async Task<bool> LogTestEventAsync(string message, string level = "Information")
    {
        if (_namedPipeClient is null || !_namedPipeClient.IsConnected)
        {
            _logger?.LogWarning("Cannot log event: not connected to Full Trust Process");
            return false;
        }

        return await _namedPipeClient.LogEventAsync(message, level);
    }

    #endregion

    #region Uptime Tracking

    private void StartUptimeTracking()
    {
        _uptimeStopwatch.Restart();

        _uptimeTimer = _dispatcherQueue.CreateTimer();
        _uptimeTimer.Interval = TimeSpan.FromSeconds(1);
        _uptimeTimer.Tick += OnUptimeTick;
        _uptimeTimer.Start();
    }

    private void StopUptimeTracking()
    {
        _uptimeTimer?.Stop();
        _uptimeTimer = null;
        _uptimeStopwatch.Stop();
    }

    private void OnUptimeTick(DispatcherQueueTimer sender, object args)
    {
        UptimeUpdated?.Invoke(this, _uptimeStopwatch.Elapsed);
    }

    #endregion

    #region Status Management

    private void SetStatus(ServiceStatus status)
    {
        if (_status != status)
        {
            _status = status;
            _logger?.LogDebug("Service status changed to: {Status}", status);
            StatusChanged?.Invoke(this, status);
        }
    }

    #endregion

    #region Background Task Registration

    private void UnregisterBackgroundTask()
    {
        foreach (var task in BackgroundTaskRegistration.AllTasks)
        {
            if (task.Value.Name == BackgroundTaskName)
            {
                task.Value.Unregister(true);
                _logger?.LogDebug("Unregistered existing background task");
            }
        }
        _taskRegistration = null;
    }

    private void OnBackgroundTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
    {
        _logger?.LogInformation("Background task completed");
    }

    #endregion

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        _timer?.Stop();
        _uptimeTimer?.Stop();
        _appServiceRefreshTimer?.Stop();
        _fullTrustRefreshTimer?.Stop();
        _uptimeStopwatch.Stop();

        _appServiceConnection?.Dispose();
        _namedPipeClient?.Dispose();

        if (_taskRegistration is not null)
        {
            _taskRegistration.Completed -= OnBackgroundTaskCompleted;
        }

        _logger?.LogDebug("ServiceController disposed");
    }
}
