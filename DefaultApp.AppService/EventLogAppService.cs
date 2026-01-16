using System.Diagnostics;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace DefaultApp.AppService;

/// <summary>
/// App Service that reads Windows Application Event Log entries.
/// Provides bidirectional communication with the main application.
/// </summary>
public sealed class EventLogAppService : IBackgroundTask
{
    private BackgroundTaskDeferral? _deferral;
    private AppServiceConnection? _connection;

    /// <summary>
    /// Entry point for the background task.
    /// </summary>
    public void Run(IBackgroundTaskInstance taskInstance)
    {
        // Get a deferral to keep the service running
        _deferral = taskInstance.GetDeferral();

        // Register for cancellation
        taskInstance.Canceled += OnTaskCanceled;

        // Get the app service trigger details
        if (taskInstance.TriggerDetails is AppServiceTriggerDetails triggerDetails)
        {
            _connection = triggerDetails.AppServiceConnection;
            _connection.RequestReceived += OnRequestReceived;
            _connection.ServiceClosed += OnServiceClosed;
        }
    }

    private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {
        // Get a deferral for async processing
        var messageDeferral = args.GetDeferral();

        try
        {
            var request = args.Request.Message;
            var response = new ValueSet();

            // Check the command type
            if (request.TryGetValue("Command", out var command))
            {
                switch (command?.ToString())
                {
                    case "QueryEvents":
                        await QueryEventsAsync(response);
                        break;

                    case "Ping":
                        response["Status"] = "OK";
                        response["Message"] = "App Service is running";
                        break;

                    default:
                        response["Status"] = "Error";
                        response["Message"] = $"Unknown command: {command}";
                        break;
                }
            }
            else
            {
                response["Status"] = "Error";
                response["Message"] = "No command specified";
            }

            await args.Request.SendResponseAsync(response);
        }
        catch (Exception ex)
        {
            var errorResponse = new ValueSet
            {
                ["Status"] = "Error",
                ["Message"] = ex.Message
            };

            try
            {
                await args.Request.SendResponseAsync(errorResponse);
            }
            catch
            {
                // Connection may be closed
            }
        }
        finally
        {
            messageDeferral.Complete();
        }
    }

    private Task QueryEventsAsync(ValueSet response)
    {
        try
        {
            var events = GetRecentEventLogEntries();

            response["Status"] = "OK";
            response["EventCount"] = events.Count;

            // Add each event to the response
            for (int i = 0; i < events.Count; i++)
            {
                var evt = events[i];
                response[$"Event_{i}_Id"] = evt.EventId;
                response[$"Event_{i}_Source"] = evt.Source;
                response[$"Event_{i}_Level"] = evt.Level;
                response[$"Event_{i}_Timestamp"] = evt.Timestamp;
                response[$"Event_{i}_Message"] = evt.Message;
            }
        }
        catch (Exception ex)
        {
            response["Status"] = "Error";
            response["Message"] = $"Failed to query event log: {ex.Message}";
            response["EventCount"] = 0;
        }

        return Task.CompletedTask;
    }

    private List<EventLogEntry> GetRecentEventLogEntries()
    {
        var entries = new List<EventLogEntry>();
        var cutoffTime = DateTime.Now.AddHours(-24);

        try
        {
            using var eventLog = new EventLog("Application");

            // Get entries in reverse order (most recent first)
            for (int i = eventLog.Entries.Count - 1; i >= 0 && entries.Count < 10; i--)
            {
                try
                {
                    var entry = eventLog.Entries[i];

                    // Filter by time (last 24 hours)
                    if (entry.TimeGenerated < cutoffTime)
                    {
                        break; // Older entries won't be newer
                    }

                    // Filter by level (Warning and Error only)
                    if (entry.EntryType == EventLogEntryType.Warning ||
                        entry.EntryType == EventLogEntryType.Error)
                    {
                        entries.Add(new EventLogEntry
                        {
                            EventId = entry.InstanceId,
                            Source = entry.Source ?? "Unknown",
                            Level = entry.EntryType.ToString(),
                            Timestamp = entry.TimeGenerated.ToString("yyyy-MM-dd HH:mm:ss"),
                            Message = TruncateMessage(entry.Message, 200)
                        });
                    }
                }
                catch
                {
                    // Skip entries that can't be read
                    continue;
                }
            }
        }
        catch (Exception)
        {
            // Return empty list if event log can't be accessed
        }

        return entries;
    }

    private static string TruncateMessage(string? message, int maxLength)
    {
        if (string.IsNullOrEmpty(message))
        {
            return "No message";
        }

        // Replace newlines with spaces for display
        message = message.Replace("\r\n", " ").Replace("\n", " ").Trim();

        if (message.Length <= maxLength)
        {
            return message;
        }

        return message.Substring(0, maxLength - 3) + "...";
    }

    private void OnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
    {
        _deferral?.Complete();
    }

    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
        _connection?.Dispose();
        _deferral?.Complete();
    }

    /// <summary>
    /// Represents a simplified event log entry.
    /// </summary>
    private sealed class EventLogEntry
    {
        public long EventId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
