## ADDED Requirements

### Requirement: Full Trust Process Launcher

The application SHALL launch a Win32 Full Trust Process for event logging functionality.

#### Scenario: Process launches via FullTrustProcessLauncher
- **WHEN** the Full Trust Process service is started
- **THEN** the process is launched using FullTrustProcessLauncher API
- **AND** the process runs as a separate Win32 executable

#### Scenario: Process terminates with app
- **WHEN** the main application closes
- **THEN** the Full Trust Process is terminated
- **AND** no orphan processes remain

### Requirement: Named Pipe Communication

The Full Trust Process SHALL communicate with the main app via Named Pipes.

#### Scenario: Pipe uses instance-specific naming
- **WHEN** the Named Pipe is created
- **THEN** name follows pattern: \\.\pipe\DefaultApp_{ProcessId}

#### Scenario: Messages use JSON format
- **WHEN** sending or receiving messages
- **THEN** JSON format is used for human-readable debugging

#### Scenario: Auto-reconnect on connection loss
- **WHEN** the Named Pipe connection is lost
- **THEN** the client attempts automatic reconnection
- **AND** up to 5 retry attempts are made
- **AND** retries occur at 5-second intervals

### Requirement: Windows Event Log Writing

The Full Trust Process SHALL write events to the Windows Event Log.

#### Scenario: Events are logged to Application log
- **WHEN** a log event request is received
- **THEN** the event is written to Windows Event Log
- **AND** a custom event source is used

#### Scenario: Default log level filters events
- **WHEN** logging is configured
- **THEN** only Errors and Warnings are logged by default
- **AND** log level is configurable

### Requirement: Service Card Display

The UI SHALL display Full Trust Process status when selected.

#### Scenario: Log entries count is displayed
- **WHEN** Full Trust Process is the active service type
- **THEN** the service card shows count of log entries written

#### Scenario: Connection status is displayed
- **WHEN** Full Trust Process is the active service type
- **THEN** the service card shows Named Pipe connection status

### Requirement: Error Recovery

The system SHALL handle Full Trust Process failures gracefully.

#### Scenario: Launch failure shows error
- **WHEN** the Full Trust Process fails to launch
- **THEN** the service status shows Error
- **AND** an error dialog prompts user to restart app

#### Scenario: Connection failure triggers reconnect
- **WHEN** the Named Pipe connection fails
- **THEN** automatic reconnection is attempted
- **AND** status shows error after all retries exhausted
