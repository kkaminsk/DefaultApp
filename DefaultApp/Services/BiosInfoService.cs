using DefaultApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DefaultApp.Services;

/// <summary>
/// Service for retrieving BIOS and security information.
/// </summary>
public sealed class BiosInfoService
{
    private const string BiosRegistryKey = @"HARDWARE\DESCRIPTION\System\BIOS";
    private const string SecureBootRegistryKey = @"SYSTEM\CurrentControlSet\Control\SecureBoot\State";
    private readonly ILogger<BiosInfoService>? _logger;

    public BiosInfoService()
    {
        _logger = App.LoggerFactory?.CreateLogger<BiosInfoService>();
    }

    /// <summary>
    /// Gets complete BIOS and security information.
    /// </summary>
    public BiosInfo GetBiosInfo()
    {
        _logger?.LogDebug("Retrieving BIOS and security information");

        var biosInfo = new BiosInfo
        {
            Manufacturer = GetBiosManufacturer(),
            Name = GetBiosName(),
            Version = GetBiosVersion(),
            ReleaseDate = GetBiosReleaseDate(),
            SmbiosVersion = GetSmbiosVersion(),
            IsSecureBootEnabled = GetSecureBootStatus()
        };

        _logger?.LogInformation("BIOS info retrieved: {Manufacturer} {Version}, Secure Boot: {SecureBoot}",
            biosInfo.Manufacturer, biosInfo.Version, biosInfo.IsSecureBootEnabled ? "Enabled" : "Disabled");

        return biosInfo;
    }

    /// <summary>
    /// Gets the BIOS manufacturer/vendor.
    /// </summary>
    public string GetBiosManufacturer()
    {
        return GetBiosRegistryValue("BIOSVendor");
    }

    /// <summary>
    /// Gets the BIOS name (SystemBiosVersion which contains the full BIOS identifier).
    /// </summary>
    public string GetBiosName()
    {
        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey(BiosRegistryKey);
            if (biosKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for BIOS name");
                return "Unavailable";
            }

            // SystemBiosVersion contains the full BIOS identifier string
            var systemBiosVersion = biosKey.GetValue("SystemBiosVersion");
            if (systemBiosVersion is string[] versions && versions.Length > 0)
            {
                // Often stored as a string array, take the first meaningful entry
                var name = string.Join(" ", versions.Where(v => !string.IsNullOrWhiteSpace(v)));
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name.Trim();
                }
            }

            // Fall back to BaseBoardProduct (motherboard name) if SystemBiosVersion unavailable
            var baseBoardProduct = biosKey.GetValue("BaseBoardProduct") as string;
            if (!string.IsNullOrWhiteSpace(baseBoardProduct))
            {
                return baseBoardProduct.Trim();
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve BIOS name from Registry");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the BIOS version.
    /// </summary>
    public string GetBiosVersion()
    {
        return GetBiosRegistryValue("BIOSVersion");
    }

    /// <summary>
    /// Gets the BIOS release date.
    /// </summary>
    public string GetBiosReleaseDate()
    {
        return GetBiosRegistryValue("BIOSReleaseDate");
    }

    /// <summary>
    /// Gets the SMBIOS version from major and minor release numbers.
    /// </summary>
    public string GetSmbiosVersion()
    {
        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey(BiosRegistryKey);
            if (biosKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for SMBIOS version");
                return "Unavailable";
            }

            var majorRelease = biosKey.GetValue("BiosMajorRelease");
            var minorRelease = biosKey.GetValue("BiosMinorRelease");

            if (majorRelease is int major && minorRelease is int minor)
            {
                return $"{major}.{minor}";
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve SMBIOS version from Registry");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the Secure Boot status.
    /// </summary>
    public bool GetSecureBootStatus()
    {
        try
        {
            using var secureBootKey = Registry.LocalMachine.OpenSubKey(SecureBootRegistryKey);
            if (secureBootKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for Secure Boot status");
                return false;
            }

            var secureBootEnabled = secureBootKey.GetValue("UEFISecureBootEnabled");
            if (secureBootEnabled is int enabled)
            {
                return enabled == 1;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve Secure Boot status from Registry");
            return false;
        }
    }

    private string GetBiosRegistryValue(string valueName)
    {
        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey(BiosRegistryKey);
            if (biosKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for {ValueName}", valueName);
                return "Unavailable";
            }

            var value = biosKey.GetValue(valueName) as string;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve {ValueName} from Registry", valueName);
            return "Unavailable";
        }
    }
}
