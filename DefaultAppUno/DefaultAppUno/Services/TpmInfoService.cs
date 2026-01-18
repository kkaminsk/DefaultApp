using System.Text;
using DefaultApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DefaultApp.Services;

/// <summary>
/// Service for retrieving TPM (Trusted Platform Module) information.
/// </summary>
public sealed class TpmInfoService
{
    private const string TpmRegistryKey = @"SYSTEM\CurrentControlSet\Services\TPM\WMI";
    private readonly ILogger<TpmInfoService>? _logger;

    public TpmInfoService()
    {
        _logger = App.LoggerFactory?.CreateLogger<TpmInfoService>();
    }

    /// <summary>
    /// Gets TPM information from Registry.
    /// </summary>
    public TpmInfo GetTpmInfo()
    {
        _logger?.LogDebug("Retrieving TPM information from Registry");

        var tpmInfo = new TpmInfo();

        try
        {
            using var tpmKey = Registry.LocalMachine.OpenSubKey(TpmRegistryKey);
            if (tpmKey is null)
            {
                _logger?.LogWarning("TPM Registry key not found - TPM may not be present");
                return tpmInfo;
            }

            // Get ManufacturerId - stored as DWORD, convert to manufacturer name
            var manufacturerIdValue = tpmKey.GetValue("TaskManufacturerId");
            if (manufacturerIdValue is int manufacturerId)
            {
                tpmInfo.ManufacturerId = ConvertManufacturerIdToName(manufacturerId);
            }

            // Get ManufacturerVersion - stored as FirmwareVersionAtLastProvision or TaskFirmwareVersion
            var firmwareVersion = tpmKey.GetValue("FirmwareVersionAtLastProvision") as string
                ?? tpmKey.GetValue("TaskFirmwareVersion") as string;
            if (!string.IsNullOrWhiteSpace(firmwareVersion))
            {
                tpmInfo.ManufacturerVersion = firmwareVersion;
            }

            _logger?.LogInformation("TPM info retrieved: Manufacturer {ManufacturerId}, Version {ManufacturerVersion}",
                tpmInfo.ManufacturerId, tpmInfo.ManufacturerVersion);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve TPM information from Registry");
        }

        return tpmInfo;
    }

    private string ConvertManufacturerIdToName(int manufacturerId)
    {
        // ManufacturerId is a 4-byte ASCII string packed into a DWORD
        try
        {
            var bytes = BitConverter.GetBytes(manufacturerId);
            // The bytes are in little-endian order, reverse for string
            Array.Reverse(bytes);
            var name = Encoding.ASCII.GetString(bytes).TrimEnd('\0', ' ');

            if (!string.IsNullOrWhiteSpace(name))
            {
                // Map short codes to full names
                return name.ToUpperInvariant() switch
                {
                    "AMD" => "AMD",
                    "INTC" => "Intel",
                    "IFX" => "Infineon",
                    "STM" => "STMicroelectronics",
                    "MSFT" => "Microsoft",
                    "NTC" => "Nuvoton",
                    "ATML" => "Atmel",
                    _ => name
                };
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to convert TPM manufacturer ID {ManufacturerId} to name, using hex representation", manufacturerId);
        }

        return $"0x{manufacturerId:X8}";
    }
}
