# Proposal: Remove Packaged Services

## Summary

Remove all packaged service features from the application, including:
- Background Task service
- App Service
- Full Trust Process

The application will be simplified to a system information viewer without the service demonstration capabilities.

## Motivation

The packaged service features add complexity and MSIX package dependencies that may not be needed for a simple system information utility. Removing these features will:
- Simplify the codebase and reduce maintenance burden
- Remove the dependency on MSIX packaging for service features
- Eliminate the need for the `runFullTrust` capability
- Reduce the UI complexity by removing the third info card

## Scope

### Projects to Remove
- `DefaultApp.BackgroundTasks/` - Entire project
- `DefaultApp.AppService/` - Entire project
- `DefaultApp.FullTrustProcess/` - Entire project

### Files to Modify
- `DefaultApp.sln` - Remove service project references
- `DefaultApp/DefaultApp.csproj` - Remove project references
- `DefaultApp/Package.appxmanifest` - Remove fullTrustProcess extension
- `DefaultApp/Views/MainPage.xaml` - Remove Packaged Service card
- `DefaultApp/Views/MainPage.xaml.cs` - Remove service-related code
- `DefaultApp/ViewModels/MainViewModel.cs` - Remove service properties/commands
- `DefaultApp/Strings/en-US/Resources.resw` - Remove service strings
- `DefaultApp/Views/DebugPage.xaml` - Remove service testing buttons
- `DefaultApp/Views/DebugPage.xaml.cs` - Remove service test handlers

### Files to Delete
- `DefaultApp/Services/ServiceController.cs`
- `DefaultApp/Services/NamedPipeClient.cs`

### Tests to Update
- Remove service-related tests from `DefaultApp.Tests/`

### Documentation to Update
- `openspec/project.md` - Remove service references
- Archive related proposals

## Impact

- **UI**: The main page will have 2 cards instead of 3 (OS Info, Architecture Info)
- **Package**: MSIX package size will decrease significantly
- **Capabilities**: `runFullTrust` capability can be removed (unless still needed for WinUI 3)
- **Tests**: Service-related tests will be removed

## Dependencies

None - this is a removal proposal.
