# main-ui Specification Delta

## MODIFIED Requirements

### Requirement: Copy to Clipboard Buttons

The application SHALL provide copy to clipboard buttons with visual feedback for commonly-copied system information fields.

#### Scenario: Copy button shows success feedback
- **WHEN** a copy button is clicked
- **THEN** the button icon changes to a checkmark
- **AND** the checkmark displays for approximately 1 second
- **AND** the icon reverts to the copy icon automatically

#### Scenario: Copy button has hover state
- **WHEN** hovering over a copy button
- **THEN** a subtle background highlight appears
- **AND** the highlight uses theme-appropriate colors

### Requirement: Consistent Empty State Display

The UI SHALL display "N/A" consistently when data is unavailable.

#### Scenario: Missing OS data shows N/A
- **WHEN** any OS information cannot be retrieved
- **THEN** the property value displays "N/A"
- **AND** no other fallback text is used

#### Scenario: Missing hardware data shows N/A
- **WHEN** any hardware information cannot be retrieved
- **THEN** the property value displays "N/A"
- **AND** no other fallback text is used

#### Scenario: Missing network data shows N/A
- **WHEN** any network information cannot be retrieved
- **THEN** the property value displays "N/A"
- **AND** no other fallback text is used

### Requirement: Card Visual Structure

Cards SHALL have clean visual structure without trailing decorative elements.

#### Scenario: No trailing separators before actions
- **WHEN** a card contains an action button (e.g., Test Audio)
- **THEN** no separator line appears immediately before the button

#### Scenario: No trailing separators at card end
- **WHEN** viewing the last property in a card
- **THEN** no separator line appears after the final property value
