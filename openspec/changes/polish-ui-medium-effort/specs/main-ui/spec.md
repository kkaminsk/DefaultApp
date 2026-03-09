# main-ui Specification Delta

## MODIFIED Requirements

### Requirement: Extended Title Bar

The application SHALL use an extended title bar with compact, well-styled controls.

#### Scenario: Theme selector is compact
- **WHEN** viewing the title bar
- **THEN** the theme selector does not exceed 140 pixels width
- **AND** all theme options remain accessible

#### Scenario: Icon buttons have consistent hover states
- **WHEN** hovering over any icon-only button in the title bar
- **THEN** a subtle background highlight appears
- **AND** the highlight matches the theme

## ADDED Requirements

### Requirement: About Window Layout

The About window SHALL have clear visual separation between sections.

#### Scenario: Header is separated from content
- **WHEN** the About window is displayed
- **THEN** a horizontal separator line appears between header and content
- **AND** the separator uses theme-appropriate colors

#### Scenario: App description is displayed
- **WHEN** the About window is displayed
- **THEN** a brief description appears below the version number
- **AND** the description reads "Windows system information utility"
