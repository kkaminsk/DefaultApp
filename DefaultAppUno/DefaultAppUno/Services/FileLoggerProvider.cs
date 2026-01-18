using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DefaultApp.Services;

/// <summary>
/// Logger provider that creates FileLogger instances for file-based logging.
/// </summary>
public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();
    private readonly LoggingService _loggingService;

    public FileLoggerProvider(LoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _loggingService));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}

/// <summary>
/// Logger implementation that writes to the LoggingService.
/// </summary>
public sealed class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly LoggingService _loggingService;

    public FileLogger(string categoryName, LoggingService loggingService)
    {
        _categoryName = categoryName;
        _loggingService = loggingService;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _loggingService.MinimumLogLevel && logLevel != LogLevel.None;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        _loggingService.WriteLog(logLevel, _categoryName, message, exception);
    }
}
