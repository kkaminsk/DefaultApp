# System Info Display Improvements

## MODIFIED Requirements

### REQ-SYS-001: OS Name Display

The OS Name field SHALL display a user-friendly Windows version name combined with edition.

#### Scenario: Windows 11 Enterprise system
- **Given** the system build number is 22000 or higher
- **And** the Registry EditionID is "Enterprise"
- **When** the user views the Operating System card
- **Then** the OS Name field displays "Windows 11 Enterprise"

#### Scenario: Windows 11 Pro system
- **Given** the system build number is 22621
- **And** the Registry EditionID is "Professional"
- **When** the user views the Operating System card
- **Then** the OS Name field displays "Windows 11 Pro"

#### Scenario: Windows 10 system (if supported in future)
- **Given** the system build number is between 10240 and 21999
- **And** the Registry EditionID is "Enterprise"
- **When** the user views the Operating System card
- **Then** the OS Name field displays "Windows 10 Enterprise"

---

### REQ-SYS-002: Activation Status Detection

The Activation Status field SHALL use Registry-based detection for reliability in MSIX context.

#### Scenario: Activated Windows
- **Given** the Registry key `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\LicenseStatus` equals 1
- **When** the user views the Operating System card
- **Then** the Activation Status field displays "Activated"

#### Scenario: Not Activated Windows
- **Given** the Registry key LicenseStatus equals 0
- **When** the user views the Operating System card
- **Then** the Activation Status field displays "Not Activated"

#### Scenario: Registry unavailable
- **Given** the Registry key cannot be read
- **When** the activation status is checked
- **Then** the system falls back to P/Invoke method
- **And** displays "Unavailable" if both methods fail

---

### REQ-SYS-003: Machine Name Field Location

The Machine Name field SHALL appear in the Operating System card, positioned after the OS Name field.

#### Scenario: Field ordering in OS card
- **Given** the user views the Operating System card
- **When** the card is rendered
- **Then** fields appear in order: OS Name, Machine Name, Version, Build, Edition, 64-bit OS, System Locale, Activation Status

#### Scenario: Machine Name removed from Hardware card
- **Given** the user views the Device & Hardware card
- **When** the card is rendered
- **Then** Machine Name field is NOT present in that card

---

## ADDED Requirements

### REQ-SYS-004: Copy to Clipboard Functionality

Key fields SHALL have a copy-to-clipboard button for easy sharing.

#### Scenario: Copy Machine Name
- **Given** the user views the Operating System card
- **When** the user clicks the copy button beside Machine Name
- **Then** the Machine Name value is copied to the clipboard
- **And** a brief visual confirmation is shown

#### Scenario: Copy Version
- **Given** the user views the Operating System card
- **When** the user clicks the copy button beside Version
- **Then** the Version value is copied to the clipboard
- **And** a brief visual confirmation is shown

#### Scenario: Copy button accessibility
- **Given** a screen reader user navigates to a copy button
- **When** the button receives focus
- **Then** the screen reader announces "Copy [field name] to clipboard"

#### Scenario: Copy button tooltip
- **Given** the user hovers over a copy button
- **When** the tooltip delay elapses
- **Then** a tooltip displays "Copy to clipboard"
