## ADDED Requirements

### Requirement: DNS Server Ping Button

The Network Information card SHALL include a Ping button next to the DNS Server field that allows users to test connectivity to the DNS server.

#### Scenario: Initial button state

- **WHEN** the Network Information card is displayed
- **THEN** the DNS Server Ping button displays "Ping" as its text
- **AND** the button has no background color

#### Scenario: Ping in progress

- **WHEN** the user clicks the DNS Server Ping button
- **THEN** the button text updates to show progress (e.g., "0/5", "1/5", "2/5")

#### Scenario: Ping completes successfully with all responses

- **WHEN** all 5 ping attempts complete successfully
- **THEN** the button displays "5/5"
- **AND** the button background turns green

#### Scenario: Ping completes with partial success

- **WHEN** some but not all ping attempts succeed
- **THEN** the button displays "X/5" where X is 1-4
- **AND** the button background turns yellow

#### Scenario: Ping completes with no responses

- **WHEN** all ping attempts fail
- **THEN** the button displays "0/5"
- **AND** the button background turns red

#### Scenario: Reset after viewing results

- **WHEN** the button is showing results (not "Ping")
- **AND** the user clicks the button
- **THEN** the button resets to display "Ping"
- **AND** the button background is cleared

#### Scenario: No DNS server available

- **WHEN** the DNS Server value is "Unavailable" or empty
- **THEN** the Ping button click does nothing
