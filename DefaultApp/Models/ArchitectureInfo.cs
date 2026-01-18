namespace DefaultApp.Models;

/// <summary>
/// Represents hardware and architecture information.
/// </summary>
public sealed class ArchitectureInfo
{
    /// <summary>
    /// Gets or sets the processor architecture of the current process (e.g., "X64", "Arm64").
    /// </summary>
    public string ProcessArchitecture { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the OS architecture (e.g., "X64", "Arm64").
    /// </summary>
    public string OsArchitecture { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the machine/computer name.
    /// </summary>
    public string MachineName { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the number of logical processors.
    /// </summary>
    public int ProcessorCount { get; set; }

    /// <summary>
    /// Gets or sets the list of CPU model names.
    /// </summary>
    public IReadOnlyList<string> CpuModels { get; set; } = [];

    /// <summary>
    /// Gets or sets the total RAM formatted as GB (e.g., "31.7 GB").
    /// </summary>
    public string TotalRam { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the VRAM formatted as GB (e.g., "8.0 GB").
    /// </summary>
    public string Vram { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the device model name (e.g., "Surface Pro 9", "Dell XPS 15").
    /// </summary>
    public string DeviceModel { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the device serial number.
    /// </summary>
    public string SerialNumber { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets whether the current process is 64-bit.
    /// </summary>
    public bool Is64BitProcess { get; set; }

    /// <summary>
    /// Gets or sets whether the process is running under emulation.
    /// </summary>
    public bool IsRunningUnderEmulation { get; set; }
}
