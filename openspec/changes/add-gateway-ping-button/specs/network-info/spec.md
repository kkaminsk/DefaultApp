## ADDED Requirements

### Requirement: Gateway Ping Button

The Network Information card SHALL include a Ping button next to the Default Gateway field that allows users to test connectivity to the gateway.

#### Scenario: Initial button state

- **WHEN** the Network Information card is displayed
- **THEN** the Ping button displays "Ping" as its text

#### Scenario: Ping in progress

- **WHEN** the user clicks the Ping button
- **THEN** the button is disabled during the ping operation
- **AND** the button text updates to show progress (e.g., "1/5", "2/5")

#### Scenario: Ping completes successfully

- **WHEN** all 5 ping attempts complete
- **THEN** the button displays the result as "X/5" where X is the number of successful pings
- **AND** the button is re-enabled

#### Scenario: No gateway available

- **WHEN** the Default Gateway is "Unavailable" or empty
- **THEN** the Ping button is disabled
