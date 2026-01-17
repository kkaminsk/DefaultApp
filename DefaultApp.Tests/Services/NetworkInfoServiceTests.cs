using DefaultApp.Services;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.Services;

/// <summary>
/// Unit tests for NetworkInfoService.
/// </summary>
public class NetworkInfoServiceTests
{
    private readonly NetworkInfoService _service;

    public NetworkInfoServiceTests()
    {
        _service = new NetworkInfoService();
    }

    [Fact]
    public void GetNetworkInfo_ReturnsValidNetworkInfoObject()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        result.Should().NotBeNull();
        // Note: Properties might be "Unavailable" if no network is connected
    }

    [Fact]
    public void GetNetworkInfo_IpAddressIsValidOrUnavailable()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        result.IpAddress.Should().NotBeNull();
        if (result.IpAddress != "Unavailable")
        {
            // Should be a valid IPv4 address
            result.IpAddress.Should().MatchRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
        }
    }

    [Fact]
    public async Task PingAsync_WithInvalidAddress_ReturnsFalse()
    {
        // Act
        var result = await _service.PingAsync("invalid.address.that.does.not.exist");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PingAsync_WithUnavailable_ReturnsFalse()
    {
        // Act
        var result = await _service.PingAsync("Unavailable");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PingAsync_WithEmptyString_ReturnsFalse()
    {
        // Act
        var result = await _service.PingAsync("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PingAsync_WithNull_ReturnsFalse()
    {
        // Act
        var result = await _service.PingAsync(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PingAsync_WithLocalhost_ReturnsTrue()
    {
        // Act
        var result = await _service.PingAsync("127.0.0.1");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetNetworkInfo_MacAddressIsFormattedCorrectly()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        if (result.MacAddress != "Unavailable" && !string.IsNullOrEmpty(result.MacAddress))
        {
            // MAC address should be formatted as XX:XX:XX:XX:XX:XX
            result.MacAddress.Should().MatchRegex(@"^([0-9A-F]{2}:){5}[0-9A-F]{2}$");
        }
    }

    [Fact]
    public void GetNetworkInfo_SubnetMaskIsValidOrUnavailable()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        result.SubnetMask.Should().NotBeNull();
        if (result.SubnetMask != "Unavailable")
        {
            // Should be a valid IPv4 address (subnet mask format)
            result.SubnetMask.Should().MatchRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
        }
    }

    [Fact]
    public void GetNetworkInfo_DefaultGatewayIsValidOrUnavailable()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        result.DefaultGateway.Should().NotBeNull();
        if (result.DefaultGateway != "Unavailable")
        {
            // Should be a valid IPv4 address
            result.DefaultGateway.Should().MatchRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
        }
    }

    [Fact]
    public void GetNetworkInfo_DnsServerIsValidOrUnavailable()
    {
        // Act
        var result = _service.GetNetworkInfo();

        // Assert
        result.DnsServer.Should().NotBeNull();
        if (result.DnsServer != "Unavailable")
        {
            // Should be a valid IPv4 address
            result.DnsServer.Should().MatchRegex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
        }
    }
}
