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

This is a Windows 11 MSIX packaged application built with WinUI 3 and .NET 8. The application displays system information and demonstrates three types of MSIX packaged services.

## Key Files

| File | Purpose |
|------|---------|
| `APPLICATION_SPECIFICATION.md` | Complete application specification |
| `CLARIFICATIONS.md` | First round of design clarifications |
| `CLARIFICATIONS2.md` | Second round of implementation details |
| `CLARIFICATIONS3.md` | Third round - implementation decisions (resolved) |
| `openspec/project.md` | Project context for OpenSpec |
| `openspec/changes/` | Implementation proposals (10 total) |

## Technology Stack

- **Framework:** .NET 8 / WinUI 3 (Windows App SDK 1.5+)
- **Package Format:** MSIX (Single-Project)
- **Target OS:** Windows 11 (Build 22000+)
- **Language:** C#

## Architecture

```
DefaultApp/                        # Main WinUI 3 Application
DefaultApp.BackgroundTasks/        # Background Task Project
DefaultApp.AppService/             # App Service Project
DefaultApp.FullTrustProcess/       # Win32 Service (Named Pipes IPC)
DefaultApp.Tests/                  # Unit Tests
```

## Key Design Decisions

### No WMI
WMI queries have been explicitly removed. Use Registry and P/Invoke for system information instead.

### No Dependency Injection
Keep the demo app simple - no DI container.

### Named Pipes for IPC
Full Trust Process communicates with main app via Named Pipes.

### Localization Ready
Use `.resw` resource files from the start for all user-facing strings.

### Themes
Five themes: Light, Dark, Cyberpunk, High Contrast Dark, High Contrast Light.

## Build Commands

```bash
dotnet restore
dotnet build -c Release
dotnet test
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r win-arm64
```

## Code Signing

- Method: Azure Artifact
- Certificate location: `C:\Temp\tsscat\CodeSigning`

## Testing

- Unit tests: xUnit/MSTest
- UI automation: Microsoft.Windows.Apps.Test
- Target coverage: 80% for service layer

## Important Constraints

1. **Services stop when app closes** - No persistent background services
2. **Auto-stop on service switch** - Changing service type stops the current one first
3. **Single refresh button** - No per-card refresh, refreshes everything including services
4. **RAM format** - Always display as GB with 1 decimal (e.g., "31.7 GB")
5. **Multi-processor** - Display all CPUs in a list, not summarized

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

## Service Status Colors

| State | Color |
|-------|-------|
| Starting | Yellow/Orange |
| Running | Green |
| Stopping | Yellow/Orange |
| Stopped | Gray |
| Error | Red |

## Logging

- Location: `%LocalAppData%\DefaultApp\Logs\`
- Format: `DefaultApp_YYYYMMDD.log`
- Max size: 10 MB per file
- Retention: 5 files
- ETW: Auto-generated GUID, also writes to file log

## Implementation Proposals

Ten OpenSpec proposals define the implementation plan. Execute in dependency order:

| # | Proposal | Description | Dependencies |
|---|----------|-------------|--------------|
| 1 | `add-project-foundation` | Solution, projects, MSIX config | None |
| 2 | `add-system-info-services` | OS/hardware info, P/Invoke | #1 |
| 3 | `add-logging-infrastructure` | File logging, ETW, rotation | #1 |
| 4 | `add-main-ui-layout` | Three-card UI, accessibility | #1, #2 |
| 5 | `add-theme-system` | All five themes | #1, #4 |
| 6 | `add-background-task-service` | Timer service (default) | #1, #4 |
| 7 | `add-app-service` | Event Log Reader | #1, #4, #6 |
| 8 | `add-full-trust-process` | Named Pipes, Win32 logger | #1, #4, #6 |
| 9 | `add-testing-infrastructure` | Unit tests, UI automation | All services |
| 10 | `add-build-packaging` | CI/CD, MSIX signing | All above |

### Working with Proposals

```bash
# List all proposals
openspec list

# View a specific proposal
openspec show add-project-foundation

# Validate a proposal
openspec validate add-project-foundation --strict

# Apply a proposal (after approval)
/openspec:apply add-project-foundation
```

### Proposal Structure

Each proposal contains:
- `proposal.md` - Why and what changes
- `tasks.md` - Implementation checklist
- `specs/<capability>/spec.md` - Requirements with scenarios
