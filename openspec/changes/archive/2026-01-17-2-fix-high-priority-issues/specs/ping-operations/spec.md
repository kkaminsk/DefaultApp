## ADDED Requirements

### Requirement: Unified Ping Implementation

Ping operations MUST share a common implementation to reduce code duplication.

#### Scenario: Ping gateway
- **WHEN** the user initiates a gateway ping
- **THEN** the common ping method SHALL be invoked with the gateway address
- **AND** results SHALL update the gateway ping button

#### Scenario: Ping DNS server
- **WHEN** the user initiates a DNS server ping
- **THEN** the common ping method SHALL be invoked with the DNS server address
- **AND** results SHALL update the DNS ping button

#### Scenario: Ping Google DNS
- **WHEN** the user initiates a Google DNS ping
- **THEN** the common ping method SHALL be invoked with "8.8.8.8"
- **AND** results SHALL update the Google DNS ping button

#### Scenario: Consistent behavior across all ping types
- **WHEN** any ping operation completes
- **THEN** the button text SHALL show "X/5" format
- **AND** the button background SHALL be green (5/5), yellow (1-4), or red (0/5)
