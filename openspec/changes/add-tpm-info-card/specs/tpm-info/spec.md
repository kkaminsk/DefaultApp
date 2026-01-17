# TPM Information Capability

## ADDED Requirements

### Requirement: Application SHALL display TPM Information card

The application SHALL display a TPM Information card at the bottom of the main page showing TPM hardware details.

#### Scenario: TPM card displays on main page
- **Given** the main page is loaded
- **When** the user scrolls to the bottom
- **Then** a "TPM Information" card is visible

### Requirement: TPM card SHALL display four properties

The TPM Information card SHALL display the following properties:
- SpecVersion
- ManufacturerId
- ManufacturerVersion
- PhysicalPresenceVersionInfo

#### Scenario: All TPM properties displayed
- **Given** the TPM Information card is visible
- **And** the system has a TPM
- **Then** all four TPM properties are displayed with their values

#### Scenario: TPM not available
- **Given** the TPM Information card is visible
- **And** the system does not have a TPM
- **Then** all TPM properties display "Unavailable"

### Requirement: Each TPM property SHALL have copy button

Each TPM property SHALL have a copy-to-clipboard button that copies the property value.

#### Scenario: User copies TPM SpecVersion
- **Given** the TPM Information card is visible
- **When** the user clicks the copy button for SpecVersion
- **Then** the SpecVersion value is copied to clipboard

#### Scenario: User copies TPM ManufacturerId
- **Given** the TPM Information card is visible
- **When** the user clicks the copy button for ManufacturerId
- **Then** the ManufacturerId value is copied to clipboard

#### Scenario: User copies TPM ManufacturerVersion
- **Given** the TPM Information card is visible
- **When** the user clicks the copy button for ManufacturerVersion
- **Then** the ManufacturerVersion value is copied to clipboard

#### Scenario: User copies TPM PhysicalPresenceVersionInfo
- **Given** the TPM Information card is visible
- **When** the user clicks the copy button for PhysicalPresenceVersionInfo
- **Then** the PhysicalPresenceVersionInfo value is copied to clipboard
