using DefaultApp.Services;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.Services;

/// <summary>
/// Unit tests for BiosInfoService.
/// </summary>
public class BiosInfoServiceTests
{
    private readonly BiosInfoService _service;

    public BiosInfoServiceTests()
    {
        _service = new BiosInfoService();
    }

    [Fact]
    public void GetBiosInfo_ReturnsValidBiosInfoObject()
    {
        // Act
        var result = _service.GetBiosInfo();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void GetBiosManufacturer_ReturnsStringOrUnavailable()
    {
        // Act
        var result = _service.GetBiosManufacturer();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetBiosName_ReturnsStringOrUnavailable()
    {
        // Act
        var result = _service.GetBiosName();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetBiosVersion_ReturnsStringOrUnavailable()
    {
        // Act
        var result = _service.GetBiosVersion();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetBiosReleaseDate_ReturnsStringOrUnavailable()
    {
        // Act
        var result = _service.GetBiosReleaseDate();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetSecureBootStatus_ReturnsBoolean()
    {
        // Act
        var result = _service.GetSecureBootStatus();

        // Assert - result is a boolean, verify no exception thrown
        // Boolean can only be true or false, so just check it's one of them
        (result == true || result == false).Should().BeTrue();
    }

    [Fact]
    public void GetSmbiosVersion_ReturnsVersionStringOrUnavailable()
    {
        // Act
        var result = _service.GetSmbiosVersion();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();

        if (result != "Unavailable")
        {
            // Should be in format "X.Y"
            result.Should().MatchRegex(@"^\d+\.\d+$");
        }
    }

    [Fact]
    public void GetBiosInfo_AllPropertiesArePopulated()
    {
        // Act
        var result = _service.GetBiosInfo();

        // Assert
        result.Manufacturer.Should().NotBeNull();
        result.Name.Should().NotBeNull();
        result.Version.Should().NotBeNull();
        result.ReleaseDate.Should().NotBeNull();
        result.SmbiosVersion.Should().NotBeNull();
        // IsSecureBootEnabled is a boolean, always has a value
    }

    [Fact]
    public void GetBiosInfo_SecureBootStatusMatchesDirectCall()
    {
        // Act
        var biosInfo = _service.GetBiosInfo();
        var directSecureBootStatus = _service.GetSecureBootStatus();

        // Assert
        biosInfo.IsSecureBootEnabled.Should().Be(directSecureBootStatus);
    }

    [Fact]
    public void GetBiosInfo_SmbiosVersionMatchesDirectCall()
    {
        // Act
        var biosInfo = _service.GetBiosInfo();
        var directSmbiosVersion = _service.GetSmbiosVersion();

        // Assert
        biosInfo.SmbiosVersion.Should().Be(directSmbiosVersion);
    }
}
