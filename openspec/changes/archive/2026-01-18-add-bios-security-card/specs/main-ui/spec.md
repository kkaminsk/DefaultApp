## ADDED Requirements

### Requirement: BIOS & Security Card

The main page SHALL display a third card for BIOS and security information.

#### Scenario: BIOS & Security card is visible
- **WHEN** the application launches
- **THEN** a "BIOS & Security" card is displayed after the Device & Hardware card
- **AND** the card has a header and content area

#### Scenario: BIOS Manufacturer is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the BIOS Manufacturer field is displayed with value from Registry
- **AND** a copy to clipboard button is available

#### Scenario: BIOS Name is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the BIOS Name field is displayed
- **AND** a copy to clipboard button is available

#### Scenario: BIOS Version is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the BIOS Version field is displayed
- **AND** a copy to clipboard button is available

#### Scenario: BIOS Release Date is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the BIOS Release Date field is displayed
- **AND** a copy to clipboard button is available

#### Scenario: SMBIOS Version is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the SMBIOS Version field is displayed (format: Major.Minor)
- **AND** a copy to clipboard button is available

#### Scenario: Secure Boot Status is displayed
- **WHEN** viewing the BIOS & Security card
- **THEN** the Secure Boot Status field shows "Enabled" or "Disabled"
- **AND** no copy button is displayed for this field

### Requirement: BIOS Data Source

BIOS information SHALL be retrieved from the Windows Registry without using WMI.

#### Scenario: BIOS Registry retrieval
- **WHEN** retrieving BIOS information
- **THEN** values are read from `HKLM\HARDWARE\DESCRIPTION\System\BIOS`
- **AND** Secure Boot status is read from `HKLM\SYSTEM\CurrentControlSet\Control\SecureBoot\State`
- **AND** WMI is not used
