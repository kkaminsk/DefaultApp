using Windows.ApplicationModel.Background;

namespace DefaultApp.BackgroundTasks;

/// <summary>
/// Timer-based background task that provides current time updates.
/// This task runs in-process and uses ApplicationTrigger for on-demand activation.
/// </summary>
public sealed class TimerBackgroundTask : IBackgroundTask
{
    private BackgroundTaskDeferral? _deferral;

    /// <summary>
    /// Event raised when the timer ticks (every second).
    /// </summary>
    public static event EventHandler<DateTime>? TimerTick;

    /// <summary>
    /// Event raised when the task completes or is cancelled.
    /// </summary>
    public static event EventHandler? TaskCompleted;

    /// <summary>
    /// Gets whether the task is currently running.
    /// </summary>
    public static bool IsRunning { get; private set; }

    /// <summary>
    /// Runs the background task.
    /// </summary>
    /// <param name="taskInstance">The background task instance.</param>
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        // Get a deferral to keep the task running
        _deferral = taskInstance.GetDeferral();

        // Register for cancellation
        taskInstance.Canceled += OnCanceled;

        IsRunning = true;

        // Note: The actual timer is managed by ServiceController using DispatcherTimer
        // because we need UI thread access for updating the display.
        // This background task entry point is used for registration and lifecycle management.
    }

    /// <summary>
    /// Raises the TimerTick event. Called by ServiceController.
    /// </summary>
    /// <param name="currentTime">The current time.</param>
    public static void RaiseTimerTick(DateTime currentTime)
    {
        TimerTick?.Invoke(null, currentTime);
    }

    /// <summary>
    /// Completes the background task.
    /// </summary>
    public void Complete()
    {
        IsRunning = false;
        TaskCompleted?.Invoke(this, EventArgs.Empty);
        _deferral?.Complete();
    }

    private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
        IsRunning = false;
        TaskCompleted?.Invoke(this, EventArgs.Empty);
        _deferral?.Complete();
    }
}
