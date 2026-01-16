using System.Diagnostics;

namespace DefaultApp.FullTrustProcess;

/// <summary>
/// Service for writing events to the Windows Event Log.
/// Registers a custom event source and tracks events written.
/// </summary>
public sealed class EventLoggerService : IDisposable
{
    private const string EventSourceName = "DefaultApp";
    private const string LogName = "Application";

    private readonly object _lock = new();
    private bool _isDisposed;
    private int _eventsWritten;

    public EventLoggerService()
    {
        StartTime = DateTime.Now;
        MinimumLevel = "Warning"; // Default to Errors and Warnings only

        // Try to register the event source (requires admin on first run)
        EnsureEventSourceExists();
    }

    /// <summary>
    /// Gets the time the service was started.
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// Gets or sets the minimum log level (Error, Warning, Information).
    /// </summary>
    public string MinimumLevel { get; set; }

    /// <summary>
    /// Gets the total number of events written.
    /// </summary>
    public int EventsWritten => _eventsWritten;

    /// <summary>
    /// Writes an event to the Windows Event Log.
    /// </summary>
    /// <param name="message">The event message.</param>
    /// <param name="level">The event level (Error, Warning, Information).</param>
    /// <param name="eventId">The event ID.</param>
    public void WriteEvent(string message, string level, int eventId = 1000)
    {
        // Check if level meets minimum threshold
        if (!ShouldLog(level))
        {
            Console.WriteLine($"Skipped event (below minimum level): {level}");
            return;
        }

        var entryType = level.ToLowerInvariant() switch
        {
            "error" => EventLogEntryType.Error,
            "warning" => EventLogEntryType.Warning,
            _ => EventLogEntryType.Information
        };

        try
        {
            EventLog.WriteEntry(EventSourceName, message, entryType, eventId);

            lock (_lock)
            {
                _eventsWritten++;
            }

            Console.WriteLine($"Event logged: [{level}] {message} (Total: {_eventsWritten})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write event log: {ex.Message}");
        }
    }

    /// <summary>
    /// Determines if the given level should be logged based on minimum level.
    /// </summary>
    private bool ShouldLog(string level)
    {
        var levelPriority = GetLevelPriority(level);
        var minimumPriority = GetLevelPriority(MinimumLevel);

        return levelPriority >= minimumPriority;
    }

    private static int GetLevelPriority(string level)
    {
        return level.ToLowerInvariant() switch
        {
            "error" => 3,
            "warning" => 2,
            "information" => 1,
            _ => 0
        };
    }

    private static void EnsureEventSourceExists()
    {
        try
        {
            if (!EventLog.SourceExists(EventSourceName))
            {
                Console.WriteLine($"Creating event source: {EventSourceName}");
                EventLog.CreateEventSource(EventSourceName, LogName);
                Console.WriteLine("Event source created successfully.");
            }
            else
            {
                Console.WriteLine($"Event source already exists: {EventSourceName}");
            }
        }
        catch (Exception ex)
        {
            // This typically happens if not running as admin on first run
            Console.WriteLine($"Could not verify/create event source: {ex.Message}");
            Console.WriteLine("Events may still be logged if the source was previously created.");
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
    }
}
