using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;

namespace DefaultApp.Services;

/// <summary>
/// Central logging service that handles file output, log rotation, and ETW integration.
/// </summary>
public sealed class LoggingService : IDisposable
{
    private const string LogFolderName = "Logs";
    private const string LogFilePrefix = "DefaultApp_";
    private const string LogFileExtension = ".log";
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
    private const int MaxRetainedFiles = 5;

    private readonly string _logDirectory;
    private readonly object _writeLock = new();
    private readonly DefaultAppEventSource _eventSource;
    private StreamWriter? _currentWriter;
    private string? _currentLogFilePath;
    private long _currentFileSize;
    private bool _disposed;

    /// <summary>
    /// Gets or sets the minimum log level to record.
    /// </summary>
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// Initializes the logging service.
    /// </summary>
    public LoggingService()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _logDirectory = Path.Combine(localAppData, "DefaultApp", LogFolderName);
        _eventSource = new DefaultAppEventSource();

        EnsureLogDirectoryExists();
        InitializeLogFile();

#if DEBUG
        MinimumLogLevel = LogLevel.Debug;
#endif
    }

    /// <summary>
    /// Writes a log entry to the file and ETW.
    /// </summary>
    public void WriteLog(LogLevel level, string category, string message, Exception? exception = null)
    {
        if (_disposed)
        {
            return;
        }

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var levelString = GetLogLevelString(level);
        var logEntry = FormatLogEntry(timestamp, levelString, category, message, exception);

        // Write to file
        WriteToFile(logEntry);

        // Write to ETW
        WriteToEtw(level, category, message, exception);

        // Debug output in debug builds
#if DEBUG
        Debug.WriteLine(logEntry);
#endif
    }

    /// <summary>
    /// Logs a service start event.
    /// </summary>
    public void LogServiceStart(string serviceName)
    {
        WriteLog(LogLevel.Information, "Audit", $"Service started: {serviceName}");
        _eventSource.ServiceStarted(serviceName);
    }

    /// <summary>
    /// Logs a service stop event.
    /// </summary>
    public void LogServiceStop(string serviceName)
    {
        WriteLog(LogLevel.Information, "Audit", $"Service stopped: {serviceName}");
        _eventSource.ServiceStopped(serviceName);
    }

    /// <summary>
    /// Logs a service state transition.
    /// </summary>
    public void LogServiceStateChange(string serviceName, string fromState, string toState)
    {
        WriteLog(LogLevel.Information, "Audit", $"Service '{serviceName}' state changed: {fromState} -> {toState}");
        _eventSource.ServiceStateChanged(serviceName, fromState, toState);
    }

    /// <summary>
    /// Logs an unhandled exception (for crash reporting).
    /// </summary>
    public void LogUnhandledException(Exception exception)
    {
        WriteLog(LogLevel.Critical, "Crash", "Unhandled exception occurred", exception);
        _eventSource.UnhandledException(exception.GetType().Name, exception.Message, exception.StackTrace ?? "");

        // Ensure the log is flushed before the application crashes
        Flush();
    }

    /// <summary>
    /// Flushes any buffered log entries to disk.
    /// </summary>
    public void Flush()
    {
        lock (_writeLock)
        {
            _currentWriter?.Flush();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        lock (_writeLock)
        {
            _currentWriter?.Flush();
            _currentWriter?.Dispose();
            _currentWriter = null;
        }

        _eventSource.Dispose();
    }

    private void EnsureLogDirectoryExists()
    {
        try
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        catch
        {
            // Silently fail if we can't create the directory
        }
    }

    private void InitializeLogFile()
    {
        try
        {
            var fileName = $"{LogFilePrefix}{DateTime.Now:yyyyMMdd}{LogFileExtension}";
            _currentLogFilePath = Path.Combine(_logDirectory, fileName);

            // Check if file exists and get its size
            if (File.Exists(_currentLogFilePath))
            {
                var fileInfo = new FileInfo(_currentLogFilePath);
                _currentFileSize = fileInfo.Length;

                // If file is too large, rotate immediately
                if (_currentFileSize >= MaxFileSizeBytes)
                {
                    RotateLogFile();
                    return;
                }
            }
            else
            {
                _currentFileSize = 0;
            }

            _currentWriter = new StreamWriter(_currentLogFilePath, append: true, Encoding.UTF8)
            {
                AutoFlush = true
            };

            // Cleanup old files
            CleanupOldLogFiles();
        }
        catch
        {
            // Silently fail if we can't initialize the log file
        }
    }

    private void WriteToFile(string logEntry)
    {
        lock (_writeLock)
        {
            try
            {
                if (_currentWriter is null)
                {
                    return;
                }

                var entryBytes = Encoding.UTF8.GetByteCount(logEntry) + Environment.NewLine.Length;

                // Check if we need to rotate
                if (_currentFileSize + entryBytes > MaxFileSizeBytes)
                {
                    RotateLogFile();
                }

                _currentWriter?.WriteLine(logEntry);
                _currentFileSize += entryBytes;
            }
            catch
            {
                // Silently fail if we can't write
            }
        }
    }

    private void RotateLogFile()
    {
        try
        {
            _currentWriter?.Flush();
            _currentWriter?.Dispose();
            _currentWriter = null;

            // Generate new filename with sequence number
            var baseName = $"{LogFilePrefix}{DateTime.Now:yyyyMMdd}";
            var sequence = 1;
            string newFilePath;

            do
            {
                var fileName = sequence == 1
                    ? $"{baseName}{LogFileExtension}"
                    : $"{baseName}_{sequence}{LogFileExtension}";
                newFilePath = Path.Combine(_logDirectory, fileName);
                sequence++;
            }
            while (File.Exists(newFilePath) && new FileInfo(newFilePath).Length >= MaxFileSizeBytes);

            _currentLogFilePath = newFilePath;
            _currentFileSize = File.Exists(_currentLogFilePath) ? new FileInfo(_currentLogFilePath).Length : 0;

            _currentWriter = new StreamWriter(_currentLogFilePath, append: true, Encoding.UTF8)
            {
                AutoFlush = true
            };

            CleanupOldLogFiles();
        }
        catch
        {
            // Silently fail if rotation fails
        }
    }

    private void CleanupOldLogFiles()
    {
        try
        {
            var logFiles = Directory.GetFiles(_logDirectory, $"{LogFilePrefix}*{LogFileExtension}")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .ToList();

            // Keep only the most recent files
            foreach (var file in logFiles.Skip(MaxRetainedFiles))
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    // Skip files that can't be deleted
                }
            }
        }
        catch
        {
            // Silently fail if cleanup fails
        }
    }

    private void WriteToEtw(LogLevel level, string category, string message, Exception? exception)
    {
        try
        {
            var exceptionInfo = exception is not null
                ? $" | Exception: {exception.GetType().Name}: {exception.Message}"
                : "";

            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _eventSource.Debug(category, message + exceptionInfo);
                    break;
                case LogLevel.Information:
                    _eventSource.Info(category, message + exceptionInfo);
                    break;
                case LogLevel.Warning:
                    _eventSource.Warning(category, message + exceptionInfo);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    _eventSource.Error(category, message + exceptionInfo);
                    break;
            }
        }
        catch
        {
            // ETW failures should not affect logging
        }
    }

    private static string FormatLogEntry(string timestamp, string level, string category, string message, Exception? exception)
    {
        var sb = new StringBuilder();
        sb.Append($"[{timestamp}] [{level}] [{category}] {message}");

        if (exception is not null)
        {
            sb.AppendLine();
            sb.Append($"  Exception: {exception.GetType().FullName}: {exception.Message}");
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                sb.AppendLine();
                sb.Append($"  StackTrace: {exception.StackTrace}");
            }
        }

        return sb.ToString();
    }

    private static string GetLogLevelString(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Information => "INFO ",
            LogLevel.Warning => "WARN ",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRIT ",
            _ => "UNKN "
        };
    }
}

