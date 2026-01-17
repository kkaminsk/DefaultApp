# Application Specification: Default App

## Overview

**Application Name:** Default App
**Package Format:** MSIX
**Platform:** Windows 11+
**Language:** C#
**Framework:** .NET 8 / WinUI 3 (Windows App SDK)
**Packaging Model:** Single-Project MSIX
**Version Scheme:** SemVer (1.0.0, 1.0.1, 1.1.0)

## Purpose

Default App is a diagnostic utility that displays comprehensive system information including operating system details, device architecture, and hardware information. It demonstrates three types of MSIX packaged services: Background Task, App Service, and Full Trust Process.

---

## Functional Requirements

### 1. Operating System Information

Display the following OS details:

| Property | Source |
|----------|--------|
| OS Name | `Environment.OSVersion.Platform` |
| OS Version | `Environment.OSVersion.Version` |
| OS Build Number | `Environment.OSVersion.Version.Build` |
| Edition | Registry `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EditionID` |
| Is 64-bit OS | `Environment.Is64BitOperatingSystem` |
| System Locale | `CultureInfo.CurrentCulture` |
| Windows Activation Status | `SLIsGenuineLocal` P/Invoke API |

**Activation Status API:**
- Run on background thread (can take several hundred milliseconds)
- Display "Checking..." initially while querying
- Cache result after first successful call

**Activation Status Display:**
Show full activation states:
- Activated
- Not Activated
- Grace Period
- Notification Mode

### 2. Device Architecture Information

Display the following architecture and hardware details:

| Property | Source |
|----------|--------|
| Processor Architecture | `RuntimeInformation.ProcessArchitecture` |
| OS Architecture | `RuntimeInformation.OSArchitecture` |
| Machine Name | `Environment.MachineName` |
| Processor Count | `Environment.ProcessorCount` |
| CPU Model(s) | Registry `HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor\*` |
| Total RAM | `Windows.System.MemoryManager` (primary), `MEMORYSTATUSEX` P/Invoke (fallback) |
| Is 64-bit Process | `Environment.Is64BitProcess` |
| Running Under Emulation | Compare `ProcessArchitecture` vs `OSArchitecture` |

**Note:** WMI queries have been removed to avoid complexity and potential failures.

**Multi-Processor Display:**
On systems with multiple physical processors, display all processors in a list format.

**RAM Display Format:**
Display RAM in GB with 1 decimal place (e.g., "31.7 GB").

**ARM64 Emulation Detection:**
When `RuntimeInformation.ProcessArchitecture` differs from `RuntimeInformation.OSArchitecture`, display:
- "Running under emulation: Yes" (e.g., x64 app on ARM64 Windows)
- "Running under emulation: No" (native execution)

### 3. Packaged Services

The application implements all three service types in v1.0 to demonstrate full MSIX capabilities.

#### 3.1 Background Task (Timer Service)
- In-process background task using `IBackgroundTask` with `ApplicationTrigger`
- Timer that ticks every second via `DispatcherTimer` in main app
- Updates the UI with the current time
- Demonstrates basic Background Task lifecycle
- Re-registers on every app launch
- Default service type on app startup

#### 3.2 App Service (Event Log Reader)
- Bidirectional communication with UI
- Retrieves application event log summaries
- Updates UI with recent event information

**Event Log Query Configuration:**

| Setting | Value |
|---------|-------|
| Log Source | Application Event Log only |
| Event Count | 10 most recent events |
| Event Levels | Warning and Error only |
| Time Window | Last 24 hours |

**Event Display Fields:**

| Field | Included |
|-------|----------|
| Event ID | Yes |
| Source | Yes |
| Level (Error/Warning) | Yes |
| Timestamp | Yes |
| Message | Yes (truncated) |

#### 3.3 Full Trust Process (Continuous Logger)
- Win32 background service via `windows.fullTrustProcess`
- Communicates with main app via **Named Pipes**
- Logs application-generated events to Windows Event Log

**Named Pipe Configuration:**

| Setting | Value |
|---------|-------|
| Naming Convention | Instance-specific: `\\.\pipe\DefaultApp_{ProcessId}` |
| Protocol Format | JSON (human-readable, easy debugging) |
| Auto Reconnect | Yes |
| Retry Attempts | 5 |
| Retry Interval | 5 seconds |

