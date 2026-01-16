using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DefaultApp.Services;

/// <summary>
/// Named Pipe client for communicating with the Full Trust Process.
/// Implements auto-reconnect with configurable retry logic.
/// </summary>
public sealed class NamedPipeClient : IDisposable
{
    private const int MaxRetries = 5;
    private const int RetryDelayMs = 5000;
    private const int ConnectionTimeoutMs = 10000;

    private readonly ILogger<NamedPipeClient>? _logger;
    private NamedPipeClientStream? _pipeClient;
    private string _pipeName = string.Empty;
    private bool _isDisposed;
    private bool _isConnected;

    public NamedPipeClient()
    {
        _logger = App.LoggerFactory?.CreateLogger<NamedPipeClient>();
    }

    /// <summary>
    /// Gets whether the client is currently connected.
    /// </summary>
    public bool IsConnected => _isConnected && (_pipeClient?.IsConnected ?? false);

    /// <summary>
    /// Gets the current pipe name.
    /// </summary>
    public string PipeName => _pipeName;

    /// <summary>
    /// Event raised when connection state changes.
    /// </summary>
    public event EventHandler<bool>? ConnectionStateChanged;

    /// <summary>
    /// Connects to the Named Pipe server with retry logic.
    /// </summary>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if connected successfully.</returns>
    public async Task<bool> ConnectAsync(string pipeName, CancellationToken cancellationToken = default)
    {
        _pipeName = pipeName;
        _logger?.LogInformation("Attempting to connect to pipe: {PipeName}", pipeName);

        for (int retry = 0; retry < MaxRetries; retry++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            try
            {
                // Dispose existing connection if any
                _pipeClient?.Dispose();

                _pipeClient = new NamedPipeClientStream(
                    ".",
                    pipeName,
                    PipeDirection.InOut,
                    PipeOptions.Asynchronous);

                using var timeoutCts = new CancellationTokenSource(ConnectionTimeoutMs);
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken, timeoutCts.Token);

                await _pipeClient.ConnectAsync(linkedCts.Token);

                _isConnected = true;
                _logger?.LogInformation("Connected to pipe: {PipeName}", pipeName);
                ConnectionStateChanged?.Invoke(this, true);

                return true;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogDebug("Connection cancelled");
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(
                    "Connection attempt {Attempt}/{MaxRetries} failed: {Error}",
                    retry + 1, MaxRetries, ex.Message);

                if (retry < MaxRetries - 1)
                {
                    _logger?.LogDebug("Retrying in {Delay}ms...", RetryDelayMs);
                    try
                    {
                        await Task.Delay(RetryDelayMs, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return false;
                    }
                }
            }
        }

        _isConnected = false;
        _logger?.LogError("Failed to connect after {MaxRetries} attempts", MaxRetries);
        ConnectionStateChanged?.Invoke(this, false);

        return false;
    }

    /// <summary>
    /// Sends a JSON message and receives a response.
    /// </summary>
    /// <param name="message">The message object to send (will be serialized to JSON).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response as a JsonDocument, or null if failed.</returns>
    public async Task<JsonDocument?> SendMessageAsync(object message, CancellationToken cancellationToken = default)
    {
        if (!IsConnected || _pipeClient is null)
        {
            _logger?.LogWarning("Cannot send message: not connected");
            return null;
        }

        try
        {
            var json = JsonSerializer.Serialize(message);
            _logger?.LogDebug("Sending: {Message}", json);

            var messageBytes = Encoding.UTF8.GetBytes(json);
            var lengthBytes = BitConverter.GetBytes(messageBytes.Length);

            // Send length prefix + message
            await _pipeClient.WriteAsync(lengthBytes, cancellationToken);
            await _pipeClient.WriteAsync(messageBytes, cancellationToken);
            await _pipeClient.FlushAsync(cancellationToken);

            // Read response length
            var responseLengthBytes = new byte[4];
            var bytesRead = await _pipeClient.ReadAsync(responseLengthBytes, cancellationToken);

            if (bytesRead < 4)
            {
                _logger?.LogWarning("Failed to read response length");
                return null;
            }

            var responseLength = BitConverter.ToInt32(responseLengthBytes, 0);
            if (responseLength <= 0 || responseLength > 65536)
            {
                _logger?.LogWarning("Invalid response length: {Length}", responseLength);
                return null;
            }

            // Read response message
            var responseBuffer = new byte[responseLength];
            var totalRead = 0;
            while (totalRead < responseLength)
            {
                bytesRead = await _pipeClient.ReadAsync(
                    responseBuffer.AsMemory(totalRead, responseLength - totalRead),
                    cancellationToken);

                if (bytesRead == 0) break;
                totalRead += bytesRead;
            }

            var responseJson = Encoding.UTF8.GetString(responseBuffer, 0, totalRead);
            _logger?.LogDebug("Received: {Response}", responseJson);

            return JsonDocument.Parse(responseJson);
        }
        catch (IOException ex)
        {
            _logger?.LogError(ex, "IO error during message exchange");
            HandleDisconnection();
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error sending message");
            return null;
        }
    }

    /// <summary>
    /// Sends a log event request to the Full Trust Process.
    /// </summary>
    public async Task<bool> LogEventAsync(string message, string level = "Information", int eventId = 1000)
    {
        var request = new
        {
            Command = "LogEvent",
            Message = message,
            Level = level,
            EventId = eventId
        };

        var response = await SendMessageAsync(request);
        if (response is null) return false;

        using (response)
        {
            return response.RootElement.TryGetProperty("Status", out var status) &&
                   status.GetString() == "OK";
        }
    }

    /// <summary>
    /// Gets the status from the Full Trust Process.
    /// </summary>
    public async Task<(bool Success, int EventsLogged, double UptimeSeconds)> GetStatusAsync()
    {
        var request = new { Command = "GetStatus" };

        var response = await SendMessageAsync(request);
        if (response is null) return (false, 0, 0);

        using (response)
        {
            var root = response.RootElement;

            if (!root.TryGetProperty("Status", out var status) || status.GetString() != "OK")
            {
                return (false, 0, 0);
            }

            var eventsLogged = root.TryGetProperty("EventsLogged", out var events)
                ? events.GetInt32()
                : 0;

            var uptime = root.TryGetProperty("Uptime", out var uptimeProp)
                ? uptimeProp.GetDouble()
                : 0;

            return (true, eventsLogged, uptime);
        }
    }

    /// <summary>
    /// Sends a shutdown request to the Full Trust Process.
    /// </summary>
    public async Task<bool> SendShutdownAsync()
    {
        var request = new { Command = "Shutdown" };
        var response = await SendMessageAsync(request);

        using (response)
        {
            return response is not null;
        }
    }

    /// <summary>
    /// Disconnects from the Named Pipe server.
    /// </summary>
    public void Disconnect()
    {
        if (_pipeClient is not null)
        {
            try
            {
                _pipeClient.Dispose();
            }
            catch { }
            _pipeClient = null;
        }

        _isConnected = false;
        ConnectionStateChanged?.Invoke(this, false);
        _logger?.LogInformation("Disconnected from pipe");
    }

    private void HandleDisconnection()
    {
        _isConnected = false;
        ConnectionStateChanged?.Invoke(this, false);
        _logger?.LogWarning("Connection lost");
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        Disconnect();
    }
}
