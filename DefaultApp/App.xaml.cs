using DefaultApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace DefaultApp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window? _window;
    private SplashWindow? _splashWindow;
    private LoggingService? _loggingService;
    private ILoggerFactory? _loggerFactory;

    /// <summary>
    /// Gets the logging service instance.
    /// </summary>
    public static LoggingService? Logger { get; private set; }

    /// <summary>
    /// Gets the logger factory for creating category-specific loggers.
    /// </summary>
    public static ILoggerFactory? LoggerFactory { get; private set; }

    /// <summary>
    /// Initializes the singleton application object.
    /// </summary>
    public App()
    {
        InitializeLogging();
        SetupCrashReporting();
        this.InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _loggingService?.WriteLog(LogLevel.Information, "App", "Application launched");

        // Show splash screen first
        _splashWindow = new SplashWindow();
        _splashWindow.Activate();

        // Initialize main window in background
        _ = InitializeAndShowMainWindowAsync();
    }

    private async Task InitializeAndShowMainWindowAsync()
    {
        try
        {
            _splashWindow?.SetLoadingStatus("Initializing...");

            // Small delay to ensure splash is visible
            await Task.Delay(300);

            // Create main window (this does the heavy initialization)
            _window = new MainWindow();

            _splashWindow?.SetLoadingStatus("Loading system information...");

            // Signal splash that app initialization is complete
            _splashWindow?.SignalAppReady();

            // Wait for splash to complete its full sequence (min time + fade out)
            if (_splashWindow is not null)
            {
                await _splashWindow.SplashCompleteTask;
            }

            // Show main window and close splash
            _window.Activate();
            _window.Closed += OnWindowClosed;

            _splashWindow?.Close();
            _splashWindow = null;

            _loggingService?.WriteLog(LogLevel.Information, "App", "Main window activated");
        }
        catch (Exception ex)
        {
            _loggingService?.WriteLog(LogLevel.Error, "App", $"Error during initialization: {ex.Message}");

            // If something goes wrong, still try to show the main window
            _window?.Activate();
            _splashWindow?.Close();
        }
    }

    /// <summary>
    /// Gets the main window of the application.
    /// </summary>
    public Window? MainWindow => _window;

    private void InitializeLogging()
    {
        try
        {
            _loggingService = new LoggingService();
            Logger = _loggingService;

            _loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new FileLoggerProvider(_loggingService));
#if DEBUG
                builder.SetMinimumLevel(LogLevel.Debug);
#else
                builder.SetMinimumLevel(LogLevel.Information);
#endif
            });
            LoggerFactory = _loggerFactory;

            _loggingService.WriteLog(LogLevel.Information, "App", "Logging initialized");
        }
        catch (Exception ex)
        {
            // Logging initialization failed - continue without logging
            System.Diagnostics.Debug.WriteLine($"Failed to initialize logging: {ex.Message}");
        }
    }

    private void SetupCrashReporting()
    {
        // Handle unhandled exceptions in the UI thread
        UnhandledException += OnUnhandledException;

        // Handle unhandled exceptions in other threads
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

        // Handle task unobserved exceptions
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        _loggingService?.LogUnhandledException(e.Exception);

        // Allow Windows Error Reporting to handle the crash
        // Setting Handled = false ensures WER receives the crash data
        e.Handled = false;
    }

    private void OnAppDomainUnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            _loggingService?.LogUnhandledException(ex);
        }
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        _loggingService?.LogUnhandledException(e.Exception);
        _loggingService?.WriteLog(LogLevel.Warning, "App", "Unobserved task exception was logged but not re-thrown");
        e.SetObserved(); // Prevent the process from terminating
    }

    private void OnWindowClosed(object sender, WindowEventArgs args)
    {
        _loggingService?.WriteLog(LogLevel.Information, "App", "Application closing");
        _loggingService?.Flush();
        _loggingService?.Dispose();
        _loggerFactory?.Dispose();
    }
}
