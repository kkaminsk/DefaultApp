# app-service Specification

## Purpose
TBD - created by archiving change add-app-service. Update Purpose after archive.
## Requirements
### Requirement: Event Log Reader App Service

The application SHALL implement an App Service that reads Windows Application Event Log entries.

#### Scenario: Service queries Application Event Log
- **WHEN** the App Service is active
- **THEN** it queries the Windows Application Event Log
- **AND** only Warning and Error level events are returned

#### Scenario: Events are filtered by time
- **WHEN** querying event log
- **THEN** only events from the last 24 hours are included

#### Scenario: Event count is limited
- **WHEN** querying event log
- **THEN** a maximum of 10 most recent events are returned

### Requirement: Event Display Fields

The service SHALL return specific fields for each event.

#### Scenario: All required fields are returned
- **WHEN** event data is retrieved
- **THEN** Event ID, Source, Level, Timestamp, and Message are included
- **AND** Message is truncated if too long

### Requirement: Bidirectional Communication

The App Service SHALL support bidirectional communication with the main application.

#### Scenario: Service receives requests
- **WHEN** the main app sends a request via AppServiceConnection
- **THEN** the service processes the request
- **AND** returns event data as a ValueSet response

#### Scenario: Service handles connection lifecycle
- **WHEN** the AppServiceConnection is opened
- **THEN** the service maintains the connection
- **AND** properly handles disconnection

### Requirement: Service Card Display

The UI SHALL display event log information when App Service is selected.

#### Scenario: Event summary is displayed
- **WHEN** App Service is the active service type
- **THEN** the service card shows event log summary/count
- **AND** individual event details may be shown

### Requirement: Error Handling

The service SHALL handle errors gracefully.

#### Scenario: Event log access failure is handled
- **WHEN** event log cannot be accessed
- **THEN** an error response is returned
- **AND** the service continues to function

#### Scenario: Connection failure is handled
- **WHEN** AppServiceConnection fails
- **THEN** the service status shows Error
- **AND** user can retry via refresh

