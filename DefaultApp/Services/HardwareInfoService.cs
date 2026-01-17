using System.Runtime.InteropServices;
using DefaultApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace DefaultApp.Services;

/// <summary>
/// Service for retrieving hardware and architecture information.
/// </summary>
public sealed class HardwareInfoService
{
    private const string CentralProcessorRegistryKey = @"HARDWARE\DESCRIPTION\System\CentralProcessor";
    private const string BiosRegistryKey = @"HARDWARE\DESCRIPTION\System\BIOS";
    private readonly ILogger<HardwareInfoService>? _logger;

    public HardwareInfoService()
    {
        _logger = App.LoggerFactory?.CreateLogger<HardwareInfoService>();
    }

    /// <summary>
    /// Gets complete architecture and hardware information.
    /// </summary>
    public ArchitectureInfo GetArchitectureInfo()
    {
        _logger?.LogDebug("Retrieving hardware and architecture information");

        var processArch = GetProcessorArchitecture();
        var osArch = GetOsArchitecture();

        var archInfo = new ArchitectureInfo
        {
            ProcessArchitecture = processArch,
            OsArchitecture = osArch,
            MachineName = GetMachineName(),
            ProcessorCount = GetProcessorCount(),
            CpuModels = GetCpuModels(),
            TotalRam = GetTotalRam(),
            DeviceModel = GetDeviceModel(),
            SerialNumber = GetSerialNumber(),
            Is64BitProcess = GetIs64BitProcess(),
            IsRunningUnderEmulation = GetIsRunningUnderEmulation(processArch, osArch)
        };

        _logger?.LogInformation("Hardware info retrieved: {ProcessorCount} cores, {TotalRam} RAM, {Architecture}",
            archInfo.ProcessorCount, archInfo.TotalRam, archInfo.OsArchitecture);
        return archInfo;
    }