**Event Log Writing:**

| Setting | Value |
|---------|-------|
| Default Level | Errors and Warnings only |
| Configurable | Yes (log level can be changed) |
| Custom Event Source | To be registered on install |

**Service Card Display:**
- Log entries written count
- Connection status

**Service Switching Behavior:**
When user switches service types while a service is running, automatically stop the current service, then start the new one.

**Service Lifecycle:**

| Behavior | Setting |
|----------|---------|
| Auto-start with application | Yes |
| Persist after app closes | No (services stop when app closes) |
| On start failure | Display error dialog prompting user to restart app |
| Stop mechanism | Button in app UI |

**Status Display:**
- Extended states: Starting / Running / Stopping / Stopped / Error
- Include service uptime/runtime duration when running
- Uptime format: `HH:MM:SS`
- When stopped: Display "N/A" for uptime

**Service-Specific UI Content:**

| Service Type | Content Displayed |
|--------------|-------------------|
| Background Task | Current time from timer |
| App Service | Event log summary/count |
| Full Trust Process | Log entries written, connection status |

**Status Indicator Colors (Fixed):**

| State | Color |
|-------|-------|
| Starting | Yellow/Orange |
| Running | Green |
| Stopping | Yellow/Orange |
| Stopped | Gray |
| Error | Red |

**Button Controls:**
- "Start Service" - Launches the selected service type
- "Stop Service" - Terminates the running service (visible when active)
- Service type selector (dropdown)

---

## Non-Functional Requirements

### Packaging

- **Package Identity:** com.contoso.defaultapp
- **Min OS Version:** Windows 11 (Build 22000)
- **Target OS Version:** Windows 11 (Build 22621+)
- **Distribution:** Microsoft Store and Sideloading
- **Capabilities Required:**
  - `runFullTrust` (required for WinUI 3 and Full Trust Process)
  - Justification documentation required for Store submission

### UI/UX

- Modern WinUI 3 design with Fluent Design System
- Minimum window dimensions: 800x600
- Responsive layout: cards stack vertically on narrow windows
- Windows 11 Snap Layouts support
- Manual refresh button (single global button, no per-card refresh)
- Refresh includes: system info, hardware info, and service reconnection
- Visual feedback during refresh (spinner, disabled state)
- Real-time updates for dynamic values (service status, timer)

**Title Bar:**
- Theme selector located in the title bar
- Extended view into title bar (modern WinUI pattern)

**Keyboard Navigation Order:**
1. Theme selector
2. Refresh button
3. Service type dropdown
4. Start/Stop button

### Themes

| Theme | Description |
|-------|-------------|
| Light | Standard light theme |
| Dark | Standard dark theme |
| Cyberpunk | Custom neon/cyber aesthetic |
| High Contrast Dark | Accessibility-focused dark theme |
| High Contrast Light | Accessibility-focused light theme |

- Follow system theme by default
- Manual theme override available in settings
- Real-time theme switching without app restart

**Cyberpunk Theme Specification:**
*(Color palette to be defined during implementation)*

| Element | Description |
|---------|-------------|
| Primary color | Neon pink or cyan |
| Background color | Dark purple or black |
| Accent color | Neon green or yellow |
| Font style | Standard (no stylized fonts) |
| Glow effects | Optional on cards/buttons |

**High Contrast Themes:**
- Use Windows system high contrast colors
- Adapt to user's Windows high contrast settings when enabled

### Accessibility

| Requirement | Status |
|-------------|--------|
| Screen reader compatibility | Required |
| Full keyboard navigation | Required |
| High contrast theme support | Required |
| UI Automation properties | Required |

**AutomationProperties Scope:**
- All interactive controls (buttons, dropdowns, etc.)
- Cards/groups with automation peers
- Static text labels are excluded

### Configuration Storage

User preferences stored in `ApplicationData.Current.LocalSettings` (packaged app storage).

### Localization

Use resource files (`.resw`) from the start for all user-facing strings to enable future localization.

---

## Architecture

