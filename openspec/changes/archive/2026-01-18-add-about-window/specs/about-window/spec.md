# About Window Capability

## ADDED Requirements

### Requirement: Info button in title bar SHALL open About window

The main window SHALL display an info button in the title bar that opens the About window.

#### Scenario: User clicks info button
- **Given** the main window is displayed
- **When** the user clicks the info button in the title bar
- **Then** the About window opens

### Requirement: About window SHALL display version 1.1 information

The About window SHALL display the application name, version, and release notes for version 1.1 only.

#### Scenario: About window shows correct content
- **Given** the About window is open
- **Then** the window displays:
  - Application name "Default App"
  - Version number (from package manifest)
  - Release notes for version 1.1:
    - Added splash screen
    - Added information window
    - Enhanced UI
    - Removed unused code
    - Fixed bugs
  - Author: Kevin Kaminski MVP
  - Organization: Big Hat Group Inc.
  - License: MIT

#### Scenario: Version 0.5 content is not displayed
- **Given** the About window is open
- **Then** no version 0.5 release notes are shown

### Requirement: About window SHALL be closable

The About window SHALL provide a close button for the user to dismiss it.

#### Scenario: User closes About window
- **Given** the About window is open
- **When** the user clicks the close button
- **Then** the About window closes
