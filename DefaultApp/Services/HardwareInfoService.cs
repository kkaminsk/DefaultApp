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
        catch
        {
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
        catch
        {
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
        catch
        {
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
        catch
        {
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
        catch
        {
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
    /// Gets whether the current process is 64-bit.
    /// </summary>
    public bool GetIs64BitProcess()
    {
        try
        {
            return Environment.Is64BitProcess;
        }
        catch
        {
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

    private static ulong GetTotalRamFromMemoryManager()
    {
        try
        {
            // Windows.System.MemoryManager.AppMemoryUsageLimit returns the memory limit for the app
            // For total system memory, we need to use a different approach
            // MemoryManager is sandboxed and doesn't give system-wide info reliably
            // So we'll return 0 to fall through to P/Invoke
            return 0;
        }
        catch
        {
            return 0;
        }
    }

    private static ulong GetTotalRamFromPInvoke()
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
        catch
        {
            return 0;
        }
    }

    private static string FormatBytesAsGb(ulong bytes)
    {
        const double bytesPerGb = 1024.0 * 1024.0 * 1024.0;
        var gb = bytes / bytesPerGb;
        return $"{gb:F1} GB";
    }
}