```
DefaultApp/
├── DefaultApp/                        # Main WinUI 3 Application
│   ├── App.xaml(.cs)
│   ├── MainWindow.xaml(.cs)
│   ├── Views/
│   │   └── MainPage.xaml(.cs)
│   ├── ViewModels/
│   │   └── MainViewModel.cs
│   ├── Services/
│   │   ├── SystemInfoService.cs
│   │   ├── HardwareInfoService.cs
│   │   ├── ActivationService.cs
│   │   └── ServiceController.cs
│   ├── Models/
│   │   ├── OsInfo.cs
│   │   ├── ArchitectureInfo.cs
│   │   └── ServiceStatus.cs
│   ├── Themes/
│   │   ├── Cyberpunk.xaml
│   │   ├── HighContrastDark.xaml
│   │   └── HighContrastLight.xaml
│   ├── Strings/
│   │   └── en-US/
│   │       └── Resources.resw
│   └── Package.appxmanifest
│
├── DefaultApp.BackgroundTasks/        # Background Task Project
│   └── TimerBackgroundTask.cs
│
├── DefaultApp.AppService/             # App Service Project
│   └── EventLogAppService.cs
│
├── DefaultApp.FullTrustProcess/       # Win32 Service Project
│   ├── Program.cs
│   ├── NamedPipeServer.cs
│   └── EventLoggerService.cs
│
├── DefaultApp.Tests/                  # Unit Test Project
│   ├── Services/
│   │   └── SystemInfoServiceTests.cs
│   └── ViewModels/
│       └── MainViewModelTests.cs
│
└── DefaultApp.sln
```

**Architecture Notes:**
- No dependency injection (keep simple for demo app)
- Named Pipes for Full Trust Process ↔ WinUI communication

---

## UI Layout

```
┌─────────────────────────────────────────────────────────────────┐
│  Default App                          [Theme ▼] [Refresh]  _ □ X │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Operating System                                          │ │
│  ├────────────────────────────────────────────────────────────┤ │
│  │  OS Name:              Microsoft Windows                   │ │
│  │  Version:              10.0.22631.0                        │ │
│  │  Build:                22631                               │ │
│  │  Edition:              Pro                                 │ │
│  │  64-bit OS:            Yes                                 │ │
│  │  System Locale:        en-US                               │ │
│  │  Activation Status:    Activated                           │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Device & Hardware                                         │ │
│  ├────────────────────────────────────────────────────────────┤ │
│  │  Machine Name:         DESKTOP-ABC123                      │ │
│  │  Processor Arch:       X64                                 │ │
│  │  OS Architecture:      X64                                 │ │
│  │  Emulation:            No                                  │ │
│  │  CPU Model:            Intel Core i7-12700K                │ │
│  │  Processor Count:      20                                  │ │
│  │  Total RAM:            31.7 GB                             │ │
│  │  64-bit Process:       Yes                                 │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │  Packaged Service                                          │ │
│  ├────────────────────────────────────────────────────────────┤ │
│  │  Service Type:  [ Background Task ▼ ]                      │ │
│  │                                                            │ │
│  │  Status:        ● Running                                  │ │
│  │  Uptime:        00:05:32                                   │ │
│  │  Current Time:  14:32:15 (from timer service)              │ │
│  │                                                            │ │
│  │  [ Stop Service ]                                          │ │
│  └────────────────────────────────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Package Manifest Extensions

```xml
<Extensions>
  <!-- Background Task for Timer Service -->
  <Extension Category="windows.backgroundTasks"
             EntryPoint="DefaultApp.BackgroundTasks.TimerBackgroundTask">
    <BackgroundTasks>
      <Task Type="timer"/>
      <Task Type="general"/>
    </BackgroundTasks>
  </Extension>

  <!-- App Service for Event Log Reader -->
  <uap:Extension Category="windows.appService">
    <uap:AppService Name="com.contoso.defaultapp.eventlogservice"/>
  </uap:Extension>

  <!-- Full Trust Process for Continuous Logger -->
  <desktop:Extension Category="windows.fullTrustProcess"
                      Executable="DefaultApp.FullTrustProcess.exe"/>
</Extensions>

<Capabilities>
  <rescap:Capability Name="runFullTrust"/>
