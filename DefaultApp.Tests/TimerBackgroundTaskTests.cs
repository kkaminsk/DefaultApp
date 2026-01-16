using DefaultApp.BackgroundTasks;
using FluentAssertions;
using Xunit;

namespace DefaultApp.Tests;

/// <summary>
/// Unit tests for TimerBackgroundTask.
/// </summary>
public class TimerBackgroundTaskTests
{
    [Fact]
    public void TimerBackgroundTask_InitialState_IsNotRunning()
    {
        // The static IsRunning property should initially be false
        // Note: This tests the initial state before any task is started
        TimerBackgroundTask.IsRunning.Should().BeFalse();
    }

    [Fact]
    public void RaiseTimerTick_WithValidDateTime_RaisesEvent()
    {
        // Arrange
        DateTime? receivedTime = null;
        var handler = new EventHandler<DateTime>((sender, time) => receivedTime = time);
        TimerBackgroundTask.TimerTick += handler;

        try
        {
            var testTime = new DateTime(2024, 1, 15, 10, 30, 45);

            // Act
            TimerBackgroundTask.RaiseTimerTick(testTime);

            // Assert
            receivedTime.Should().NotBeNull();
            receivedTime.Should().Be(testTime);
        }
        finally
        {
            // Cleanup
            TimerBackgroundTask.TimerTick -= handler;
        }
    }

    [Fact]
    public void RaiseTimerTick_WithNoSubscribers_DoesNotThrow()
    {
        // Arrange
        var testTime = DateTime.Now;

        // Act & Assert - should not throw even with no subscribers
        var action = () => TimerBackgroundTask.RaiseTimerTick(testTime);
        action.Should().NotThrow();
    }

    [Fact]
    public void TimerBackgroundTask_CanBeInstantiated()
    {
        // Act
        var task = new TimerBackgroundTask();

        // Assert
        task.Should().NotBeNull();
    }

    [Fact]
    public void TimerTick_Event_CanBeSubscribedAndUnsubscribed()
    {
        // Arrange
        var callCount = 0;
        var handler = new EventHandler<DateTime>((sender, time) => callCount++);

        // Act - Subscribe
        TimerBackgroundTask.TimerTick += handler;
        TimerBackgroundTask.RaiseTimerTick(DateTime.Now);

        // Assert
        callCount.Should().Be(1);

        // Act - Unsubscribe
        TimerBackgroundTask.TimerTick -= handler;
        TimerBackgroundTask.RaiseTimerTick(DateTime.Now);

        // Assert - should still be 1 after unsubscribe
        callCount.Should().Be(1);
    }
}
