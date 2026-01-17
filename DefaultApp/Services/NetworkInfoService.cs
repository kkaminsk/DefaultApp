using System.Net.NetworkInformation;
using System.Net.Sockets;
using DefaultApp.Models;
using Microsoft.Extensions.Logging;

namespace DefaultApp.Services;

/// <summary>
/// Service for retrieving network configuration information.
/// </summary>
public sealed class NetworkInfoService
{
    private readonly ILogger<NetworkInfoService>? _logger;

    public NetworkInfoService()
    {
        _logger = App.LoggerFactory?.CreateLogger<NetworkInfoService>();
    }

    /// <summary>
    /// Gets network information from the active network adapter.
    /// </summary>
    public NetworkInfo GetNetworkInfo()
    {
        _logger?.LogDebug("Retrieving network information");

        var networkInfo = new NetworkInfo();

        try
        {
            // Find the best active network interface
            var activeInterface = GetActiveNetworkInterface();
            if (activeInterface is null)
            {
                _logger?.LogWarning("No active network interface found");
                return networkInfo;
            }

            _logger?.LogDebug("Using network interface: {Name} ({Type})",
                activeInterface.Name, activeInterface.NetworkInterfaceType);

            // Get IP properties
            var ipProperties = activeInterface.GetIPProperties();

            // Get IPv4 address and subnet mask
            foreach (var unicast in ipProperties.UnicastAddresses)
            {
                if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    networkInfo.IpAddress = unicast.Address.ToString();
                    networkInfo.SubnetMask = unicast.IPv4Mask?.ToString() ?? "Unavailable";
                    break;
                }
            }

            // Get default gateway
            foreach (var gateway in ipProperties.GatewayAddresses)
            {
                if (gateway.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    networkInfo.DefaultGateway = gateway.Address.ToString();
                    break;
                }
            }

            // Get DNS server
            foreach (var dns in ipProperties.DnsAddresses)
            {
                if (dns.AddressFamily == AddressFamily.InterNetwork)
                {
                    networkInfo.DnsServer = dns.ToString();
                    break;
                }
            }

            // Get MAC address
            var macBytes = activeInterface.GetPhysicalAddress().GetAddressBytes();
            if (macBytes.Length > 0)
            {
                networkInfo.MacAddress = string.Join(":", macBytes.Select(b => b.ToString("X2")));
            }

            _logger?.LogInformation("Network info retrieved: IP {IpAddress}, Gateway {Gateway}",
                networkInfo.IpAddress, networkInfo.DefaultGateway);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to retrieve network information");
        }

        return networkInfo;
    }

    private NetworkInterface? GetActiveNetworkInterface()
    {
        // Get all network interfaces that are up and have an IPv4 address
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(ni => ni.OperationalStatus == OperationalStatus.Up
                && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
                && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                && ni.GetIPProperties().UnicastAddresses
                    .Any(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork
                        && !ua.Address.ToString().StartsWith("169.254"))) // Exclude APIPA
            .ToList();

        // Prefer Ethernet over WiFi, WiFi over others
        return interfaces
            .OrderByDescending(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            .ThenByDescending(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            .ThenByDescending(ni => ni.GetIPProperties().GatewayAddresses.Count > 0)
            .FirstOrDefault();
    }
}