</Capabilities>
```

---

## Error Handling Strategy

| Scenario | Handling |
|----------|----------|
| Registry access failure | Display "Unable to retrieve" for affected field |
| P/Invoke failure | Display "Unavailable" for affected field |
| Service start failure | Show error dialog prompting user to restart app |
| Service stop failure | Show error notification |
| General exceptions | Log to file, display user-friendly error message |

**Graceful Degradation:**
- Individual system info items that fail to retrieve show "Unavailable"
- App continues to function with available data
- No error tooltips or hidden fields - always show the field with status

---

## Logging & Diagnostics

| Feature | Implementation |
|---------|----------------|
| Debug logging | Write to `%LocalAppData%\DefaultApp\Logs\` |
| ETW Provider | Auto-generated GUID, no manifest registration |
| ETW to file | ETW events also written to file log |
| Crash reporting | Windows Error Reporting integration |
| Audit logging | Log all service start/stop events with timestamps |

**Log Rotation Policy:**

| Setting | Value |
|---------|-------|
| Maximum file size | 10 MB |
| Files retained | 5 |
| Auto cleanup | Yes |

**Log File Format:** `DefaultApp_YYYYMMDD.log`

**Log Levels:**

| Level | Purpose |
|-------|---------|
| Trace | Verbose debugging |
| Debug | Development diagnostics |
| Information | Normal operation events |
| Warning | Potential issues |
| Error | Failures |

---

## Data Refresh Strategy

| Data Type | Refresh Behavior |
|-----------|------------------|
| OS Information | On app launch + manual refresh button |
| Hardware Information | On app launch + manual refresh button |
| Service Status | Real-time updates (polling every 500ms) |
| Timer Display | Real-time (1 second interval from service) |

**Adaptive Polling:**
- Service status polling: 500ms when app is in foreground
- Reduce polling to 2-5 seconds when app is minimized or in background
- Timer display continues at 1 second interval regardless of focus

**Refresh Button Behavior:**
- Refreshes system info, hardware info, AND reconnects services
- Shows spinner and disabled state during refresh
- Single global refresh button (no per-card refresh)

---

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.WindowsAppSDK | 1.5+ | WinUI 3 Framework |
| Microsoft.Windows.SDK.BuildTools | 10.0.22621+ | Windows SDK |
| CommunityToolkit.Mvvm | 8.2+ | MVVM Pattern |
| CommunityToolkit.WinUI.UI.Controls | 8.0+ | UI Components |
| Microsoft.Extensions.Logging | 8.0+ | Logging abstractions |

---

## Testing Requirements

| Test Type | Framework/Approach |
|-----------|-------------------|
| Unit Tests | xUnit / MSTest |
| UI Automation | Microsoft.Windows.Apps.Test |
| Accessibility (CI) | Automated accessibility tests in pipeline |
| Accessibility (Manual) | Screen reader testing (Narrator, NVDA, JAWS) |
| Manual Testing | Windows 11 22H2, 23H2, 24H2 |
| ARM64 Testing | Native ARM64 and x64 emulation scenarios |

**Test Coverage Target:** 80% code coverage for service layer

---

## Build & Deployment

### CI/CD Pipeline

| Feature | Implementation |
|---------|----------------|
| Platform | GitHub Actions |
| MSIX Signing | Automated in pipeline |
| Store Submission | Automated |

### Code Signing

| Setting | Value |
|---------|-------|
| Signing Method | Azure Artifact |
| Certificate Source | `C:\Temp\tsscat\CodeSigning` |

### Build Commands

```bash
dotnet restore
dotnet build -c Release
dotnet test
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r win-arm64
```

### Build Architectures

| Architecture | Included |
|--------------|----------|
| x64 | Yes |
| ARM64 | Yes |
| AnyCPU | Yes |
| x86 (32-bit) | No |

### Debug Build Features

Debug builds include additional development tools:

| Feature | Description |
|---------|-------------|
| Verbose console logging | Extended logging output to console |
| Debug menu/page | Testing and diagnostic UI |
| Mock data options | Simulated service responses for testing |

### MSIX Packaging

- Package created via Visual Studio or `MakeAppx.exe`
- Signed via Azure Artifact in CI pipeline
- ARM64 and x64 packages for Store submission

---

## Security Considerations

| Item | Handling |
|------|----------|
| Machine name display | Displayed clearly (no obfuscation) |
| Service audit logging | All service starts/stops logged with timestamp |
| Sensitive data | No sensitive data collected or stored |

---

## Future Considerations

- Clipboard copy functionality for system info
- Export system information to JSON/XML
- Additional language localizations
- Additional service demonstration types
- System tray presence for background service
