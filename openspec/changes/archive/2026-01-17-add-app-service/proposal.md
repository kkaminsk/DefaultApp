## Why

The application needs to demonstrate MSIX App Service capabilities with an event log reader that provides bidirectional communication between the service and UI.

## What Changes

- Implement EventLogAppService in DefaultApp.AppService project
- Create App Service connection handling in main app
- Query Windows Application Event Log for recent Warning/Error events
- Display event summaries in the service card
- Update Package.appxmanifest with app service extension

## Impact

- Affected specs: `app-service` (new capability)
- Affected code: DefaultApp.AppService/, Services/ServiceController.cs
- Dependencies: Requires `add-project-foundation`, `add-main-ui-layout`, `add-background-task-service`
- Related: Second of three service implementations
