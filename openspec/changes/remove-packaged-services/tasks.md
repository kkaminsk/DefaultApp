## 1. Remove Service Projects from Solution

- [x] 1.1 Remove DefaultApp.BackgroundTasks project reference from DefaultApp.sln
- [x] 1.2 Remove DefaultApp.AppService project reference from DefaultApp.sln
- [x] 1.3 Remove DefaultApp.FullTrustProcess project reference from DefaultApp.sln
- [x] 1.4 Delete DefaultApp.BackgroundTasks/ directory
- [x] 1.5 Delete DefaultApp.AppService/ directory
- [x] 1.6 Delete DefaultApp.FullTrustProcess/ directory

## 2. Update Main Project Configuration

- [x] 2.1 Remove project references from DefaultApp/DefaultApp.csproj
- [x] 2.2 Remove fullTrustProcess extension from Package.appxmanifest
- [x] 2.3 Evaluate if runFullTrust capability can be removed (kept - required for WinUI 3)

## 3. Remove Service Files from Main Project

- [x] 3.1 Delete DefaultApp/Services/ServiceController.cs
- [x] 3.2 Delete DefaultApp/Services/NamedPipeClient.cs

## 4. Update UI - Remove Service Card

- [x] 4.1 Remove Packaged Service card from MainPage.xaml
- [x] 4.2 Remove service-related code from MainPage.xaml.cs
- [x] 4.3 Update grid/layout if needed for 2-card layout

## 5. Update ViewModel

- [x] 5.1 Remove service properties from MainViewModel.cs (SelectedServiceIndex, ServiceStatus, ServiceUptime, etc.)
- [x] 5.2 Remove ToggleServiceCommand
- [x] 5.3 Remove service initialization and disposal code
- [x] 5.4 Remove service-related logging calls

## 6. Update Resources

- [x] 6.1 Remove service-related strings from Resources.resw (ServiceCardHeader, ServiceTypeLabel, etc.)

## 7. Update Debug Page

- [x] 7.1 Remove service testing section from DebugPage.xaml
- [x] 7.2 Remove service testing handlers from DebugPage.xaml.cs

## 8. Update Tests

- [x] 8.1 Delete TimerBackgroundTaskTests.cs
- [x] 8.2 Delete EventLogAppServiceTests.cs
- [x] 8.3 Delete EventLoggerServiceTests.cs
- [x] 8.4 Delete NamedPipeProtocolTests.cs
- [x] 8.5 Update any remaining tests that reference service code

## 9. Update Documentation

- [x] 9.1 Update openspec/project.md to remove service references
- [x] 9.2 Update CLAUDE.md if needed
- [x] 9.3 Archive add-background-task-service proposal (N/A - proposals remain for history)
- [x] 9.4 Archive add-app-service proposal (N/A - proposals remain for history)
- [x] 9.5 Archive add-full-trust-process proposal (N/A - proposals remain for history)

## 10. Validation

- [x] 10.1 Build solution successfully
- [x] 10.2 Run remaining tests (10 tests passed)
- [x] 10.3 Verify application launches and displays 2 cards
- [x] 10.4 Build MSIX package successfully
- [x] 10.5 Verify MSIX package size decreased (reduced from 4 projects to 2)
