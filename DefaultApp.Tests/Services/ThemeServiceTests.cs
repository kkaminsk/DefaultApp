using DefaultApp.Services;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.Services;

/// <summary>
/// Unit tests for ThemeService.
/// Tests static methods that don't require UI initialization.
/// </summary>
public class ThemeServiceTests
{
    [Theory]
    [InlineData(0, AppTheme.SystemDefault)]
    [InlineData(1, AppTheme.Inverted)]
    [InlineData(-1, AppTheme.SystemDefault)]
    [InlineData(2, AppTheme.SystemDefault)]
    [InlineData(100, AppTheme.SystemDefault)]
    public void FromComboBoxIndex_MapsIndicesCorrectly(int index, AppTheme expectedTheme)
    {
        // Act
        var result = ThemeService.FromComboBoxIndex(index);

        // Assert
        result.Should().Be(expectedTheme);
    }

    [Theory]
    [InlineData(AppTheme.SystemDefault, 0)]
    [InlineData(AppTheme.Inverted, 1)]
    public void ToComboBoxIndex_MapsThemesCorrectly(AppTheme theme, int expectedIndex)
    {
        // Act
        var result = ThemeService.ToComboBoxIndex(theme);

        // Assert
        result.Should().Be(expectedIndex);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void RoundTrip_IndexToThemeToIndex_PreservesValue(int originalIndex)
    {
        // Act
        var theme = ThemeService.FromComboBoxIndex(originalIndex);
        var resultIndex = ThemeService.ToComboBoxIndex(theme);

        // Assert
        resultIndex.Should().Be(originalIndex);
    }

    [Theory]
    [InlineData(AppTheme.SystemDefault)]
    [InlineData(AppTheme.Inverted)]
    public void RoundTrip_ThemeToIndexToTheme_PreservesValue(AppTheme originalTheme)
    {
        // Act
        var index = ThemeService.ToComboBoxIndex(originalTheme);
        var resultTheme = ThemeService.FromComboBoxIndex(index);

        // Assert
        resultTheme.Should().Be(originalTheme);
    }

    [Fact]
    public void AllAppThemeValues_HaveValidComboBoxIndices()
    {
        // Arrange
        var themes = Enum.GetValues<AppTheme>();

        // Act & Assert
        foreach (var theme in themes)
        {
            var index = ThemeService.ToComboBoxIndex(theme);
            index.Should().BeGreaterOrEqualTo(0);
        }
    }

    [Fact]
    public void FromComboBoxIndex_InvalidIndex_ReturnsSystemDefault()
    {
        // Act & Assert
        ThemeService.FromComboBoxIndex(-100).Should().Be(AppTheme.SystemDefault);
        ThemeService.FromComboBoxIndex(int.MaxValue).Should().Be(AppTheme.SystemDefault);
        ThemeService.FromComboBoxIndex(int.MinValue).Should().Be(AppTheme.SystemDefault);
    }

    [Fact]
    public void ToComboBoxIndex_UnknownTheme_ReturnsZero()
    {
        // Arrange - cast an invalid value
        var invalidTheme = (AppTheme)999;

        // Act
        var result = ThemeService.ToComboBoxIndex(invalidTheme);

        // Assert
        result.Should().Be(0);
    }
}
