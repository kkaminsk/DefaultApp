## 1. Full Trust Process Application

- [x] 1.1 Create Program.cs entry point
- [x] 1.2 Parse command-line arguments for pipe name
- [x] 1.3 Initialize NamedPipeServer
- [x] 1.4 Initialize EventLoggerService
- [x] 1.5 Handle graceful shutdown signals

## 2. Named Pipe Server

- [x] 2.1 Create NamedPipeServer class
- [x] 2.2 Use instance-specific naming: \\.\pipe\DefaultApp_{ProcessId}
- [x] 2.3 Implement JSON message protocol
- [x] 2.4 Handle client connections
- [x] 2.5 Process incoming messages
- [x] 2.6 Send responses back to client

## 3. Event Logger Service

- [x] 3.1 Create EventLoggerService class
- [x] 3.2 Register custom event source on first run
- [x] 3.3 Implement WriteEvent() method
- [x] 3.4 Default to Errors and Warnings only
- [x] 3.5 Make log level configurable
- [x] 3.6 Track count of entries written

## 4. Named Pipe Client (Main App)

- [x] 4.1 Create NamedPipeClient class in main app
- [x] 4.2 Implement ConnectAsync() with retry logic
- [x] 4.3 Auto-reconnect with 5 retries at 5-second intervals
- [x] 4.4 Send JSON messages to Full Trust Process
- [x] 4.5 Receive and parse responses

## 5. Service Controller Extension

- [x] 5.1 Add StartFullTrustProcessAsync() method
- [x] 5.2 Add StopFullTrustProcess() method
- [x] 5.3 Launch process using FullTrustProcessLauncher
- [x] 5.4 Track process lifetime
- [x] 5.5 Handle process exit events

## 6. Package Manifest

- [x] 6.1 Add windows.fullTrustProcess extension to manifest
- [x] 6.2 Set Executable to DefaultApp.FullTrustProcess.exe
- [x] 6.3 Verify runFullTrust capability is declared

## 7. JSON Protocol

- [x] 7.1 Define message types (LogEvent, GetStatus, Shutdown)
- [x] 7.2 Define request/response JSON schemas
- [x] 7.3 Implement serialization/deserialization
- [x] 7.4 Handle malformed messages gracefully

## 8. UI Integration

- [x] 8.1 Display log entries written count in service card
- [x] 8.2 Display connection status
- [x] 8.3 Provide test button to log sample events
- [x] 8.4 Update status on connection state changes

## 9. Error Handling

- [x] 9.1 Handle process launch failures
- [x] 9.2 Handle pipe connection failures
- [x] 9.3 Handle process unexpected termination
- [x] 9.4 Implement reconnection logic
- [x] 9.5 Display appropriate error states

## 10. Validation

- [x] 10.1 Verify process launches correctly
- [x] 10.2 Verify Named Pipe communication works
- [x] 10.3 Verify events are written to Windows Event Log
- [x] 10.4 Verify auto-reconnect after connection loss
- [x] 10.5 Verify process terminates when app closes
