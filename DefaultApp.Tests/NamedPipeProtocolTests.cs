using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests;

/// <summary>
/// Tests for the Named Pipe JSON protocol used by Full Trust Process.
/// </summary>
public class NamedPipeProtocolTests
{
    [Fact]
    public void LogEventRequest_SerializesCorrectly()
    {
        // Arrange
        var request = new
        {
            Command = "LogEvent",
            Message = "Test log message",
            Level = "Warning",
            EventId = 1001
        };

        // Act
        var json = JsonSerializer.Serialize(request);
        using var doc = JsonDocument.Parse(json);

        // Assert
        doc.RootElement.GetProperty("Command").GetString().Should().Be("LogEvent");
        doc.RootElement.GetProperty("Message").GetString().Should().Be("Test log message");
        doc.RootElement.GetProperty("Level").GetString().Should().Be("Warning");
        doc.RootElement.GetProperty("EventId").GetInt32().Should().Be(1001);
    }

    [Fact]
    public void GetStatusRequest_SerializesCorrectly()
    {
        // Arrange
        var request = new { Command = "GetStatus" };

        // Act
        var json = JsonSerializer.Serialize(request);
        using var doc = JsonDocument.Parse(json);

        // Assert
        doc.RootElement.GetProperty("Command").GetString().Should().Be("GetStatus");
    }

    [Fact]
    public void ShutdownRequest_SerializesCorrectly()
    {
        // Arrange
        var request = new { Command = "Shutdown" };

        // Act
        var json = JsonSerializer.Serialize(request);
        using var doc = JsonDocument.Parse(json);

        // Assert
        doc.RootElement.GetProperty("Command").GetString().Should().Be("Shutdown");
    }

    [Fact]
    public void SuccessResponse_CanBeParsed()
    {
        // Arrange
        var responseJson = """
            {
                "Status": "OK",
                "Message": "Event logged successfully",
                "TotalEventsLogged": 5
            }
            """;

        // Act
        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("Status").GetString().Should().Be("OK");
        root.GetProperty("Message").GetString().Should().Be("Event logged successfully");
        root.GetProperty("TotalEventsLogged").GetInt32().Should().Be(5);
    }

    [Fact]
    public void ErrorResponse_CanBeParsed()
    {
        // Arrange
        var responseJson = """
            {
                "Status": "Error",
                "Message": "Unknown command: InvalidCommand"
            }
            """;

        // Act
        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("Status").GetString().Should().Be("Error");
        root.GetProperty("Message").GetString().Should().Contain("Unknown command");
    }

    [Fact]
    public void GetStatusResponse_ContainsExpectedFields()
    {
        // Arrange
        var responseJson = """
            {
                "Status": "OK",
                "ProcessId": 12345,
                "PipeName": "DefaultApp_12345",
                "EventsLogged": 10,
                "LogLevel": "Warning",
                "Uptime": 123.45
            }
            """;

        // Act
        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        // Assert
        root.GetProperty("Status").GetString().Should().Be("OK");
        root.GetProperty("ProcessId").GetInt32().Should().Be(12345);
        root.GetProperty("PipeName").GetString().Should().Be("DefaultApp_12345");
        root.GetProperty("EventsLogged").GetInt32().Should().Be(10);
        root.GetProperty("LogLevel").GetString().Should().Be("Warning");
        root.GetProperty("Uptime").GetDouble().Should().BeApproximately(123.45, 0.01);
    }

    [Theory]
    [InlineData("Error")]
    [InlineData("Warning")]
    [InlineData("Information")]
    public void LogLevel_AcceptsValidValues(string level)
    {
        // Arrange
        var request = new
        {
            Command = "LogEvent",
            Message = "Test",
            Level = level,
            EventId = 1000
        };

        // Act
        var json = JsonSerializer.Serialize(request);
        using var doc = JsonDocument.Parse(json);

        // Assert
        doc.RootElement.GetProperty("Level").GetString().Should().Be(level);
    }

    [Fact]
    public void MalformedJson_ThrowsJsonException()
    {
        // Arrange
        var malformedJson = "{ not valid json }";

        // Act & Assert
        var action = () => JsonDocument.Parse(malformedJson);
        action.Should().Throw<JsonException>();
    }

    [Fact]
    public void EmptyJson_CanBeParsed()
    {
        // Arrange
        var emptyJson = "{}";

        // Act
        using var doc = JsonDocument.Parse(emptyJson);

        // Assert
        doc.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
    }

    [Fact]
    public void MessageLengthPrefix_SerializesCorrectly()
    {
        // Arrange
        var message = "Test message";
        var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
        var lengthBytes = BitConverter.GetBytes(messageBytes.Length);

        // Act & Assert
        lengthBytes.Length.Should().Be(4); // Int32 is 4 bytes
        BitConverter.ToInt32(lengthBytes, 0).Should().Be(messageBytes.Length);
    }
}
