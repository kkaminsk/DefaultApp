# window-positioning Specification

## Purpose
TBD - created by archiving change add-window-centering. Update Purpose after archive.
## Requirements
### Requirement: Window Centering on Launch

The application window SHALL be positioned at the center of the primary display's work area when launched.

#### Scenario: Normal launch on single monitor

- **WHEN** the application is launched
- **THEN** the window is positioned at the horizontal and vertical center of the primary display's work area (excluding taskbar and other reserved areas)

#### Scenario: High DPI display

- **WHEN** the application is launched on a display with DPI scaling (e.g., 125%, 150%, 200%)
- **THEN** the window is correctly centered accounting for the DPI scale factor

