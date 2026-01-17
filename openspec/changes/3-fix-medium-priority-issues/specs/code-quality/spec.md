## ADDED Requirements

### Requirement: Reasonable Splash Screen Duration

The splash screen MUST display for a reasonable duration that balances branding with user responsiveness.

#### Scenario: Application startup
- **WHEN** the application is launched
- **THEN** the splash screen SHALL be displayed for no more than 2 seconds
- **AND** the main window SHALL appear promptly after the splash

#### Scenario: Fast system startup
- **WHEN** system data loads before the splash timeout
- **THEN** the splash screen MAY close early to improve perceived performance

### Requirement: No Dead Code

Unused methods and unreachable code paths MUST be removed from the codebase.

#### Scenario: Code review for unused methods
- **WHEN** a method is not called anywhere in the codebase
- **THEN** the method SHALL be removed unless it is part of a public API

#### Scenario: Always-false conditions
- **WHEN** a code path is never executed (e.g., method always returns 0)
- **THEN** the dead code path SHALL be removed or the implementation fixed

### Requirement: Production-Ready Logging

Debug output statements MUST be replaced with proper structured logging.

#### Scenario: Debug information needed
- **WHEN** diagnostic information is useful during development
- **THEN** ILogger.LogDebug() SHALL be used instead of Debug.WriteLine()
- **AND** the output SHALL only appear when log level is set to Debug

#### Scenario: Release build
- **WHEN** the application is built in Release configuration
- **THEN** no Debug.WriteLine() calls SHALL affect performance
- **AND** logging SHALL respect the configured minimum log level

### Requirement: Named Constants for UI Values

Magic numbers in UI code MUST be replaced with named constants.

#### Scenario: Ping success color
- **WHEN** all 5 pings succeed
- **THEN** the button SHALL use a constant named `PingSuccessColor` or similar
- **AND** the color value SHALL be defined in one location

#### Scenario: Ping failure color
- **WHEN** all 5 pings fail
- **THEN** the button SHALL use a constant named `PingFailureColor` or similar

#### Scenario: Ping partial success color
- **WHEN** 1-4 pings succeed
- **THEN** the button SHALL use a constant named `PingPartialColor` or similar
