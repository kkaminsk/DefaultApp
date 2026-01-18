namespace DefaultApp.Models;

/// <summary>
/// Represents BIOS and security information.
/// </summary>
public sealed class BiosInfo
{
    /// <summary>
    /// Gets or sets the BIOS manufacturer/vendor.
    /// </summary>
    public string Manufacturer { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the BIOS name (typically same as version string).
    /// </summary>
    public string Name { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the BIOS version.
    /// </summary>
    public string Version { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the BIOS release date.
    /// </summary>
    public string ReleaseDate { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the SMBIOS version (format: Major.Minor).
    /// </summary>
    public string SmbiosVersion { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets whether Secure Boot is enabled.
    /// </summary>
    public bool IsSecureBootEnabled { get; set; }
}
