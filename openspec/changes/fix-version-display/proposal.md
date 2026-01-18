# Proposal: Fix Version Display to 1.3

## Summary

Update the version number displayed in the About window and project files to 1.3.

## Motivation

The About window currently displays "Version 1.1" which is outdated. The csproj file shows version 1.2.0. Both need to be updated to reflect the current version 1.3.

## Current State

| Location | Current Value |
|----------|---------------|
| `AboutWindow.xaml` (line 33) | "Version 1.1" (hardcoded) |
| `DefaultApp.csproj` Version | 1.2.0 |
| `DefaultApp.csproj` AssemblyVersion | 1.2.0.0 |
| `DefaultApp.csproj` FileVersion | 1.2.0.0 |

## Approach

Update all version references to 1.3:
1. Change `AboutWindow.xaml` display text to "Version 1.3"
2. Update `DefaultApp.csproj` Version to 1.3.0
3. Update `DefaultApp.csproj` AssemblyVersion to 1.3.0.0
4. Update `DefaultApp.csproj` FileVersion to 1.3.0.0

## Scope

- **In scope**: Updating version numbers in About window and project file
- **Out of scope**: Dynamic version reading from assembly (could be a future enhancement)

## Risks

None - straightforward text/version updates.
