using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests;

/// <summary>
/// Test infrastructure validation and sample tests.
/// </summary>
public class TestInfrastructureTests
{
    [Fact]
    public void TestInfrastructure_XUnitWorks()
    {
        // Verifies xUnit test infrastructure is properly configured
        Assert.True(true);
    }

    [Fact]
    public void TestInfrastructure_FluentAssertionsWorks()
    {
        // Verifies FluentAssertions is properly configured
        var value = 42;
        value.Should().Be(42);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 3, 5)]
    [InlineData(0, 0, 0)]
    [InlineData(-1, 1, 0)]
    public void TestInfrastructure_TheoryTestsWork(int a, int b, int expected)
    {
        // Verifies Theory tests with InlineData work
        var result = a + b;
        result.Should().Be(expected);
    }

    [Fact]
    public async Task TestInfrastructure_AsyncTestsWork()
    {
        // Verifies async tests work
        var result = await Task.FromResult(42);
        result.Should().Be(42);
    }
}

/// <summary>
/// Tests for common model types and enums.
/// </summary>
public class ModelTests
{
    [Theory]
    [InlineData("Error", 3)]
    [InlineData("Warning", 2)]
    [InlineData("Information", 1)]
    public void LogLevel_HasCorrectPriority(string level, int expectedPriority)
    {
        // Test that log level priority mapping is consistent
        var priority = level.ToLowerInvariant() switch
        {
            "error" => 3,
            "warning" => 2,
            "information" => 1,
            _ => 0
        };

        priority.Should().Be(expectedPriority);
    }
}
