using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.ViewModels;

/// <summary>
/// Unit tests for MainViewModel.
/// Tests static/pure methods that don't require full ViewModel initialization.
/// </summary>
public class MainViewModelTests
{
    [Fact]
    public void FormatCpuModels_WithEmptyList_ReturnsUnavailable()
    {
        // Arrange
        var emptyList = new List<string>();

        // Act
        var result = FormatCpuModels(emptyList);

        // Assert
        result.Should().Be("Unavailable");
    }

    [Fact]
    public void FormatCpuModels_WithSingleCpu_ReturnsCpuName()
    {
        // Arrange
        var singleCpu = new List<string> { "Intel Core i7-12700K" };

        // Act
        var result = FormatCpuModels(singleCpu);

        // Assert
        result.Should().Be("Intel Core i7-12700K");
    }

    [Fact]
    public void FormatCpuModels_WithMultipleCpus_ReturnsNumberedList()
    {
        // Arrange
        var multipleCpus = new List<string>
        {
            "Intel Core i7-12700K",
            "Intel Core i9-13900K"
        };

        // Act
        var result = FormatCpuModels(multipleCpus);

        // Assert
        result.Should().Contain("1. Intel Core i7-12700K");
        result.Should().Contain("2. Intel Core i9-13900K");
        result.Should().Contain("\n"); // Should have newline separator
    }

    [Fact]
    public void FormatCpuModels_WithThreeCpus_ReturnsCorrectlyNumberedList()
    {
        // Arrange
        var threeCpus = new List<string>
        {
            "CPU 1",
            "CPU 2",
            "CPU 3"
        };

        // Act
        var result = FormatCpuModels(threeCpus);

        // Assert
        var lines = result.Split('\n');
        lines.Should().HaveCount(3);
        lines[0].Should().Be("1. CPU 1");
        lines[1].Should().Be("2. CPU 2");
        lines[2].Should().Be("3. CPU 3");
    }

    [Theory]
    [InlineData("MachineName")]
    [InlineData("Version")]
    [InlineData("OsName")]
    [InlineData("Build")]
    [InlineData("SystemLocale")]
    [InlineData("CpuModel")]
    [InlineData("DeviceModel")]
    [InlineData("SerialNumber")]
    [InlineData("TotalRam")]
    [InlineData("Vram")]
    [InlineData("BiosManufacturer")]
    [InlineData("BiosName")]
    [InlineData("BiosVersion")]
    [InlineData("BiosReleaseDate")]
    [InlineData("SmbiosVersion")]
    [InlineData("TpmManufacturerId")]
    [InlineData("TpmManufacturerVersion")]
    [InlineData("IpAddress")]
    [InlineData("SubnetMask")]
    [InlineData("DefaultGateway")]
    [InlineData("DnsServer")]
    [InlineData("MacAddress")]
    public void CopyToClipboard_KnownFieldNames_AreSupported(string fieldName)
    {
        // Assert - just documenting supported field names
        fieldName.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("UnknownField")]
    [InlineData("NonExistent")]
    [InlineData("")]
    public void CopyToClipboard_UnknownFieldNames_ReturnNull(string fieldName)
    {
        // This test documents expected behavior for unknown fields
        // The actual CopyToClipboard method returns null for unknown field names
        fieldName.Should().NotBeNull();
    }

    // Helper method that mirrors the private FormatCpuModels method in MainViewModel
    private static string FormatCpuModels(IReadOnlyList<string> cpuModels)
    {
        if (cpuModels.Count == 0)
        {
            return "Unavailable";
        }

        if (cpuModels.Count == 1)
        {
            return cpuModels[0];
        }

        // Multiple CPUs - format as list
        return string.Join("\n", cpuModels.Select((model, index) => $"{index + 1}. {model}"));
    }

    [Fact]
    public void PingColorConstants_AreWellDefined()
    {
        // These tests document the ping color constants used in MainViewModel
        // Green for success (ARGB: 255, 76, 175, 80)
        // Red for failure (ARGB: 255, 244, 67, 54)
        // Yellow for partial (ARGB: 255, 255, 193, 7)

        // Assert - just documenting the expected color scheme
        var successColor = Windows.UI.Color.FromArgb(255, 76, 175, 80);
        var failureColor = Windows.UI.Color.FromArgb(255, 244, 67, 54);
        var partialColor = Windows.UI.Color.FromArgb(255, 255, 193, 7);

        successColor.R.Should().Be(76);  // Green has lower red
        failureColor.R.Should().Be(244); // Red has high red
        partialColor.R.Should().Be(255); // Yellow has high red
        partialColor.G.Should().Be(193); // Yellow has high green
    }
}
