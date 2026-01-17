using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DefaultApp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI;

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
    private readonly NetworkInfoService _networkInfoService;
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
        _networkInfoService = new NetworkInfoService();
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

    #region Network Information Properties

    [ObservableProperty]
    private string _ipAddress = "Loading...";

    [ObservableProperty]
    private string _subnetMask = "Loading...";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PingGatewayCommand))]
    private string _defaultGateway = "Loading...";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PingDnsCommand))]
    private string _dnsServer = "Loading...";

    [ObservableProperty]
    private string _macAddress = "Loading...";

    [ObservableProperty]
    private string _pingButtonText = "Ping";

    [ObservableProperty]
    private Brush? _pingButtonBackground;

    [ObservableProperty]
    private string _pingDnsButtonText = "Ping";

    [ObservableProperty]
    private Brush? _pingDnsButtonBackground;

    [ObservableProperty]
    private string _pingGoogleDnsButtonText = "Ping";

    [ObservableProperty]
    private Brush? _pingGoogleDnsButtonBackground;

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

            // Load network information
            var networkInfo = _networkInfoService.GetNetworkInfo();
            IpAddress = networkInfo.IpAddress;
            SubnetMask = networkInfo.SubnetMask;
            DefaultGateway = networkInfo.DefaultGateway;
            DnsServer = networkInfo.DnsServer;
            MacAddress = networkInfo.MacAddress;

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
            "IpAddress" => IpAddress,
            "SubnetMask" => SubnetMask,
            "DefaultGateway" => DefaultGateway,
            "DnsServer" => DnsServer,
            "MacAddress" => MacAddress,
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

    /// <summary>
    /// Pings the default gateway 5 times and displays the results.
    /// First click resets to "Ping", second click runs the test.
    /// </summary>
    [RelayCommand]
    private void PingGateway()
    {
        // Check if we can ping
        if (string.IsNullOrEmpty(DefaultGateway) ||
            DefaultGateway == "Loading..." ||
            DefaultGateway == "Unavailable")
        {
            return;
        }

        // If showing results, reset to initial state
        if (PingButtonText != "Ping")
        {
            PingButtonText = "Ping";
            PingButtonBackground = null;
            _logger?.LogDebug("Ping button reset to initial state");
            return;
        }

        // Start the ping test
        _logger?.LogInformation("Starting ping to gateway: {Gateway}", DefaultGateway);
        PingButtonText = "0/5";

        // Fire and forget the async ping operation
        _ = ExecutePingAsync();
    }

    private async Task ExecutePingAsync()
    {
        var successCount = 0;
        const int totalPings = 5;

        try
        {
            for (var i = 1; i <= totalPings; i++)
            {
                var success = await _networkInfoService.PingAsync(DefaultGateway);
                if (success)
                {
                    successCount++;
                }
                PingButtonText = $"{successCount}/{i}";
                if (i < totalPings)
                {
                    await Task.Delay(1000);
                }
            }

            _logger?.LogInformation("Ping completed: {Success}/{Total}", successCount, totalPings);

            // Set button color based on results
            PingButtonBackground = successCount switch
            {
                5 => new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),   // Green
                0 => new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),   // Red
                _ => new SolidColorBrush(Color.FromArgb(255, 255, 193, 7))    // Yellow (1-4)
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Ping operation failed");
            PingButtonText = "Error";
            PingButtonBackground = new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)); // Red
        }
    }

    /// <summary>
    /// Pings the DNS server 5 times and displays the results.
    /// First click resets to "Ping", second click runs the test.
    /// </summary>
    [RelayCommand]
    private void PingDns()
    {
        // Check if we can ping
        if (string.IsNullOrEmpty(DnsServer) ||
            DnsServer == "Loading..." ||
            DnsServer == "Unavailable")
        {
            return;
        }

        // If showing results, reset to initial state
        if (PingDnsButtonText != "Ping")
        {
            PingDnsButtonText = "Ping";
            PingDnsButtonBackground = null;
            _logger?.LogDebug("DNS ping button reset to initial state");
            return;
        }

        // Start the ping test
        _logger?.LogInformation("Starting ping to DNS server: {DnsServer}", DnsServer);
        PingDnsButtonText = "0/5";

        // Fire and forget the async ping operation
        _ = ExecutePingDnsAsync();
    }

    private async Task ExecutePingDnsAsync()
    {
        var successCount = 0;
        const int totalPings = 5;

        try
        {
            for (var i = 1; i <= totalPings; i++)
            {
                var success = await _networkInfoService.PingAsync(DnsServer);
                if (success)
                {
                    successCount++;
                }
                PingDnsButtonText = $"{successCount}/{i}";
                if (i < totalPings)
                {
                    await Task.Delay(1000);
                }
            }

            _logger?.LogInformation("DNS ping completed: {Success}/{Total}", successCount, totalPings);

            // Set button color based on results
            PingDnsButtonBackground = successCount switch
            {
                5 => new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),   // Green
                0 => new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),   // Red
                _ => new SolidColorBrush(Color.FromArgb(255, 255, 193, 7))    // Yellow (1-4)
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "DNS ping operation failed");
            PingDnsButtonText = "Error";
            PingDnsButtonBackground = new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)); // Red
        }
    }

    /// <summary>
    /// Pings Google DNS (8.8.8.8) 5 times and displays the results.
    /// First click resets to "Ping", second click runs the test.
    /// </summary>
    [RelayCommand]
    private void PingGoogleDns()
    {
        // If showing results, reset to initial state
        if (PingGoogleDnsButtonText != "Ping")
        {
            PingGoogleDnsButtonText = "Ping";
            PingGoogleDnsButtonBackground = null;
            _logger?.LogDebug("Google DNS ping button reset to initial state");
            return;
        }

        // Start the ping test
        _logger?.LogInformation("Starting ping to Google DNS: 8.8.8.8");
        PingGoogleDnsButtonText = "0/5";

        // Fire and forget the async ping operation
        _ = ExecutePingGoogleDnsAsync();
    }

    private async Task ExecutePingGoogleDnsAsync()
    {
        var successCount = 0;
        const int totalPings = 5;
        const string googleDns = "8.8.8.8";

        try
        {
            for (var i = 1; i <= totalPings; i++)
            {
                var success = await _networkInfoService.PingAsync(googleDns);
                if (success)
                {
                    successCount++;
                }
                PingGoogleDnsButtonText = $"{successCount}/{i}";
                if (i < totalPings)
                {
                    await Task.Delay(1000);
                }
            }

            _logger?.LogInformation("Google DNS ping completed: {Success}/{Total}", successCount, totalPings);

            // Set button color based on results
            PingGoogleDnsButtonBackground = successCount switch
            {
                5 => new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),   // Green
                0 => new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)),   // Red
                _ => new SolidColorBrush(Color.FromArgb(255, 255, 193, 7))    // Yellow (1-4)
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Google DNS ping operation failed");
            PingGoogleDnsButtonText = "Error";
            PingGoogleDnsButtonBackground = new SolidColorBrush(Color.FromArgb(255, 244, 67, 54)); // Red
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
