using DefaultApp.Models;
using DefaultApp.Services;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests.Services;

/// <summary>
/// Unit tests for ActivationService.
/// </summary>
public class ActivationServiceTests
{
    private readonly ActivationService _service;

    public ActivationServiceTests()
    {
        _service = new ActivationService();
    }

    [Theory]
    [InlineData(ActivationStatus.Activated, "Activated")]
    [InlineData(ActivationStatus.NotActivated, "Not Activated")]
    [InlineData(ActivationStatus.GracePeriod, "Grace Period")]
    [InlineData(ActivationStatus.NotificationMode, "Notification Mode")]
    [InlineData(ActivationStatus.Checking, "Checking...")]
    [InlineData(ActivationStatus.Unavailable, "Unavailable")]
    public void GetActivationStatusDisplay_ReturnsCorrectStrings(
        ActivationStatus status, string expectedDisplay)
    {
        // Act
        var result = ActivationService.GetActivationStatusDisplay(status);

        // Assert
        result.Should().Be(expectedDisplay);
    }

    [Fact]
    public void GetActivationStatusDisplay_WithUnknownValue_ReturnsUnknown()
    {
        // Arrange - cast an invalid value
        var invalidStatus = (ActivationStatus)999;

        // Act
        var result = ActivationService.GetActivationStatusDisplay(invalidStatus);

        // Assert
        result.Should().Be("Unknown");
    }

    [Fact]
    public async Task ClearCache_ResetsCachedStatus()
    {
        // Arrange - get status to cache it
        var initialStatus = await _service.GetActivationStatusAsync();

        // Act
        _service.ClearCache();

        // Assert - can't directly verify cache is cleared, but verify no exception
        // and that subsequent call still works
        var afterClearStatus = await _service.GetActivationStatusAsync();
        afterClearStatus.Should().NotBe(ActivationStatus.Checking);
    }

    [Fact]
    public async Task GetActivationStatusAsync_ReturnsCachedValueOnSecondCall()
    {
        // Arrange - clear cache first to ensure clean state
        _service.ClearCache();

        // Act
        var firstCall = await _service.GetActivationStatusAsync();
        var secondCall = await _service.GetActivationStatusAsync();

        // Assert
        firstCall.Should().Be(secondCall);
        // Both calls should return the same cached value
        firstCall.Should().NotBe(ActivationStatus.Checking);
    }

    [Fact]
    public async Task GetActivationStatusAsync_ReturnsValidStatus()
    {
        // Arrange
        _service.ClearCache();

        // Act
        var result = await _service.GetActivationStatusAsync();

        // Assert
        result.Should().NotBe(ActivationStatus.Checking);
        // Should be one of the valid final states
        result.Should().BeOneOf(
            ActivationStatus.Activated,
            ActivationStatus.NotActivated,
            ActivationStatus.GracePeriod,
            ActivationStatus.NotificationMode,
            ActivationStatus.Unavailable);
    }

    [Fact]
    public void AllActivationStatusEnumValues_HaveDisplayStrings()
    {
        // Arrange
        var allStatuses = Enum.GetValues<ActivationStatus>();

        // Act & Assert
        foreach (var status in allStatuses)
        {
            var display = ActivationService.GetActivationStatusDisplay(status);
            display.Should().NotBeNullOrWhiteSpace();
        }
    }
}
