# main-ui Specification Delta

## ADDED Requirements

### Requirement: About Window Version Display

The About window SHALL display the current application version accurately.

#### Scenario: Version matches project version

- **GIVEN** the application version is 1.3.0
- **WHEN** the user opens the About window
- **THEN** "Version 1.3" is displayed below the application name

#### Scenario: Version is visible in header

- **GIVEN** the About window is open
- **WHEN** viewing the header section
- **THEN** the version text appears below "Default App" title
- **AND** uses secondary text styling for visual hierarchy
