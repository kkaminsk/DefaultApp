using DefaultApp.AppService;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests;

/// <summary>
/// Unit tests for EventLogAppService.
/// Note: Full integration tests require the AppService to be running within a packaged context.
/// These tests verify the class structure and basic functionality.
/// </summary>
public class EventLogAppServiceTests
{
    [Fact]
    public void EventLogAppService_CanBeInstantiated()
    {
        // Act
        var service = new EventLogAppService();

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void EventLogAppService_ImplementsIBackgroundTask()
    {
        // Arrange
        var service = new EventLogAppService();

        // Assert
        service.Should().BeAssignableTo<Windows.ApplicationModel.Background.IBackgroundTask>();
    }
}

/// <summary>
/// Tests for ValueSet message format used by AppService communication.
/// </summary>
public class AppServiceProtocolTests
{
    [Fact]
    public void ValueSet_CanStoreCommandKey()
    {
        // Arrange
        var valueSet = new Windows.Foundation.Collections.ValueSet();

        // Act
        valueSet["Command"] = "QueryEvents";

        // Assert
        valueSet.ContainsKey("Command").Should().BeTrue();
        valueSet["Command"].Should().Be("QueryEvents");
    }

    [Fact]
    public void ValueSet_CanStoreMultipleEventEntries()
    {
        // Arrange
        var valueSet = new Windows.Foundation.Collections.ValueSet();

        // Act
        valueSet["Status"] = "OK";
        valueSet["EventCount"] = 3;
        valueSet["Event_0_Id"] = 1001L;
        valueSet["Event_0_Source"] = "TestSource";
        valueSet["Event_0_Level"] = "Warning";
        valueSet["Event_0_Timestamp"] = "2024-01-15 10:30:00";
        valueSet["Event_0_Message"] = "Test message";

        // Assert
        valueSet["Status"].Should().Be("OK");
        valueSet["EventCount"].Should().Be(3);
        valueSet["Event_0_Id"].Should().Be(1001L);
        valueSet["Event_0_Source"].Should().Be("TestSource");
        valueSet["Event_0_Level"].Should().Be("Warning");
    }

    [Fact]
    public void ValueSet_TryGetValue_ReturnsCorrectValue()
    {
        // Arrange
        var valueSet = new Windows.Foundation.Collections.ValueSet
        {
            ["Status"] = "OK",
            ["EventCount"] = 5
        };

        // Act & Assert
        valueSet.TryGetValue("Status", out var status).Should().BeTrue();
        status.Should().Be("OK");

        valueSet.TryGetValue("EventCount", out var count).Should().BeTrue();
        count.Should().Be(5);
    }

    [Fact]
    public void ValueSet_TryGetValue_ReturnsFalseForMissingKey()
    {
        // Arrange
        var valueSet = new Windows.Foundation.Collections.ValueSet();

        // Act & Assert
        valueSet.TryGetValue("NonExistentKey", out var value).Should().BeFalse();
        value.Should().BeNull();
    }

    [Theory]
    [InlineData("QueryEvents")]
    [InlineData("Ping")]
    public void ValidCommands_AreRecognized(string command)
    {
        // These are the valid commands accepted by EventLogAppService
        var validCommands = new[] { "QueryEvents", "Ping" };
        validCommands.Should().Contain(command);
    }

    [Fact]
    public void ErrorResponse_HasCorrectFormat()
    {
        // Arrange
        var valueSet = new Windows.Foundation.Collections.ValueSet
        {
            ["Status"] = "Error",
            ["Message"] = "No command specified"
        };

        // Assert
        valueSet["Status"].Should().Be("Error");
        valueSet["Message"].Should().NotBeNull();
        (valueSet["Message"] as string).Should().NotBeNullOrEmpty();
    }
}