    /// <summary>
    /// Gets the processor architecture of the current process.
    /// </summary>
    public string GetProcessorArchitecture()
    {
        try
        {
            return RuntimeInformation.ProcessArchitecture.ToString();
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get processor architecture");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the OS architecture.
    /// </summary>
    public string GetOsArchitecture()
    {
        try
        {
            return RuntimeInformation.OSArchitecture.ToString();
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get OS architecture");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the machine/computer name.
    /// </summary>
    public string GetMachineName()
    {
        try
        {
            return Environment.MachineName;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get machine name");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the number of logical processors.
    /// </summary>
    public int GetProcessorCount()
    {
        try
        {
            return Environment.ProcessorCount;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get processor count");
            return 0;
        }
    }

    /// <summary>
    /// Gets the list of CPU model names from the Registry.
    /// </summary>
    public IReadOnlyList<string> GetCpuModels()
    {
        var models = new List<string>();

        try
        {
            using var baseKey = Registry.LocalMachine.OpenSubKey(CentralProcessorRegistryKey);
            if (baseKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for CPU models");
                return ["Unavailable"];
            }

            var subKeyNames = baseKey.GetSubKeyNames();
            foreach (var subKeyName in subKeyNames)
            {
                using var processorKey = baseKey.OpenSubKey(subKeyName);
                if (processorKey is null)
                {
                    continue;
                }

                var processorName = processorKey.GetValue("ProcessorNameString") as string;
                if (!string.IsNullOrWhiteSpace(processorName))
                {
                    // Trim extra whitespace that some processors have
                    var trimmedName = processorName.Trim();
                    if (!models.Contains(trimmedName))
                    {
                        models.Add(trimmedName);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve CPU models from Registry");
            return ["Unavailable"];
        }

        return models.Count > 0 ? models : ["Unavailable"];
    }

    /// <summary>
    /// Gets the total RAM formatted as GB with 1 decimal place.
    /// Uses Windows.System.MemoryManager as primary source with MEMORYSTATUSEX fallback.
    /// </summary>
    public string GetTotalRam()
    {
        try
        {
            // Try Windows.System.MemoryManager first
            var totalBytes = GetTotalRamFromMemoryManager();
            if (totalBytes > 0)
            {
                return FormatBytesAsGb(totalBytes);
            }

            // Fallback to MEMORYSTATUSEX P/Invoke
            totalBytes = GetTotalRamFromPInvoke();
            if (totalBytes > 0)
            {
                return FormatBytesAsGb(totalBytes);
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get total RAM");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the device model name from the Registry.
    /// </summary>
    public string GetDeviceModel()
    {
        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey(BiosRegistryKey);
            if (biosKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for device model");
                return "Unavailable";
            }

            var productName = biosKey.GetValue("SystemProductName") as string;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                return productName.Trim();
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve device model from Registry");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets the device serial number.
    /// Tries SMBIOS firmware table first, then falls back to Registry.
    /// </summary>
    public string GetSerialNumber()
    {
        // Try SMBIOS first (most reliable across different hardware)
        var smbiosSerial = GetSerialNumberFromSmbios();
        if (!string.IsNullOrWhiteSpace(smbiosSerial))
        {
            _logger?.LogDebug("Retrieved serial number from SMBIOS: {SerialNumber}", smbiosSerial);
            return smbiosSerial;
        }

        // Fall back to Registry
        try
        {
            using var biosKey = Registry.LocalMachine.OpenSubKey(BiosRegistryKey);
            if (biosKey is null)
            {
                _logger?.LogWarning("Failed to open Registry key for serial number");
                return "Unavailable";
            }

            // Try SystemSerialNumber first
            var serialNumber = biosKey.GetValue("SystemSerialNumber") as string;
            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                return serialNumber.Trim();
            }

            // Fall back to BaseBoardSerialNumber
            serialNumber = biosKey.GetValue("BaseBoardSerialNumber") as string;
            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                return serialNumber.Trim();
            }

            return "Unavailable";
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve serial number from Registry");
            return "Unavailable";
        }
    }

    /// <summary>
    /// Gets whether the current process is 64-bit.
    /// </summary>
    public bool GetIs64BitProcess()
    {
        try
        {
            return Environment.Is64BitProcess;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to determine if process is 64-bit");
            return false;
        }
    }

    /// <summary>
    /// Determines if the process is running under emulation.
    /// </summary>
    public bool GetIsRunningUnderEmulation(string processArchitecture, string osArchitecture)
    {
        if (processArchitecture == "Unavailable" || osArchitecture == "Unavailable")
        {
            return false;
        }

        return !string.Equals(processArchitecture, osArchitecture, StringComparison.OrdinalIgnoreCase);
    }

    private ulong GetTotalRamFromMemoryManager()
    {
        try
        {
            // Windows.System.MemoryManager.AppMemoryUsageLimit returns the memory limit for the app
            // For total system memory, we need to use a different approach
            // MemoryManager is sandboxed and doesn't give system-wide info reliably
            // So we'll return 0 to fall through to P/Invoke
            return 0;
        }
        catch (Exception ex)
        {
            _logger?.LogDebug(ex, "MemoryManager approach failed, falling back to P/Invoke");
            return 0;
        }
    }

    private ulong GetTotalRamFromPInvoke()
    {
        try
        {
            var memStatus = NativeMethods.MEMORYSTATUSEX.Create();
            if (NativeMethods.GlobalMemoryStatusEx(ref memStatus))
            {
                return memStatus.ullTotalPhys;
            }
            return 0;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get total RAM via P/Invoke");
            return 0;
        }
    }

    private static string FormatBytesAsGb(ulong bytes)
    {
        const double bytesPerGb = 1024.0 * 1024.0 * 1024.0;
        var gb = bytes / bytesPerGb;
        return $"{gb:F1} GB";
    }

    /// <summary>
    /// Gets the serial number from SMBIOS firmware table.
    /// </summary>
    private string? GetSerialNumberFromSmbios()
    {
        try
        {
            // First call to get required buffer size
            var size = NativeMethods.GetSystemFirmwareTable(NativeMethods.RSMB, 0, IntPtr.Zero, 0);
            if (size == 0)
            {
                _logger?.LogDebug("GetSystemFirmwareTable returned 0 size");
                return null;
            }

            // Allocate buffer and get the data
            var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                var bytesWritten = NativeMethods.GetSystemFirmwareTable(NativeMethods.RSMB, 0, buffer, size);
                if (bytesWritten == 0)
                {
                    _logger?.LogDebug("GetSystemFirmwareTable failed to write data");
                    return null;
                }

                // Copy to managed array for easier parsing
                var data = new byte[bytesWritten];
                Marshal.Copy(buffer, data, 0, (int)bytesWritten);

                return ParseSmbiosForSerialNumber(data);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to read SMBIOS data");
            return null;
        }
    }

    /// <summary>
    /// Parses SMBIOS data to extract the system serial number from Type 1 (System Information) table.
    /// </summary>
    private string? ParseSmbiosForSerialNumber(byte[] data)
    {
        // SMBIOS raw data header is 8 bytes:
        // Byte 0: Used20CallingMethod
        // Byte 1: SMBIOSMajorVersion
        // Byte 2: SMBIOSMinorVersion
        // Byte 3: DmiRevision
        // Bytes 4-7: Length (DWORD)
        const int headerSize = 8;

        if (data.Length <= headerSize)
        {
            return null;
        }

        var offset = headerSize;

        while (offset < data.Length - 4)
        {
            var type = data[offset];
            var length = data[offset + 1];

            if (length < 4 || offset + length > data.Length)
            {
                break;
            }

            // Type 1 = System Information
            if (type == 1 && length >= 8)
            {
                // Serial number is at offset 7 within the structure (string index)
                var serialNumberIndex = data[offset + 7];

                if (serialNumberIndex == 0)
                {
                    // No serial number string
                    return null;
                }

                // Find the strings section (after the formatted portion)
                var stringsOffset = offset + length;
                return GetSmbiosString(data, stringsOffset, serialNumberIndex);
            }

            // Move to strings section (after the formatted portion)
            var stringStart = offset + length;

            // Skip past strings (each null-terminated, double-null at end)
            while (stringStart < data.Length - 1)
            {
                if (data[stringStart] == 0 && data[stringStart + 1] == 0)
                {
                    // Double null - end of this structure
                    offset = stringStart + 2;
                    break;
                }
                stringStart++;
            }

            if (stringStart >= data.Length - 1)
            {
                break;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a string from the SMBIOS strings section by index (1-based).
    /// </summary>
    private static string? GetSmbiosString(byte[] data, int stringsOffset, int stringIndex)
    {
        if (stringIndex <= 0 || stringsOffset >= data.Length)
        {
            return null;
        }

        var currentIndex = 1;
        var currentOffset = stringsOffset;

        while (currentOffset < data.Length)
        {
            // Find end of current string
            var stringEnd = currentOffset;
            while (stringEnd < data.Length && data[stringEnd] != 0)
            {
                stringEnd++;
            }

            if (currentIndex == stringIndex)
            {
                var length = stringEnd - currentOffset;
                if (length > 0)
                {
                    return System.Text.Encoding.ASCII.GetString(data, currentOffset, length).Trim();
                }
                return null;
            }

            // Move to next string
            currentOffset = stringEnd + 1;
            currentIndex++;

            // Check for double null (end of strings)
            if (currentOffset < data.Length && data[currentOffset] == 0)
            {
                break;
            }
        }

        return null;
    }
}
