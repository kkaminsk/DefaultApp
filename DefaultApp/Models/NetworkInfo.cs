namespace DefaultApp.Models;

/// <summary>
/// Represents network configuration information.
/// </summary>
public sealed class NetworkInfo
{
    /// <summary>
    /// Gets or sets the IP address.
    /// </summary>
    public string IpAddress { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the subnet mask.
    /// </summary>
    public string SubnetMask { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the default gateway.
    /// </summary>
    public string DefaultGateway { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the DNS server.
    /// </summary>
    public string DnsServer { get; set; } = "N/A";

    /// <summary>
    /// Gets or sets the MAC address.
    /// </summary>
    public string MacAddress { get; set; } = "N/A";
}
