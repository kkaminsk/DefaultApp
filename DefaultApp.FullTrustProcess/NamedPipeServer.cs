using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace DefaultApp.FullTrustProcess;

/// <summary>
/// Named Pipe server for bidirectional IPC with the main application.
/// Uses instance-specific naming: \\.\pipe\DefaultApp_{ProcessId}
/// </summary>
public sealed class NamedPipeServer : IDisposable
{
    private readonly string _pipeName;
    private readonly EventLoggerService _eventLogger;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private NamedPipeServerStream? _pipeServer;
    private bool _isDisposed;
    private bool _isRunning;

    public NamedPipeServer(string pipeName, EventLoggerService eventLogger)
    {
        _pipeName = pipeName;
        _eventLogger = eventLogger;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// Gets the pipe name being used.
    /// </summary>
    public string PipeName => _pipeName;

    /// <summary>
    /// Gets whether the server is currently running.
    /// </summary>
    public bool IsRunning => _isRunning;

    /// <summary>
    /// Event raised when a client connects.
    /// </summary>
    public event EventHandler? ClientConnected;

    /// <summary>
    /// Event raised when a client disconnects.
    /// </summary>
    public event EventHandler? ClientDisconnected;

    /// <summary>
    /// Starts the Named Pipe server and listens for connections.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _isRunning = true;
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken, _cancellationTokenSource.Token).Token;

        Console.WriteLine($"Named Pipe Server starting on: {_pipeName}");

        while (!linkedToken.IsCancellationRequested)
        {
            try
            {
                // Create a new server instance for each connection
                _pipeServer = new NamedPipeServerStream(
                    _pipeName,
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

                Console.WriteLine("Waiting for client connection...");

                await _pipeServer.WaitForConnectionAsync(linkedToken);

                Console.WriteLine("Client connected.");
                ClientConnected?.Invoke(this, EventArgs.Empty);

                // Handle client communication
                await HandleClientAsync(_pipeServer, linkedToken);
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
                await Task.Delay(1000, linkedToken);
            }
            finally
            {
                if (_pipeServer is not null)
                {
                    if (_pipeServer.IsConnected)
                    {
                        _pipeServer.Disconnect();
                    }
                    await _pipeServer.DisposeAsync();
                    _pipeServer = null;

                    ClientDisconnected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        _isRunning = false;
        Console.WriteLine("Named Pipe Server stopped.");
    }

    private async Task HandleClientAsync(NamedPipeServerStream pipeServer, CancellationToken cancellationToken)
    {
        var buffer = new byte[4096];

        while (pipeServer.IsConnected && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Read message length prefix (4 bytes)
                var lengthBuffer = new byte[4];
                var bytesRead = await pipeServer.ReadAsync(lengthBuffer, cancellationToken);

                if (bytesRead == 0)
                {
                    // Client disconnected
                    break;
                }

                if (bytesRead < 4)
                {
                    continue;
                }

                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                if (messageLength <= 0 || messageLength > 65536)
                {
                    continue;
                }

                // Read the message
                var messageBuffer = new byte[messageLength];
                var totalRead = 0;
                while (totalRead < messageLength)
                {
                    bytesRead = await pipeServer.ReadAsync(
                        messageBuffer.AsMemory(totalRead, messageLength - totalRead),
                        cancellationToken);

                    if (bytesRead == 0) break;
                    totalRead += bytesRead;
                }

                if (totalRead < messageLength)
                {
                    continue;
                }

                var messageJson = Encoding.UTF8.GetString(messageBuffer);
                Console.WriteLine($"Received: {messageJson}");

                // Process the message and get response
                var response = ProcessMessage(messageJson);

                // Send response with length prefix
                var responseBytes = Encoding.UTF8.GetBytes(response);
                var responseLength = BitConverter.GetBytes(responseBytes.Length);

                await pipeServer.WriteAsync(responseLength, cancellationToken);
                await pipeServer.WriteAsync(responseBytes, cancellationToken);
                await pipeServer.FlushAsync(cancellationToken);

                Console.WriteLine($"Sent: {response}");
            }
            catch (IOException)
            {
                // Client disconnected
                break;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling message: {ex.Message}");
            }
        }
    }

    private string ProcessMessage(string messageJson)
    {
        try
        {
            using var doc = JsonDocument.Parse(messageJson);
            var root = doc.RootElement;

            if (!root.TryGetProperty("Command", out var commandElement))
            {
                return CreateErrorResponse("No command specified");
            }

            var command = commandElement.GetString();

            switch (command)
            {
                case "LogEvent":
                    return HandleLogEvent(root);

                case "GetStatus":
                    return HandleGetStatus();

                case "Shutdown":
                    return HandleShutdown();

                default:
                    return CreateErrorResponse($"Unknown command: {command}");
            }
        }
        catch (JsonException ex)
        {
            return CreateErrorResponse($"Invalid JSON: {ex.Message}");
        }
    }

    private string HandleLogEvent(JsonElement root)
    {
        try
        {
            var level = root.TryGetProperty("Level", out var levelProp)
                ? levelProp.GetString() ?? "Information"
                : "Information";

            var message = root.TryGetProperty("Message", out var msgProp)
                ? msgProp.GetString() ?? ""
                : "";

            var eventId = root.TryGetProperty("EventId", out var idProp)
                ? idProp.GetInt32()
                : 1000;

            _eventLogger.WriteEvent(message, level, eventId);

            return JsonSerializer.Serialize(new
            {
                Status = "OK",
                Message = "Event logged successfully",
                TotalEventsLogged = _eventLogger.EventsWritten
            });
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Failed to log event: {ex.Message}");
        }
    }

    private string HandleGetStatus()
    {
        return JsonSerializer.Serialize(new
        {
            Status = "OK",
            ProcessId = Environment.ProcessId,
            PipeName = _pipeName,
            EventsLogged = _eventLogger.EventsWritten,
            LogLevel = _eventLogger.MinimumLevel,
            Uptime = (DateTime.Now - _eventLogger.StartTime).TotalSeconds
        });
    }

    private string HandleShutdown()
    {
        // Signal shutdown
        _cancellationTokenSource.Cancel();

        return JsonSerializer.Serialize(new
        {
            Status = "OK",
            Message = "Shutdown initiated"
        });
    }

    private static string CreateErrorResponse(string message)
    {
        return JsonSerializer.Serialize(new
        {
            Status = "Error",
            Message = message
        });
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        _cancellationTokenSource.Cancel();
        _pipeServer?.Dispose();
        _cancellationTokenSource.Dispose();
    }
}
