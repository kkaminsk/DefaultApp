# main-ui Specification

## Purpose
TBD - created by archiving change add-copy-buttons. Update Purpose after archive.
## Requirements
### Requirement: Copy to Clipboard Buttons

The application SHALL provide copy to clipboard buttons for commonly-copied system information fields.

#### Scenario: OS Name has copy button
- **WHEN** viewing the Operating System card
- **THEN** the OS Name field has a copy to clipboard button
- **AND** clicking it copies the OS name value to the clipboard

#### Scenario: Build has copy button
- **WHEN** viewing the Operating System card
- **THEN** the Build field has a copy to clipboard button
- **AND** clicking it copies the build number to the clipboard

#### Scenario: Edition has copy button
- **WHEN** viewing the Operating System card
- **THEN** the Edition field has a copy to clipboard button
- **AND** clicking it copies the Windows edition to the clipboard

#### Scenario: System Locale has copy button
- **WHEN** viewing the Operating System card
- **THEN** the System Locale field has a copy to clipboard button
- **AND** clicking it copies the system locale to the clipboard

#### Scenario: CPU Model has copy button
- **WHEN** viewing the Device & Hardware card
- **THEN** the CPU Model field has a copy to clipboard button
- **AND** clicking it copies the CPU model name to the clipboard

### Requirement: Three-Card Layout

The main page SHALL display three information cards: Operating System, Device & Hardware, and Packaged Service.

#### Scenario: All cards are visible
- **WHEN** the application launches
- **THEN** three distinct cards are displayed
- **AND** each card has a header and content area

#### Scenario: Operating System card displays all properties
- **WHEN** viewing the Operating System card
- **THEN** OS Name, Version, Build, Edition, 64-bit OS, System Locale, and Activation Status are displayed

#### Scenario: Device & Hardware card displays all properties
- **WHEN** viewing the Device & Hardware card
- **THEN** Machine Name, Processor Arch, OS Architecture, Emulation, CPU Model, Processor Count, Total RAM, and 64-bit Process are displayed

### Requirement: Responsive Layout

The UI SHALL adapt to different window sizes while maintaining usability.

#### Scenario: Cards stack on narrow windows
- **WHEN** window width is below 1200 pixels
- **THEN** cards stack vertically
- **AND** all content remains accessible via scrolling

#### Scenario: Cards display side-by-side on wide windows
- **WHEN** window width is 1200 pixels or above
- **THEN** cards may display horizontally
- **AND** layout uses available space efficiently

#### Scenario: Minimum window size is enforced
- **WHEN** attempting to resize the window
- **THEN** minimum width of 800 pixels is enforced
- **AND** minimum height of 600 pixels is enforced

### Requirement: Extended Title Bar

The application SHALL use an extended title bar with controls integrated into the title area.

#### Scenario: Title bar contains theme selector
- **WHEN** viewing the title bar
- **THEN** a theme selector dropdown is visible
- **AND** it is positioned to the left of the window controls

#### Scenario: Title bar contains refresh button
- **WHEN** viewing the title bar
- **THEN** a refresh button is visible
- **AND** it is positioned between theme selector and window controls

### Requirement: Keyboard Navigation

The UI SHALL support full keyboard navigation with a defined tab order.

#### Scenario: Tab order follows specification
- **WHEN** pressing Tab key repeatedly
- **THEN** focus moves in order: Theme selector, Refresh button, Service type dropdown, Start/Stop button
- **AND** all interactive controls are reachable

### Requirement: Accessibility Support

The UI SHALL be accessible to users with disabilities.

#### Scenario: Screen reader can identify controls
- **WHEN** using a screen reader
- **THEN** all interactive controls have descriptive names
- **AND** cards are identified as groups

#### Scenario: Automation properties are set
- **WHEN** UI automation tools inspect the UI
- **THEN** AutomationProperties.Name is set on interactive controls
- **AND** AutomationProperties.AutomationId is set on cards

### Requirement: Refresh Functionality

The system SHALL provide a single global refresh mechanism.

#### Scenario: Refresh button updates all data
- **WHEN** clicking the refresh button
- **THEN** OS information is refreshed
- **AND** hardware information is refreshed
- **AND** service connection is re-established

#### Scenario: Visual feedback during refresh
- **WHEN** refresh is in progress
- **THEN** a spinner is displayed
- **AND** the refresh button is disabled
- **AND** the button re-enables when refresh completes

### Requirement: Service Card Controls

The Packaged Service card SHALL contain controls for service management.

#### Scenario: Service type can be selected
- **WHEN** clicking the service type dropdown
- **THEN** Background Task, App Service, and Full Trust Process options are available

#### Scenario: Service status is displayed
- **WHEN** a service is running
- **THEN** status indicator shows colored circle
- **AND** uptime displays in HH:MM:SS format
- **AND** service-specific content is shown

#### Scenario: Uptime shows N/A when stopped
- **WHEN** no service is running
- **THEN** uptime displays "N/A"

