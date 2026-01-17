## ADDED Requirements

### Requirement: Serial Number Display

The application SHALL display the device serial number in the Device & Hardware card.

#### Scenario: Serial number is displayed
- **WHEN** viewing the Device & Hardware card
- **THEN** the Serial Number field is displayed below Device Model
- **AND** the value shows the system or baseboard serial number

#### Scenario: Serial number has copy button
- **WHEN** viewing the Serial Number field
- **THEN** a copy to clipboard button is displayed beside the value
- **AND** clicking it copies the serial number to the clipboard

#### Scenario: Serial number unavailable
- **WHEN** the serial number cannot be retrieved from Registry
- **THEN** the field displays "Unavailable"

### Requirement: Serial Number Data Source

The serial number SHALL be retrieved from the Windows Registry without using WMI.

#### Scenario: Registry retrieval with fallback
- **WHEN** retrieving the serial number
- **THEN** the value is first attempted from `HKLM\HARDWARE\DESCRIPTION\System\BIOS\SystemSerialNumber`
- **AND** if not found, falls back to `BaseBoardSerialNumber`
- **AND** WMI is not used
