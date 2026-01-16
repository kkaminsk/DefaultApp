using System.Globalization;
using DefaultApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DefaultApp.Services;

/// <summary>
/// Service for retrieving operating system information.
/// </summary>
public sealed class SystemInfoService
{
    private const string WindowsVersionRegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
    private readonly ILogger<SystemInfoService>? _logger;

    public SystemInfoService()
    {
        _logger = App.LoggerFactory?.CreateLogger<SystemInfoService>();
    }

    /// <summary>
    /// Gets complete OS information.
    /// </summary>
    public OsInfo GetOsInfo()
    {
        _logger?.LogDebug("Retrieving OS information");

        var osInfo = new OsInfo
        {
            Name = GetOsName(),
            Version = GetOsVersion(),
            BuildNumber = GetBuildNumber(),
            Edition = GetEdition(),
            Is64BitOs = GetIs64BitOs(),
            SystemLocale = GetSystemLocale(),
            ActivationStatus = ActivationStatus.Checking,
            ActivationStatusDisplay = "Checking..."
        };

        _logger?.LogInformation("OS information retrieved: {Edition} {Version}", osInfo.Edition, osInfo.Version);
        return osInfo;
    }

    /// <summary>
    /// Gets the OS name from the platform.
    /// </summary>
    public string GetOsName()
    {
        try
        {
            return Environment.OSVersion.Platform.ToString();
        }
        catch
        {
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the full OS version string.
    /// </summary>
    public string GetOsVersion()
    {
        try
        {
            return Environment.OSVersion.Version.ToString();
        }
        catch
        {
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the OS build number.
    /// </summary>
    public string GetBuildNumber()
    {
        try
        {
            return Environment.OSVersion.Version.Build.ToString();
        }
        catch
        {
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the Windows edition from the Registry.
    /// </summary>
    public string GetEdition()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(WindowsVersionRegistryKey);
            if (key is null)
            {
                _logger?.LogWarning("Failed to open Registry key for Windows edition");
                return "Unavailable";
            }

            var edition = key.GetValue("EditionID") as string;
            return string.IsNullOrWhiteSpace(edition) ? "Unavailable" : edition;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve Windows edition from Registry");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets whether the OS is 64-bit.
    /// </summary>
    public bool GetIs64BitOs()
    {
        try
        {
            return Environment.Is64BitOperatingSystem;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the system locale.
    /// </summary>
    public string GetSystemLocale()
    {
        try
        {
            return CultureInfo.CurrentCulture.Name;
        }
        catch
        {
            return "Unavailable";
        }
    }
}
