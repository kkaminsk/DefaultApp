# Proposal: Add About Window

## Change ID
`add-about-window`

## Summary

Add an info button to the main window title bar that opens an About window displaying version 1.1 release information.

## Motivation

Users need a way to view application version and release notes. An info button in the title bar provides quick access to this information following standard Windows app patterns.

## Scope

### In Scope
- Info button with icon in the title bar (next to theme selector)
- New About window displaying:
  - App name and version
  - Version 1.1 release notes
  - Author and organization information
- Close button to dismiss the About window

### Out of Scope
- Version 0.5 release notes (explicitly excluded per requirements)
- Update checking functionality
- Links to external resources

## Solution Overview

1. Add an info button (using SegoeUI Symbol font info icon) to MainWindow title bar
2. Create a new `AboutWindow.xaml` with styled content
3. Wire button click to open the About window as a modal dialog

## Content to Display

```
Default App
Version 1.1

Release Notes:
- Added splash screen
- Added information window
- Enhanced UI
- Removed unused code
- Fixed bugs

Author: Kevin Kaminski MVP
Organization: Big Hat Group Inc.
License: MIT
```

## UI Placement

The info button will be placed in the title bar between the theme selector and the window control buttons area.

```
[Icon] Default App          [DragRegion]    [Theme ▼] [ℹ] [— □ ✕]
```

## Files to Create/Modify

| File | Action |
|------|--------|
| `AboutWindow.xaml` | Create - About window UI |
| `AboutWindow.xaml.cs` | Create - Code-behind |
| `MainWindow.xaml` | Modify - Add info button |
| `MainWindow.xaml.cs` | Modify - Add click handler |
