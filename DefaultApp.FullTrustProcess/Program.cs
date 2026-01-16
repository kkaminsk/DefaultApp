namespace DefaultApp.FullTrustProcess;

/// <summary>
/// Entry point for the Full Trust Process.
/// Implements a Named Pipe server for IPC and Windows Event Log writing.
/// </summary>
internal class Program
{
    private static readonly CancellationTokenSource _shutdownTokenSource = new();
    private static NamedPipeServer? _pipeServer;
    private static EventLoggerService? _eventLogger;

    static async Task Main(string[] args)
    {
        Console.WriteLine("DefaultApp Full Trust Process starting...");
        Console.WriteLine($"Process ID: {Environment.ProcessId}");
        Console.WriteLine($"Arguments: {string.Join(" ", args)}");

        // Parse command-line arguments for pipe name override
        var pipeName = GetPipeName(args);
        Console.WriteLine($"Using pipe name: {pipeName}");

        // Set up graceful shutdown handling
        Console.CancelKeyPress += OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

        try
        {
            // Initialize services
            _eventLogger = new EventLoggerService();
            _pipeServer = new NamedPipeServer(pipeName, _eventLogger);

            _pipeServer.ClientConnected += (s, e) =>
                Console.WriteLine("Client connected to Named Pipe.");

            _pipeServer.ClientDisconnected += (s, e) =>
                Console.WriteLine("Client disconnected from Named Pipe.");

            // Log startup event
            _eventLogger.WriteEvent(
                $"DefaultApp Full Trust Process started (PID: {Environment.ProcessId})",
                "Information",
                1001);

            // Run the pipe server until shutdown
            await _pipeServer.RunAsync(_shutdownTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Shutdown requested.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            _eventLogger?.WriteEvent($"Full Trust Process error: {ex.Message}", "Error", 1002);
        }
        finally
        {
            Cleanup();
        }

        Console.WriteLine("DefaultApp Full Trust Process exiting.");
    }

    /// <summary>
    /// Gets the pipe name from command-line arguments or generates a default.
    /// </summary>
    private static string GetPipeName(string[] args)
    {
        // Check for --pipe argument
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == "--pipe" || args[i] == "-p")
            {
                return args[i + 1];
            }
        }

        // Default: use process ID for instance-specific naming
        return $"DefaultApp_{Environment.ProcessId}";
    }

    private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        Console.WriteLine("Cancel key pressed, initiating shutdown...");
        e.Cancel = true;
        _shutdownTokenSource.Cancel();
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("Process exit event received.");
        Cleanup();
    }

    private static void Cleanup()
    {
        Console.WriteLine("Cleaning up resources...");

        _pipeServer?.Stop();
        _pipeServer?.Dispose();
        _eventLogger?.Dispose();
        _shutdownTokenSource.Dispose();

        Console.WriteLine("Cleanup complete.");
    }
}
