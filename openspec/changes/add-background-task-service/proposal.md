## Why

The application needs to demonstrate MSIX Background Task capabilities with a timer service that updates the UI every second. This is the default service type on app startup.

## What Changes

- Implement TimerBackgroundTask in DefaultApp.BackgroundTasks project
- Create in-process background task using ApplicationTrigger
- Implement DispatcherTimer for 1-second updates
- Add ServiceController for service lifecycle management
- Update Package.appxmanifest with background task extension
- Register background task on every app launch

## Impact

- Affected specs: `background-task` (new capability)
- Affected code: DefaultApp.BackgroundTasks/, Services/ServiceController.cs
- Dependencies: Requires `add-project-foundation` and `add-main-ui-layout`
- Related: First of three service implementations
