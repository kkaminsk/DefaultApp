namespace DefaultApp.Models;

/// <summary>
/// Represents operating system information.
/// </summary>
public sealed class OsInfo
{
    /// <summary>
    /// Gets or sets the OS name (e.g., "Microsoft Windows NT").
    /// </summary>
    public string Name { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets the full OS version string (e.g., "10.0.22621.0").
    /// </summary>
    public string Version { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets the OS build number (e.g., "22621").
    /// </summary>
    public string BuildNumber { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets the Windows edition (e.g., "Professional", "Enterprise").
    /// </summary>
    public string Edition { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets whether the OS is 64-bit.
    /// </summary>
    public bool Is64BitOs { get; set; }

    /// <summary>
    /// Gets or sets the system locale (e.g., "en-US").
    /// </summary>
    public string SystemLocale { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets the Windows activation status.
    /// </summary>
    public ActivationStatus ActivationStatus { get; set; } = ActivationStatus.Checking;

    /// <summary>
    /// Gets or sets the display string for activation status.
    /// </summary>
    public string ActivationStatusDisplay { get; set; } = "Checking...";
}
