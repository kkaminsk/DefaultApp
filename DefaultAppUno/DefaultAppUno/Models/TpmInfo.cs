namespace DefaultApp.Models;

/// <summary>
/// Represents TPM (Trusted Platform Module) information.
/// </summary>
public sealed class TpmInfo
{
    /// <summary>
    /// Gets or sets the TPM manufacturer identifier.
    /// </summary>
    public string ManufacturerId { get; set; } = "Unavailable";

    /// <summary>
    /// Gets or sets the TPM manufacturer version (firmware version).
    /// </summary>
    public string ManufacturerVersion { get; set; } = "Unavailable";
}
