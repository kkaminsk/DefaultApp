using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DefaultApp.Services;
using Microsoft.Extensions.Logging;

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

    #region Refresh State

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshCommand))]
    private bool _isRefreshing;

    #endregion

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
        _logger?.LogDebug("MainViewModel disposed");
    }
}
