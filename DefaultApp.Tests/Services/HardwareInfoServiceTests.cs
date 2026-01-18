using DefaultApp.Services;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.Services;

/// <summary>
/// Unit tests for HardwareInfoService.
/// </summary>
public class HardwareInfoServiceTests
{
    private readonly HardwareInfoService _service;

    public HardwareInfoServiceTests()
    {
        _service = new HardwareInfoService();
    }

    [Fact]
    public void GetProcessorArchitecture_ReturnsValidArchitecture()
    {
        // Act
        var result = _service.GetProcessorArchitecture();

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().BeOneOf("X64", "X86", "Arm64", "Arm", "N/A");
    }

    [Fact]
    public void GetOsArchitecture_ReturnsValidArchitecture()
    {
        // Act
        var result = _service.GetOsArchitecture();

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().BeOneOf("X64", "X86", "Arm64", "Arm", "N/A");
    }

    [Fact]
    public void GetMachineName_ReturnsNonEmptyString()
    {
        // Act
        var result = _service.GetMachineName();

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().NotBe("N/A");
    }

    [Fact]
    public void GetProcessorCount_ReturnsPositiveNumber()
    {
        // Act
        var result = _service.GetProcessorCount();

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetCpuModels_ReturnsNonEmptyList()
    {
        // Act
        var result = _service.GetCpuModels();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.First().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetTotalRam_ReturnsFormattedStringWithGB()
    {
        // Act
        var result = _service.GetTotalRam();

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        if (result != "N/A")
        {
            result.Should().EndWith(" GB");
            // Should have format like "31.7 GB"
            result.Should().MatchRegex(@"^\d+\.\d+ GB$");
        }
    }

    [Fact]
    public void GetIs64BitProcess_ReturnsBoolean()
    {
        // Act
        var result = _service.GetIs64BitProcess();

        // Assert - on modern Windows with x64 test runner, expect 64-bit
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("X64", "X64", false)]
    [InlineData("X86", "X64", true)]
    [InlineData("Arm64", "Arm64", false)]
    [InlineData("X64", "Arm64", true)]
    [InlineData("N/A", "X64", false)]
    [InlineData("X64", "N/A", false)]
    public void GetIsRunningUnderEmulation_ReturnsCorrectResult(
        string processArch, string osArch, bool expectedEmulation)
    {
        // Act
        var result = _service.GetIsRunningUnderEmulation(processArch, osArch);

        // Assert
        result.Should().Be(expectedEmulation);
    }

    [Fact]
    public void GetArchitectureInfo_ReturnsCompleteArchitectureInfo()
    {
        // Act
        var result = _service.GetArchitectureInfo();

        // Assert
        result.Should().NotBeNull();
        result.ProcessArchitecture.Should().NotBeNullOrWhiteSpace();
        result.OsArchitecture.Should().NotBeNullOrWhiteSpace();
        result.MachineName.Should().NotBeNullOrWhiteSpace();
        result.ProcessorCount.Should().BeGreaterThan(0);
        result.CpuModels.Should().NotBeEmpty();
        result.TotalRam.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GetDeviceModel_ReturnsStringOrNA()
    {
        // Act
        var result = _service.GetDeviceModel();

        // Assert
        result.Should().NotBeNull();
        // Should be either "N/A" or an actual device model
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetSerialNumber_ReturnsStringOrNA()
    {
        // Act
        var result = _service.GetSerialNumber();

        // Assert
        result.Should().NotBeNull();
        // Should be either "N/A" or an actual serial number
        result.Should().NotBeEmpty();
    }
}
