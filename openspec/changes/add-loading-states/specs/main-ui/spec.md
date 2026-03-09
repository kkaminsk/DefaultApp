# main-ui Specification Delta

## ADDED Requirements

### Requirement: Skeleton Loading States

The UI SHALL display skeleton placeholders while data is loading.

#### Scenario: Skeleton cards display during initial load
- **WHEN** the application is loading system information
- **THEN** skeleton placeholder cards are displayed
- **AND** placeholder shapes indicate where content will appear
- **AND** real content replaces skeletons when loading completes

#### Scenario: Skeleton uses theme-appropriate colors
- **WHEN** skeleton placeholders are displayed
- **THEN** placeholder colors match the current theme
- **AND** placeholders are visually distinct from real content

#### Scenario: Shimmer animation is subtle
- **WHEN** skeleton placeholders are displayed
- **THEN** an optional shimmer effect may animate across placeholders
- **AND** the animation is subtle and non-distracting
- **AND** animation respects Windows reduced motion settings

## MODIFIED Requirements

### Requirement: Refresh Functionality

The system SHALL provide a single global refresh mechanism with loading feedback.

#### Scenario: Refresh button shows loading state
- **WHEN** refresh is in progress
- **THEN** the refresh button is disabled
- **AND** a progress ring indicator is visible
- **AND** the button re-enables when refresh completes

#### Scenario: Content remains visible during refresh
- **WHEN** refresh is in progress
- **THEN** existing content remains visible
- **AND** content updates in place when new data arrives
- **AND** skeleton placeholders are NOT shown during refresh
