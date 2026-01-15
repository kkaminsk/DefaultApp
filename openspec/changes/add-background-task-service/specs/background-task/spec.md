## ADDED Requirements

### Requirement: Timer Background Task

The application SHALL implement a background task that provides current time updates every second.

#### Scenario: Timer updates every second
- **WHEN** the Background Task service is running
- **THEN** the current time is updated every second
- **AND** the time is displayed in the service card

#### Scenario: Timer uses DispatcherTimer
- **WHEN** the Background Task is active
- **THEN** a DispatcherTimer with 1-second interval is used
- **AND** updates are delivered to the UI thread

### Requirement: In-Process Background Task

The background task SHALL run in-process using ApplicationTrigger for on-demand activation.

#### Scenario: Task uses ApplicationTrigger
- **WHEN** starting the Background Task service
- **THEN** an ApplicationTrigger is used to activate the task
- **AND** the task runs in the same process as the main app

#### Scenario: Task re-registers on launch
- **WHEN** the application launches
- **THEN** the background task is registered
- **AND** any existing registration is replaced

### Requirement: Service Lifecycle Management

The ServiceController SHALL manage service states and transitions.

#### Scenario: Service states are tracked
- **WHEN** a service is managed
- **THEN** status is one of: Starting, Running, Stopping, Stopped, Error

#### Scenario: Uptime is tracked
- **WHEN** a service is Running
- **THEN** uptime is tracked and displayed in HH:MM:SS format
- **AND** uptime shows "N/A" when service is Stopped

#### Scenario: Service auto-starts on launch
- **WHEN** the application launches
- **THEN** the Background Task service starts automatically
- **AND** it is the default service type

### Requirement: Status Indicator Colors

The service status SHALL be displayed with fixed color indicators.

#### Scenario: Starting shows yellow/orange
- **WHEN** service status is Starting
- **THEN** status indicator is Yellow/Orange

#### Scenario: Running shows green
- **WHEN** service status is Running
- **THEN** status indicator is Green

#### Scenario: Stopping shows yellow/orange
- **WHEN** service status is Stopping
- **THEN** status indicator is Yellow/Orange

#### Scenario: Stopped shows gray
- **WHEN** service status is Stopped
- **THEN** status indicator is Gray

#### Scenario: Error shows red
- **WHEN** service status is Error
- **THEN** status indicator is Red

### Requirement: Service Switching

The system SHALL handle switching between service types gracefully.

#### Scenario: Current service stops before switching
- **WHEN** user selects a different service type
- **AND** a service is currently running
- **THEN** the current service stops first
- **AND** the new service starts after

### Requirement: Service Cleanup

Services SHALL stop when the application closes.

#### Scenario: Service stops on app close
- **WHEN** the application is closed
- **THEN** all running services are stopped
- **AND** no background processes persist
