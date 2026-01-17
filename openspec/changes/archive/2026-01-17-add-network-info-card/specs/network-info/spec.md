## ADDED Requirements

### Requirement: Network Information Display

The application SHALL display network configuration information in a dedicated card including IP Address, Subnet Mask, Default Gateway, DNS Server, and MAC Address.

#### Scenario: Display network information

- **WHEN** the main page loads
- **THEN** the Network Information card displays the current network adapter's IP Address, Subnet Mask, Default Gateway, DNS Server, and MAC Address

#### Scenario: No active network adapter

- **WHEN** no active network adapter is found
- **THEN** the fields display "Unavailable"

### Requirement: Network Information Copy Buttons

Each network information field SHALL have a copy-to-clipboard button to allow users to easily copy values.

#### Scenario: Copy IP Address

- **WHEN** the user clicks the copy button next to IP Address
- **THEN** the IP Address value is copied to the clipboard

#### Scenario: Copy MAC Address

- **WHEN** the user clicks the copy button next to MAC Address
- **THEN** the MAC Address value is copied to the clipboard
