## 1. Full Trust Process Application

- [ ] 1.1 Create Program.cs entry point
- [ ] 1.2 Parse command-line arguments for pipe name
- [ ] 1.3 Initialize NamedPipeServer
- [ ] 1.4 Initialize EventLoggerService
- [ ] 1.5 Handle graceful shutdown signals

## 2. Named Pipe Server

- [ ] 2.1 Create NamedPipeServer class
- [ ] 2.2 Use instance-specific naming: \\.\pipe\DefaultApp_{ProcessId}
- [ ] 2.3 Implement JSON message protocol
- [ ] 2.4 Handle client connections
- [ ] 2.5 Process incoming messages
- [ ] 2.6 Send responses back to client

## 3. Event Logger Service

- [ ] 3.1 Create EventLoggerService class
- [ ] 3.2 Register custom event source on first run
- [ ] 3.3 Implement WriteEvent() method
- [ ] 3.4 Default to Errors and Warnings only
- [ ] 3.5 Make log level configurable
- [ ] 3.6 Track count of entries written

## 4. Named Pipe Client (Main App)

- [ ] 4.1 Create NamedPipeClient class in main app
- [ ] 4.2 Implement ConnectAsync() with retry logic
- [ ] 4.3 Auto-reconnect with 5 retries at 5-second intervals
- [ ] 4.4 Send JSON messages to Full Trust Process
- [ ] 4.5 Receive and parse responses

## 5. Service Controller Extension

- [ ] 5.1 Add StartFullTrustProcessAsync() method
- [ ] 5.2 Add StopFullTrustProcess() method
- [ ] 5.3 Launch process using FullTrustProcessLauncher
- [ ] 5.4 Track process lifetime
- [ ] 5.5 Handle process exit events

## 6. Package Manifest

- [ ] 6.1 Add windows.fullTrustProcess extension to manifest
- [ ] 6.2 Set Executable to DefaultApp.FullTrustProcess.exe
- [ ] 6.3 Verify runFullTrust capability is declared

## 7. JSON Protocol

- [ ] 7.1 Define message types (LogEvent, GetStatus, Shutdown)
- [ ] 7.2 Define request/response JSON schemas
- [ ] 7.3 Implement serialization/deserialization
- [ ] 7.4 Handle malformed messages gracefully

## 8. UI Integration

- [ ] 8.1 Display log entries written count in service card
- [ ] 8.2 Display connection status
- [ ] 8.3 Provide test button to log sample events
- [ ] 8.4 Update status on connection state changes

## 9. Error Handling

- [ ] 9.1 Handle process launch failures
- [ ] 9.2 Handle pipe connection failures
- [ ] 9.3 Handle process unexpected termination
- [ ] 9.4 Implement reconnection logic
- [ ] 9.5 Display appropriate error states

## 10. Validation

- [ ] 10.1 Verify process launches correctly
- [ ] 10.2 Verify Named Pipe communication works
- [ ] 10.3 Verify events are written to Windows Event Log
- [ ] 10.4 Verify auto-reconnect after connection loss
- [ ] 10.5 Verify process terminates when app closes
