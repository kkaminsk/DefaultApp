using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DefaultApp.Models;
using DefaultApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;

namespace DefaultApp.ViewModels;

/// <summary>
/// ViewModel for the main page displaying system and hardware information.
/// </summary>
public partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly SystemInfoService _systemInfoService;
    private readonly HardwareInfoService _hardwareInfoService;
    private readonly ActivationService _activationService;
    private readonly ILogger<MainViewModel>? _logger;
    private ServiceController? _serviceController;
    private bool _isDisposed;

    public MainViewModel()
    {
        _systemInfoService = new SystemInfoService();
        _hardwareInfoService = new HardwareInfoService();
        _activationService = new ActivationService();
        _logger = App.LoggerFactory?.CreateLogger<MainViewModel>();
    }

    #region OS Information Properties

    [ObservableProperty]
    private string _osName = "Loading...";

    [ObservableProperty]
    private string _osVersion = "Loading...";

    [ObservableProperty]
    private string _osBuildNumber = "Loading...";

    [ObservableProperty]
    private string _osEdition = "Loading...";

    [ObservableProperty]
    private string _osIs64Bit = "Loading...";

    [ObservableProperty]
    private string _systemLocale = "Loading...";

    [ObservableProperty]
    private string _activationStatus = "Checking...";

    #endregion

    #region Hardware Information Properties

    [ObservableProperty]
    private string _machineName = "Loading...";

    [ObservableProperty]
    private string _processorArchitecture = "Loading...";

    [ObservableProperty]
    private string _osArchitecture = "Loading...";

    [ObservableProperty]
    private string _isRunningUnderEmulation = "Loading...";

    [ObservableProperty]
    private string _cpuModel = "Loading...";

    [ObservableProperty]
    private string _processorCount = "Loading...";

    [ObservableProperty]
    private string _totalRam = "Loading...";

    [ObservableProperty]
    private string _is64BitProcess = "Loading...";

    #endregion

    #region Service Properties

    [ObservableProperty]
    private int _selectedServiceIndex;

    [ObservableProperty]
    private string _serviceStatus = "Stopped";

    [ObservableProperty]
    private string _serviceStatusColor = "Gray";

    [ObservableProperty]
    private string _serviceUptime = "N/A";

    [ObservableProperty]
    private string _serviceContent = string.Empty;

    [ObservableProperty]
    private bool _isServiceRunning;

    [ObservableProperty]
    private string _startStopButtonText = "Start Service";

    #endregion

    #region Refresh State

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private bool _isRefreshing;

    #endregion

    /// <summary>
    /// Initializes the service controller with the dispatcher queue.
    /// Must be called from the UI thread after construction.
    /// </summary>
    public async Task InitializeServiceControllerAsync(DispatcherQueue dispatcherQueue)
    {
        _serviceController = new ServiceController(dispatcherQueue);

        // Subscribe to service events
        _serviceController.StatusChanged += OnServiceStatusChanged;
        _serviceController.UptimeUpdated += OnServiceUptimeUpdated;
        _serviceController.ContentUpdated += OnServiceContentUpdated;
        _serviceController.ErrorOccurred += OnServiceErrorOccurred;

        // Register the background task
        await _serviceController.RegisterBackgroundTaskAsync();

        _logger?.LogInformation("Service controller initialized");
    }

    /// <summary>
    /// Auto-starts the default service (Background Task).
    /// </summary>
    public async Task AutoStartServiceAsync()
    {
        if (_serviceController is not null)
        {
            _logger?.LogInformation("Auto-starting default service (Background Task)");
            await _serviceController.StartServiceAsync(ServiceType.BackgroundTask);
        }
    }

    /// <summary>
    /// Loads all data asynchronously.
    /// </summary>
    public async Task LoadDataAsync()
    {
        _logger?.LogInformation("Loading system information data");

        try
        {
            // Load OS information
            var osInfo = _systemInfoService.GetOsInfo();
            OsName = osInfo.Name;
            OsVersion = osInfo.Version;
            OsBuildNumber = osInfo.BuildNumber;
            OsEdition = osInfo.Edition;
            OsIs64Bit = osInfo.Is64BitOs ? "Yes" : "No";
            SystemLocale = osInfo.SystemLocale;
            ActivationStatus = "Checking...";

            // Load hardware information
            var archInfo = _hardwareInfoService.GetArchitectureInfo();
            MachineName = archInfo.MachineName;
            ProcessorArchitecture = archInfo.ProcessArchitecture;
            OsArchitecture = archInfo.OsArchitecture;
            IsRunningUnderEmulation = archInfo.IsRunningUnderEmulation ? "Yes" : "No";
            CpuModel = FormatCpuModels(archInfo.CpuModels);
            ProcessorCount = archInfo.ProcessorCount.ToString();
            TotalRam = archInfo.TotalRam;
            Is64BitProcess = archInfo.Is64BitProcess ? "Yes" : "No";

            // Load activation status asynchronously
            var status = await _activationService.GetActivationStatusAsync();
            ActivationStatus = ActivationService.GetActivationStatusDisplay(status);

            _logger?.LogInformation("System information data loaded successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading system information data");
        }
    }

    /// <summary>
    /// Refreshes all system information.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private async Task RefreshAsync()
    {
        _logger?.LogInformation("Refreshing system information");
        IsRefreshing = true;

        try
        {
            // Clear activation cache to force re-check
            _activationService.ClearCache();

            await LoadDataAsync();

            _logger?.LogInformation("System information refreshed");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private bool CanRefresh() => !IsRefreshing;

    /// <summary>
    /// Toggles the service start/stop state.
    /// </summary>
    [RelayCommand]
    private async Task ToggleServiceAsync()
    {
        if (_serviceController is null)
        {
            _logger?.LogWarning("Service controller not initialized");
            return;
        }

        _logger?.LogInformation("Toggle service requested for service type index: {Index}", SelectedServiceIndex);

        if (IsServiceRunning)
        {
            await _serviceController.StopServiceAsync();
        }
        else
        {
            var serviceType = IndexToServiceType(SelectedServiceIndex);
            await _serviceController.StartServiceAsync(serviceType);
        }
    }

    partial void OnSelectedServiceIndexChanged(int value)
    {
        _logger?.LogDebug("Service type changed to index: {Index}", value);

        if (_serviceController is null)
        {
            return;
        }

        // Switch service type (will stop current service if running)
        var newServiceType = IndexToServiceType(value);
        _ = _serviceController.SwitchServiceAsync(newServiceType);

        // Clear service-specific content
        ServiceContent = string.Empty;
    }

    private void OnServiceStatusChanged(object? sender, ServiceStatus status)
    {
        _logger?.LogDebug("Service status changed: {Status}", status);

        ServiceStatus = status.ToString();
        ServiceStatusColor = status switch
        {
            Services.ServiceStatus.Starting => "Yellow",
            Services.ServiceStatus.Running => "Green",
            Services.ServiceStatus.Stopping => "Yellow",
            Services.ServiceStatus.Error => "Red",
            _ => "Gray"
        };

        IsServiceRunning = status == Services.ServiceStatus.Running;
        StartStopButtonText = IsServiceRunning ? "Stop Service" : "Start Service";

        if (status == Services.ServiceStatus.Stopped)
        {
            ServiceUptime = "N/A";
        }
    }

    private void OnServiceUptimeUpdated(object? sender, TimeSpan uptime)
    {
        ServiceUptime = uptime.ToString(@"hh\:mm\:ss");
    }

    private void OnServiceContentUpdated(object? sender, string content)
    {
        ServiceContent = content;
    }

    private void OnServiceErrorOccurred(object? sender, string errorMessage)
    {
        _logger?.LogError("Service error: {Error}", errorMessage);
        // Error is already reflected in status change
    }

    private static ServiceType IndexToServiceType(int index)
    {
        return index switch
        {
            0 => ServiceType.BackgroundTask,
            1 => ServiceType.AppService,
            2 => ServiceType.FullTrustProcess,
            _ => ServiceType.BackgroundTask
        };
    }

    private static string FormatCpuModels(IReadOnlyList<string> cpuModels)
    {
        if (cpuModels.Count == 0)
        {
            return "Unavailable";
        }

        if (cpuModels.Count == 1)
        {
            return cpuModels[0];
        }

        // Multiple CPUs - format as list
        return string.Join("\n", cpuModels.Select((model, index) => $"{index + 1}. {model}"));
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        if (_serviceController is not null)
        {
            _serviceController.StatusChanged -= OnServiceStatusChanged;
            _serviceController.UptimeUpdated -= OnServiceUptimeUpdated;
            _serviceController.ContentUpdated -= OnServiceContentUpdated;
            _serviceController.ErrorOccurred -= OnServiceErrorOccurred;
            _serviceController.Dispose();
        }

        _logger?.LogDebug("MainViewModel disposed");
    }
}
