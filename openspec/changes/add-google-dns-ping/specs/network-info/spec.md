## ADDED Requirements

### Requirement: Google DNS Ping Row

The Network Information card SHALL include a Google DNS row at the bottom that displays the static IP address 8.8.8.8 and allows users to test external internet connectivity.

#### Scenario: Google DNS row display

- **WHEN** the Network Information card is displayed
- **THEN** a "Google DNS" row appears at the bottom of the card
- **AND** the value displays "8.8.8.8"

#### Scenario: Initial button state

- **WHEN** the Network Information card is displayed
- **THEN** the Google DNS Ping button displays "Ping" as its text
- **AND** the button has no background color

#### Scenario: Ping in progress

- **WHEN** the user clicks the Google DNS Ping button
- **THEN** the button text updates to show progress (e.g., "0/5", "1/5", "2/5")

#### Scenario: Ping completes successfully with all responses

- **WHEN** all 5 ping attempts to 8.8.8.8 complete successfully
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
