## ADDED Requirements

### Requirement: Exception Logging in Services

All service-layer catch blocks MUST log exceptions to aid in debugging and troubleshooting.

#### Scenario: Registry access fails
- **WHEN** a registry read operation throws an exception
- **THEN** the exception details SHALL be logged at Warning or Error level
- **AND** the service SHALL return a graceful fallback value (e.g., "Unavailable")

#### Scenario: P/Invoke call fails
- **WHEN** a native method call throws an exception
- **THEN** the exception details SHALL be logged with method context
- **AND** the failure SHALL NOT crash the application

#### Scenario: Logging service internal error
- **WHEN** the LoggingService itself encounters an error
- **THEN** the error SHALL be written to Debug output
- **AND** the service SHALL continue operating in degraded mode

### Requirement: ThemeService Resource Management

The ThemeService MUST implement IDisposable to properly clean up event subscriptions.

#### Scenario: ThemeService disposed
- **WHEN** the ThemeService is disposed
- **THEN** the ColorValuesChanged event subscription SHALL be removed
- **AND** subsequent theme operations SHALL be no-ops

#### Scenario: System theme change after disposal
- **WHEN** the system theme changes after ThemeService is disposed
- **THEN** no callback SHALL be invoked
- **AND** no exceptions SHALL be thrown
