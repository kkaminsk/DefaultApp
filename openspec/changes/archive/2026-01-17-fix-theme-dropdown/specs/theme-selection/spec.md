# Theme Selection

## MODIFIED Requirements

### Requirement: Theme dropdown applies selected theme

The theme dropdown in the title bar SHALL apply the selected theme to the application UI when a user selects a theme option.

#### Scenario: User selects Light theme

- **Given** the application is running
- **When** the user clicks the theme dropdown and selects "Light"
- **Then** the application UI switches to Light theme colors
- **And** the theme selection is persisted

#### Scenario: User selects Dark theme

- **Given** the application is running with Light theme
- **When** the user clicks the theme dropdown and selects "Dark"
- **Then** the application UI switches to Dark theme colors
- **And** cards, text, and controls use dark theme styling

#### Scenario: User selects Cyberpunk theme

- **Given** the application is running
- **When** the user clicks the theme dropdown and selects "Cyberpunk"
- **Then** the application UI switches to Cyberpunk theme with neon colors
- **And** card borders show pink/cyan accent colors

#### Scenario: Theme persists across app restarts

- **Given** the user has selected "Dark" theme
- **When** the application is closed and reopened
- **Then** the application starts with Dark theme applied
- **And** the theme dropdown shows "Dark" selected
