## ADDED Requirements

### Requirement: Safe Async Execution

Fire-and-forget async operations MUST be wrapped in exception handling to prevent unobserved task exceptions.

#### Scenario: Ping operation throws exception
- **WHEN** a ping operation encounters a network error or exception
- **THEN** the exception SHALL be logged via the logging service
- **AND** the UI SHALL display an "Error" state on the ping button
- **AND** the application SHALL NOT crash

#### Scenario: Multiple ping operations
- **WHEN** multiple ping operations are executed concurrently
- **THEN** each operation SHALL handle its own exceptions independently
- **AND** a failure in one operation SHALL NOT affect others

### Requirement: ViewModel Disposal

ViewModels that implement `IDisposable` MUST be disposed when their associated views are unloaded.

#### Scenario: MainPage unloaded
- **WHEN** the MainPage is unloaded from the visual tree
- **THEN** the MainViewModel SHALL be disposed
- **AND** the MediaPlayer resource SHALL be released

#### Scenario: Application shutdown
- **WHEN** the application is closed
- **THEN** all disposable ViewModels SHALL be disposed before the process exits

### Requirement: Window Subclass Cleanup

Window subclassing MUST be properly cleaned up when the window is closed.

#### Scenario: MainWindow closed
- **WHEN** the MainWindow is closed
- **THEN** the original window procedure SHALL be restored via SetWindowLongPtr
- **AND** the delegate reference SHALL be cleared to allow garbage collection

#### Scenario: Window close during operation
- **WHEN** the window is closed while ping operations are in progress
- **THEN** pending async operations SHALL complete gracefully
- **AND** no access violations SHALL occur from stale window handles
