# Project Context

## Purpose

Default App is a diagnostic utility that displays comprehensive system information including operating system details, device architecture, and hardware information. It demonstrates three types of MSIX packaged services: Background Task, App Service, and Full Trust Process.

**Target Users:** Developers and IT professionals who need to view system information and understand MSIX service capabilities.

## Tech Stack

- **Language:** C#
- **Framework:** .NET 8 / WinUI 3 (Windows App SDK 1.5+)
- **Platform:** Windows 11+ (Build 22000 minimum)
- **Package Format:** MSIX (Single-Project)
- **UI Toolkit:** Fluent Design System
- **MVVM:** CommunityToolkit.Mvvm 8.2+
- **Logging:** Microsoft.Extensions.Logging 8.0+

## Project Conventions

### Code Style

- No dependency injection (keep simple for demo app)
- Use resource files (`.resw`) for all user-facing strings
- P/Invoke for system APIs (`SLIsGenuineLocal`, `MEMORYSTATUSEX`)
- Registry access for CPU model and Windows Edition
- No WMI queries (removed for simplicity and reliability)

### Architecture Patterns

```
DefaultApp/                        # Main WinUI 3 Application
├── Views/                         # XAML pages
├── ViewModels/                    # MVVM ViewModels
├── Services/                      # Business logic services
├── Models/                        # Data models
├── Themes/                        # Custom theme resources
└── Strings/                       # Localization resources

DefaultApp.BackgroundTasks/        # In-process Background Task
DefaultApp.AppService/             # App Service for Event Log
DefaultApp.FullTrustProcess/       # Win32 Named Pipe service
DefaultApp.Tests/                  # Unit tests
```

**Key Services:**
- `SystemInfoService` - OS information retrieval
- `HardwareInfoService` - Hardware/architecture detection
- `ActivationService` - Windows activation status
- `ServiceController` - Packaged service lifecycle management

### Testing Strategy

| Test Type | Framework |
|-----------|-----------|
| Unit Tests | xUnit / MSTest |
| UI Automation | Microsoft.Windows.Apps.Test |
| Accessibility | Automated CI tests + manual screen reader testing |

**Coverage Target:** 80% for service layer

### Git Workflow

- **Version Scheme:** SemVer (1.0.0, 1.0.1, 1.1.0)
- **Main Branch:** `main`
- **CI/CD:** GitHub Actions with automated MSIX signing and Store submission

## Domain Context

### Packaged Services

The app demonstrates three MSIX service types:

1. **Background Task (Timer Service)**
   - In-process using `ApplicationTrigger`
   - Updates UI with current time every second via `DispatcherTimer`
   - Default service on app startup

2. **App Service (Event Log Reader)**
   - Reads Application Event Log (Warning/Error only, last 24 hours)
   - Returns 10 most recent events
   - Bidirectional communication with UI

3. **Full Trust Process (Continuous Logger)**
   - Win32 process via `windows.fullTrustProcess`
   - Named Pipes IPC: `\\.\pipe\DefaultApp_{ProcessId}`
   - JSON protocol with auto-reconnect (5 retries, 5s interval)

### System Information Sources

| Data | Source |
|------|--------|
| OS Version | `Environment.OSVersion` |
| Edition | Registry `HKLM\...\EditionID` |
| Activation | `SLIsGenuineLocal` P/Invoke |
| CPU Model | Registry `HKLM\HARDWARE\...\CentralProcessor\*` |
| RAM | `Windows.System.MemoryManager` (fallback: `MEMORYSTATUSEX`) |
| Architecture | `RuntimeInformation.ProcessArchitecture` / `OSArchitecture` |

### Themes

| Theme | Description |
|-------|-------------|
| Light | Standard light theme |
| Dark | Standard dark theme |
| Cyberpunk | Neon/cyber aesthetic (pink/cyan/green) |
| High Contrast Dark | Windows system high contrast |
| High Contrast Light | Windows system high contrast |

## Important Constraints

1. **No WMI** - All WMI queries have been removed
2. **No Dependency Injection** - Keep simple for demo purposes
3. **Services stop when app closes** - No persistent background services
4. **Auto-stop on service switch** - Changing service type stops current first
5. **Single refresh button** - No per-card refresh, refreshes everything
6. **RAM format** - Always GB with 1 decimal (e.g., "31.7 GB")
7. **Windows 11 only** - Minimum Build 22000
8. **runFullTrust capability** - Required for WinUI 3 and Full Trust Process

### Status Indicator Colors (Fixed)

| State | Color |
|-------|-------|
| Starting | Yellow/Orange |
| Running | Green |
| Stopping | Yellow/Orange |
| Stopped | Gray |
| Error | Red |

## External Dependencies

### NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.WindowsAppSDK | 1.5+ | WinUI 3 Framework |
| Microsoft.Windows.SDK.BuildTools | 10.0.22621+ | Windows SDK |
| CommunityToolkit.Mvvm | 8.2+ | MVVM Pattern |
| CommunityToolkit.WinUI.UI.Controls | 8.0+ | UI Components |
| Microsoft.Extensions.Logging | 8.0+ | Logging abstractions |

### Build & Signing

| Setting | Value |
|---------|-------|
| Signing Method | Azure Artifact |
| Certificate Source | `C:\Temp\tsscat\CodeSigning` |
| Architectures | x64, ARM64, AnyCPU (no x86) |

### Logging

- **Location:** `%LocalAppData%\DefaultApp\Logs\`
- **Format:** `DefaultApp_YYYYMMDD.log`
- **Max Size:** 10 MB per file
- **Retention:** 5 files
- **ETW:** Auto-generated GUID, also writes to file log
