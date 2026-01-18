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
            Name = GetFullOsDisplayName(),
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
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get OS name");
            return "N/A";
        }
    }

    /// <summary>
    /// Gets a user-friendly OS name (e.g., "Windows 11" or "Windows 10") based on build number.
    /// </summary>
    public string GetFriendlyOsName()
    {
        try
        {
            var buildNumber = Environment.OSVersion.Version.Build;
            // Windows 11 starts at build 22000
            return buildNumber >= 22000 ? "Windows 11" : "Windows 10";
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get friendly OS name");
            return "Windows";
        }
    }

    /// <summary>
    /// Gets the full OS display name combining friendly name with edition (e.g., "Windows 11 Enterprise").
    /// </summary>
    public string GetFullOsDisplayName()
    {
        try
        {
            var friendlyName = GetFriendlyOsName();
            var edition = GetEdition();

            if (string.IsNullOrWhiteSpace(edition) || edition == "N/A")
            {
                return friendlyName;
            }

            // Map EditionID to display name
            var displayEdition = MapEditionToDisplayName(edition);
            return $"{friendlyName} {displayEdition}";
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get full OS display name");
            return "N/A";
        }
    }

    /// <summary>
    /// Maps the Registry EditionID value to a user-friendly display name.
    /// </summary>
    private static string MapEditionToDisplayName(string editionId)
    {
        return editionId switch
        {
            "Enterprise" => "Enterprise",
            "Professional" => "Pro",
            "Education" => "Education",
            "Core" => "Home",
            "CoreN" => "Home N",
            "ProfessionalN" => "Pro N",
            "EnterpriseN" => "Enterprise N",
            "ProfessionalEducation" => "Pro Education",
            "ProfessionalWorkstation" => "Pro for Workstations",
            "EnterpriseS" => "Enterprise LTSC",
            "EnterpriseSN" => "Enterprise LTSC N",
            "ServerStandard" => "Server Standard",
            "ServerDatacenter" => "Server Datacenter",
            _ => editionId
        };
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
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get OS version");
            return "N/A";
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
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get build number");
            return "N/A";
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
                return "N/A";
            }

            var edition = key.GetValue("EditionID") as string;
            return string.IsNullOrWhiteSpace(edition) ? "N/A" : edition;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve Windows edition from Registry");
            return "N/A";
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
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to determine if OS is 64-bit");
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
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get system locale");
            return "N/A";
        }
    }
}
