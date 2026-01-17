## Why

The application needs to demonstrate MSIX Full Trust Process capabilities with a Win32 background service that communicates via Named Pipes and logs events to the Windows Event Log.

## What Changes

- Implement FullTrustProcess console application
- Create NamedPipeServer for IPC communication
- Implement EventLoggerService for Windows Event Log writing
- Add Named Pipe client in main app
- Update Package.appxmanifest with fullTrustProcess extension
- Implement auto-reconnect with 5 retries at 5-second intervals

## Impact

- Affected specs: `full-trust-process` (new capability)
- Affected code: DefaultApp.FullTrustProcess/, Services/ServiceController.cs
- Dependencies: Requires `add-project-foundation`, `add-main-ui-layout`, `add-background-task-service`
- Related: Third of three service implementations
