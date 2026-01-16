## ADDED Requirements

### Requirement: Device Model Display

The application SHALL display the device model in the Device & Hardware card.

#### Scenario: Device model is displayed
- **WHEN** viewing the Device & Hardware card
- **THEN** the Device Model field is displayed below Processor Count
- **AND** the value shows the system product name (e.g., "Surface Pro 9", "Dell XPS 15 9520")

#### Scenario: Device model has copy button
- **WHEN** viewing the Device Model field
- **THEN** a copy to clipboard button is displayed beside the value
- **AND** clicking it copies the device model to the clipboard

#### Scenario: Device model unavailable
- **WHEN** the device model cannot be retrieved from Registry
- **THEN** the field displays "Unavailable"

### Requirement: Device Model Data Source

The device model SHALL be retrieved from the Windows Registry without using WMI.

#### Scenario: Registry retrieval
- **WHEN** retrieving the device model
- **THEN** the value is read from `HKLM\HARDWARE\DESCRIPTION\System\BIOS\SystemProductName`
- **AND** WMI is not used
