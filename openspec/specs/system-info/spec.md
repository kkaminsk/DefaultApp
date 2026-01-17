# system-info Specification

## Purpose
TBD - created by archiving change add-device-model. Update Purpose after archive.
## Requirements
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

### Requirement: Operating System Information Retrieval

The system SHALL retrieve and expose operating system details through SystemInfoService.

#### Scenario: OS name is retrieved
- **WHEN** requesting OS name
- **THEN** Environment.OSVersion.Platform value is returned

#### Scenario: OS version is retrieved
- **WHEN** requesting OS version
- **THEN** full version string from Environment.OSVersion.Version is returned

#### Scenario: Windows edition is retrieved from Registry
- **WHEN** requesting Windows edition
- **THEN** value from HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EditionID is returned
- **AND** "Unavailable" is returned if Registry access fails

#### Scenario: System locale is retrieved
- **WHEN** requesting system locale
- **THEN** CultureInfo.CurrentCulture name is returned (e.g., "en-US")

### Requirement: Windows Activation Status

The system SHALL retrieve Windows activation status using the SLIsGenuineLocal P/Invoke API.

#### Scenario: Activation check runs asynchronously
- **WHEN** activation status is requested
- **THEN** "Checking..." is displayed initially
- **AND** the actual check runs on a background thread

#### Scenario: Activation result is cached
- **WHEN** activation status is retrieved successfully
- **THEN** the result is cached for the session
- **AND** subsequent requests return the cached value immediately

#### Scenario: Full activation states are supported
- **WHEN** activation check completes
- **THEN** one of these states is returned: Activated, Not Activated, Grace Period, Notification Mode

### Requirement: Hardware Information Retrieval

The system SHALL retrieve hardware and architecture details through HardwareInfoService.

#### Scenario: Processor architecture is retrieved
- **WHEN** requesting processor architecture
- **THEN** RuntimeInformation.ProcessArchitecture is returned

#### Scenario: CPU models are retrieved from Registry
- **WHEN** requesting CPU model information
- **THEN** all processors from HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor\* are returned
- **AND** results are returned as a list for multi-processor systems

#### Scenario: Total RAM is formatted correctly
- **WHEN** requesting total RAM
- **THEN** value is displayed in GB with 1 decimal place (e.g., "31.7 GB")
- **AND** Windows.System.MemoryManager is used as primary source
- **AND** MEMORYSTATUSEX P/Invoke is used as fallback

#### Scenario: Emulation detection works
- **WHEN** ProcessArchitecture differs from OSArchitecture
- **THEN** "Running under emulation: Yes" is indicated
- **AND** native execution shows "Running under emulation: No"

### Requirement: Graceful Degradation

The system SHALL handle failures gracefully without crashing.

#### Scenario: Registry access failure is handled
- **WHEN** Registry access fails for any property
- **THEN** "Unavailable" is returned for that property
- **AND** other properties continue to function

#### Scenario: P/Invoke failure is handled
- **WHEN** a P/Invoke call fails
- **THEN** "Unavailable" is returned for that property
- **AND** the application continues to function

