# themes Specification Delta

## MODIFIED Requirements

### Requirement: Cyberpunk Theme

The Cyberpunk theme SHALL maintain accessibility including visible focus indicators.

#### Scenario: Focus indicators are visible
- **WHEN** Cyberpunk theme is active
- **AND** a control receives keyboard focus
- **THEN** the focus ring is clearly visible against the dark purple background
- **AND** focus uses a high-contrast color (cyan or white)

### Requirement: Focus Visibility Across Themes

All themes SHALL provide clearly visible focus indicators.

#### Scenario: Focus is visible in Light theme
- **WHEN** Light theme is active
- **THEN** focus indicators have sufficient contrast against light backgrounds

#### Scenario: Focus is visible in Dark theme
- **WHEN** Dark theme is active
- **THEN** focus indicators have sufficient contrast against dark backgrounds

#### Scenario: Focus is visible in High Contrast themes
- **WHEN** High Contrast Dark or Light theme is active
- **THEN** focus indicators use system high contrast colors