/// <summary>
/// ETW Event Source for DefaultApp.
/// </summary>
[EventSource(Name = "DefaultApp")]
internal sealed class DefaultAppEventSource : EventSource
{
    public static readonly DefaultAppEventSource Log = new();

    [Event(1, Level = EventLevel.Informational, Message = "Service started: {0}")]
    public void ServiceStarted(string serviceName)
    {
        WriteEvent(1, serviceName);
    }

    [Event(2, Level = EventLevel.Informational, Message = "Service stopped: {0}")]
    public void ServiceStopped(string serviceName)
    {
        WriteEvent(2, serviceName);
    }

    [Event(3, Level = EventLevel.Informational, Message = "Service state changed: {0} from {1} to {2}")]
    public void ServiceStateChanged(string serviceName, string fromState, string toState)
    {
        WriteEvent(3, serviceName, fromState, toState);
    }

    [Event(4, Level = EventLevel.Verbose, Message = "[{0}] {1}")]
    public void Debug(string category, string message)
    {
        WriteEvent(4, category, message);
    }

    [Event(5, Level = EventLevel.Informational, Message = "[{0}] {1}")]
    public void Info(string category, string message)
    {
        WriteEvent(5, category, message);
    }

    [Event(6, Level = EventLevel.Warning, Message = "[{0}] {1}")]
    public void Warning(string category, string message)
    {
        WriteEvent(6, category, message);
    }

    [Event(7, Level = EventLevel.Error, Message = "[{0}] {1}")]
    public void Error(string category, string message)
    {
        WriteEvent(7, category, message);
    }

    [Event(8, Level = EventLevel.Critical, Message = "Unhandled exception: {0}: {1}")]
    public void UnhandledException(string exceptionType, string message, string stackTrace)
    {
        WriteEvent(8, exceptionType, message, stackTrace);
    }
}
