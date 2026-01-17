<!-- OPENSPEC:START -->
# OpenSpec Instructions

These instructions are for AI assistants working in this project.

Always open `@/openspec/AGENTS.md` when the request:
- Mentions planning or proposals (words like proposal, spec, change, plan)
- Introduces new capabilities, breaking changes, architecture shifts, or big performance/security work
- Sounds ambiguous and you need the authoritative spec before coding

Use `@/openspec/AGENTS.md` to learn:
- How to create and apply change proposals
- Spec format and conventions
- Project structure and guidelines

Keep this managed block so 'openspec update' can refresh the instructions.

<!-- OPENSPEC:END -->

# Claude Code Project Guide: Default App

## Project Overview

This is a Windows 11 MSIX packaged application built with WinUI 3 and .NET 8. The application displays system information including OS details and hardware information in a two-card UI layout.

## Key Files

| File | Purpose |
|------|---------|
| `openspec/project.md` | Project context for OpenSpec |
| `openspec/changes/` | Implementation proposals |

## Technology Stack

- **Framework:** .NET 8 / WinUI 3 (Windows App SDK 1.5+)
- **Package Format:** MSIX (Single-Project)
- **Target OS:** Windows 11 (Build 22000+)
- **Language:** C#

## Architecture

```
DefaultApp/                        # Main WinUI 3 Application
├── Views/                         # XAML pages (MainPage)
├── ViewModels/                    # MVVM ViewModels (MainViewModel)
├── Services/                      # Business logic services
│   ├── SystemInfoService.cs       # OS information retrieval
│   ├── HardwareInfoService.cs     # Hardware/architecture detection
│   ├── ActivationService.cs       # Windows activation status
│   ├── NetworkInfoService.cs      # Network info + ping
│   ├── BiosInfoService.cs         # BIOS/Secure Boot info
│   ├── TpmInfoService.cs          # TPM information
│   ├── ThemeService.cs            # Theme management
│   ├── LoggingService.cs          # File logging + ETW
│   └── NativeMethods.cs           # P/Invoke declarations
├── Models/                        # Data models
├── Themes/                        # Custom theme resources
└── Strings/                       # Localization resources

DefaultApp.Tests/                  # Unit Tests (xUnit)
```

## Key Design Decisions

### No WMI
WMI queries have been explicitly removed. Use Registry and P/Invoke for system information instead.

### No Dependency Injection
Keep the demo app simple - no DI container.

### Localization Ready
Use `.resw` resource files from the start for all user-facing strings.

### Themes
Two themes: System Default (follows Windows theme) and Inverted (opposite of system theme). High contrast mode is automatically detected and respected.

## Build Commands

```bash
dotnet restore
dotnet build -c Release
dotnet test
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r win-arm64
```

## Custom Skills

| Command | Description |
|---------|-------------|
| `/build-msix` | Build and sign the MSIX package using Azure Trusted Signing |

## Code Signing

- **Method:** Azure Trusted Signing
- **Certificate:** Big Hat Group Inc.
- **SDK Required:** Windows SDK 10.0.26100.0 (older versions don't support MSIX signing)
- **Config Location:** `C:\Temp\tsscat\CodeSigning`

### Signing Configuration

| Setting | Value |
|---------|-------|
| Metadata File | `C:\Temp\tsscat\CodeSigning\metadata-privategpt.json` |
| DLib Path | `C:\Temp\tsscat\CodeSigning\Microsoft.Trusted.Signing.Client.1.0.95\bin\x64\Azure.CodeSigning.Dlib.dll` |
| Timestamp Server | http://timestamp.acs.microsoft.com |

## Testing

- Unit tests: xUnit
- UI automation: Microsoft.Windows.Apps.Test
- Target coverage: 80% for service layer

## Important Constraints

1. **Single refresh button** - Refreshes all system information
2. **RAM format** - Always display as GB with 1 decimal (e.g., "31.7 GB")
3. **Multi-processor** - Display all CPUs in a list, not summarized

## P/Invoke Requirements

| Feature | API |
|---------|-----|
| Windows Activation | `SLIsGenuineLocal` |
| Total RAM | `MEMORYSTATUSEX` via `GlobalMemoryStatusEx` |

## Registry Keys Used

| Key | Purpose |
|-----|---------|
| `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EditionID` | Windows Edition |
| `HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor\*` | CPU Model(s) |

## Logging

- Location: `%LocalAppData%\DefaultApp\Logs\`
- Format: `DefaultApp_YYYYMMDD.log`
- Max size: 10 MB per file
- Retention: 5 files
- ETW: Auto-generated GUID, also writes to file log

## Working with Proposals

```bash
# List all proposals
openspec list

# View a specific proposal
openspec show <proposal-id>

# Apply a proposal (after approval)
/openspec:apply <proposal-id>
```

### Proposal Structure

Each proposal contains:
- `proposal.md` - Why and what changes
- `tasks.md` - Implementation checklist
- `specs/<capability>/spec.md` - Requirements with scenarios

## Code Review

A comprehensive code review was performed on 2026-01-16. See `codereview.md` in the project root for full details.

### Completed Proposals

| # | Proposal | Priority | Description |
|---|----------|----------|-------------|
| 1 | `1-fix-critical-issues` | Critical | Fire-and-forget async handling, MainViewModel disposal, window subclass cleanup |
| 2 | `2-fix-high-priority-issues` | High | Exception logging, duplicate ping code, BIOS bug fix, ThemeService memory leak |

### Remaining Proposals

| # | Proposal | Priority | Description |
|---|----------|----------|-------------|
| 3 | `3-fix-medium-priority-issues` | Medium | Splash delay, dead code removal, Debug.WriteLine cleanup, magic numbers |
| 4 | `4-add-unit-tests` | Low | Unit tests for service layer (target 80% coverage) |

### Known Issues (from code review)

**Medium Priority:**
- 5-second splash screen delay is excessive
- Unused `ApplyCustomTheme()`/`RemoveCustomTheme()` methods
- Debug.WriteLine statements in production code
- `GetTotalRamFromMemoryManager()` always returns 0
