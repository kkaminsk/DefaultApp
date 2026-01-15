## ADDED Requirements

### Requirement: Five Theme Options

The application SHALL provide five distinct themes for user selection.

#### Scenario: All themes are available
- **WHEN** opening the theme selector
- **THEN** Light, Dark, Cyberpunk, High Contrast Dark, and High Contrast Light options are available
- **AND** System Default option follows OS theme

#### Scenario: Light theme provides standard appearance
- **WHEN** Light theme is selected
- **THEN** standard WinUI 3 light colors are applied
- **AND** text is dark on light backgrounds

#### Scenario: Dark theme provides standard appearance
- **WHEN** Dark theme is selected
- **THEN** standard WinUI 3 dark colors are applied
- **AND** text is light on dark backgrounds

### Requirement: Cyberpunk Theme

The application SHALL provide a custom Cyberpunk theme with neon aesthetic.

#### Scenario: Cyberpunk uses neon colors
- **WHEN** Cyberpunk theme is selected
- **THEN** primary color is neon pink or cyan
- **AND** background is dark purple or black
- **AND** accent color is neon green or yellow

#### Scenario: Cyberpunk maintains accessibility
- **WHEN** Cyberpunk theme is active
- **THEN** text contrast meets accessibility requirements
- **AND** standard fonts are used (no stylized fonts)

#### Scenario: Cyberpunk may include glow effects
- **WHEN** Cyberpunk theme is active
- **THEN** cards and buttons may have optional glow effects
- **AND** glow effects do not impair readability

### Requirement: High Contrast Themes

The application SHALL provide high contrast themes that integrate with Windows accessibility settings.

#### Scenario: High Contrast uses system colors
- **WHEN** High Contrast Dark or Light theme is selected
- **THEN** Windows system high contrast colors are used
- **AND** theme adapts to user's Windows high contrast settings

#### Scenario: Automatic high contrast detection
- **WHEN** Windows high contrast mode is enabled
- **THEN** the application automatically switches to appropriate high contrast theme
- **AND** decorative elements are minimized for clarity

### Requirement: Real-Time Theme Switching

The application SHALL apply theme changes immediately without requiring restart.

#### Scenario: Theme changes apply instantly
- **WHEN** a new theme is selected
- **THEN** colors update immediately across all UI elements
- **AND** no application restart is required

#### Scenario: Theme selection persists
- **WHEN** a theme is selected
- **THEN** the choice is saved to LocalSettings
- **AND** the same theme is applied on next launch

### Requirement: System Default Theme

The application SHALL follow the operating system theme by default.

#### Scenario: System Default follows OS
- **WHEN** System Default is selected
- **THEN** Light theme is used when OS is in light mode
- **AND** Dark theme is used when OS is in dark mode

#### Scenario: OS theme changes are detected
- **WHEN** the OS theme changes while app is running
- **AND** System Default is selected
- **THEN** the app theme updates automatically
