namespace DefaultApp.Models;

/// <summary>
/// Represents TPM (Trusted Platform Module) information.
/// </summary>
public sealed class TpmInfo
{
    /// <summary>
    /// Gets or sets the TPM specification version.
    /// </summary>
    public string SpecVersion { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the TPM manufacturer identifier.
    /// </summary>
    public string ManufacturerId { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the TPM manufacturer version (firmware version).
    /// </summary>
    public string ManufacturerVersion { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the TPM physical presence version info.
    /// </summary>
    public string PhysicalPresenceVersionInfo { get; set; } = "N/A";
}
