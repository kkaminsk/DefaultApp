using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DefaultApp.Services;
using Microsoft.Extensions.Logging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace DefaultApp.ViewModels;

/// <summary>
/// ViewModel for the main page displaying system and hardware information.
/// </summary>
public partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly SystemInfoService _systemInfoService;
    private readonly HardwareInfoService _hardwareInfoService;
    private readonly ActivationService _activationService;
    private readonly BiosInfoService _biosInfoService;
    private readonly TpmInfoService _tpmInfoService;
    private readonly ILogger<MainViewModel>? _logger;
    private readonly MediaPlayer _mediaPlayer;
    private bool _isDisposed;

    public MainViewModel()
    {
        _systemInfoService = new SystemInfoService();
        _hardwareInfoService = new HardwareInfoService();
        _activationService = new ActivationService();
        _biosInfoService = new BiosInfoService();
        _tpmInfoService = new TpmInfoService();
        _logger = App.LoggerFactory?.CreateLogger<MainViewModel>();
        _mediaPlayer = new MediaPlayer();
    }

    #region OS Information Properties

    [ObservableProperty]
    private string _osName = "Loading...";

    [ObservableProperty]
    private string _osVersion = "Loading...";

    [ObservableProperty]
    private string _osBuildNumber = "Loading...";

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
    private string _vram = "Loading...";

    [ObservableProperty]
    private string _deviceModel = "Loading...";

    [ObservableProperty]
    private string _serialNumber = "Loading...";

    [ObservableProperty]
    private string _is64BitProcess = "Loading...";

    #endregion

    #region BIOS Information Properties

    [ObservableProperty]
    private string _biosManufacturer = "Loading...";

    [ObservableProperty]
    private string _biosName = "Loading...";

    [ObservableProperty]
    private string _biosVersion = "Loading...";

    [ObservableProperty]
    private string _biosReleaseDate = "Loading...";

    [ObservableProperty]
    private string _smbiosVersion = "Loading...";

    [ObservableProperty]
    private string _secureBootStatus = "Loading...";

    #endregion

    #region TPM Information Properties

    [ObservableProperty]
    private string _tpmManufacturerId = "Loading...";

    [ObservableProperty]
    private string _tpmManufacturerVersion = "Loading...";

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
            DeviceModel = archInfo.DeviceModel;
            SerialNumber = archInfo.SerialNumber;
            TotalRam = archInfo.TotalRam;
            Vram = archInfo.Vram;
            Is64BitProcess = archInfo.Is64BitProcess ? "Yes" : "No";

            // Load BIOS information
            var biosInfo = _biosInfoService.GetBiosInfo();
            BiosManufacturer = biosInfo.Manufacturer;
            BiosName = biosInfo.Name;
            BiosVersion = biosInfo.Version;
            BiosReleaseDate = biosInfo.ReleaseDate;
            SmbiosVersion = biosInfo.SmbiosVersion;
            SecureBootStatus = biosInfo.IsSecureBootEnabled ? "Enabled" : "Disabled";

            // Load TPM information
            var tpmInfo = _tpmInfoService.GetTpmInfo();
            TpmManufacturerId = tpmInfo.ManufacturerId;
            TpmManufacturerVersion = tpmInfo.ManufacturerVersion;

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
    /// Copies the specified field value to the clipboard.
    /// </summary>
    /// <param name="fieldName">The name of the field to copy (e.g., "MachineName", "Version").</param>
    [RelayCommand]
    private void CopyToClipboard(string fieldName)
    {
        var value = fieldName switch
        {
            "MachineName" => MachineName,
            "Version" => OsVersion,
            "OsName" => OsName,
            "Build" => OsBuildNumber,
            "SystemLocale" => SystemLocale,
            "CpuModel" => CpuModel,
            "DeviceModel" => DeviceModel,
            "SerialNumber" => SerialNumber,
            "TotalRam" => TotalRam,
            "Vram" => Vram,
            "BiosManufacturer" => BiosManufacturer,
            "BiosName" => BiosName,
            "BiosVersion" => BiosVersion,
            "BiosReleaseDate" => BiosReleaseDate,
            "SmbiosVersion" => SmbiosVersion,
            "TpmManufacturerId" => TpmManufacturerId,
            "TpmManufacturerVersion" => TpmManufacturerVersion,
            _ => null
        };

        if (string.IsNullOrEmpty(value) || value == "Loading..." || value == "Unavailable")
        {
            _logger?.LogWarning("Cannot copy empty or unavailable value for field: {Field}", fieldName);
            return;
        }

        try
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(value);
            Clipboard.SetContent(dataPackage);
            _logger?.LogDebug("Copied {Field} to clipboard: {Value}", fieldName, value);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to copy {Field} to clipboard", fieldName);
        }
    }

    /// <summary>
    /// Plays the audio file.
    /// </summary>
    [RelayCommand]
    private void PlayAudio()
    {
        try
        {
            _logger?.LogInformation("Playing audio file");

            // Get the path to the audio file relative to the executable
            var exePath = AppContext.BaseDirectory;
            var audioPath = Path.Combine(exePath, "Assets", "Audio", "Testing_Final.mp3");

            _logger?.LogDebug("Audio file path: {Path}", audioPath);

            if (!File.Exists(audioPath))
            {
                _logger?.LogWarning("Audio file not found at: {Path}", audioPath);
                return;
            }

            var uri = new Uri(audioPath);
            _mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            _mediaPlayer.Play();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to play audio");
        }
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

        _mediaPlayer.Dispose();
        _isDisposed = true;
        _logger?.LogDebug("MainViewModel disposed");
    }
}
