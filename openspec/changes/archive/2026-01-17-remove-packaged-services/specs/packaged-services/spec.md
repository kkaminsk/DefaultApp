# Packaged Services Specification

## REMOVED Requirements

### REQ-SVC-001: Background Task Service
**Priority:** N/A (Removed)

The application no longer provides a background task service demonstration.

#### Scenario: Background task removed from UI
- **Given** the application is running
- **When** the user views the main page
- **Then** there is no service type selector or background task option

---

### REQ-SVC-002: App Service
**Priority:** N/A (Removed)

The application no longer provides an app service demonstration for event log reading.

#### Scenario: App service removed from UI
- **Given** the application is running
- **When** the user views the main page
- **Then** there is no app service option or event log display

---

### REQ-SVC-003: Full Trust Process
**Priority:** N/A (Removed)

The application no longer provides a full trust process demonstration with named pipes IPC.

#### Scenario: Full trust process removed from UI
- **Given** the application is running
- **When** the user views the main page
- **Then** there is no full trust process option

---

### REQ-SVC-004: Service Controller
**Priority:** N/A (Removed)

The ServiceController class is removed as there are no services to manage.

#### Scenario: No service lifecycle management
- **Given** the application is running
- **When** the application starts
- **Then** no service controller is initialized

---

### REQ-SVC-005: Service Card UI
**Priority:** N/A (Removed)

The packaged service card is removed from the main page.

#### Scenario: Two-card layout
- **Given** the application is running
- **When** the user views the main page
- **Then** only OS Information and Architecture Information cards are displayed
- **And** the layout adjusts responsively for 2 cards instead of 3

---

## MODIFIED Requirements

### REQ-UI-001: Main Page Layout
**Priority:** High

The main page displays system information in a responsive two-card layout.

#### Scenario: Responsive two-card layout
- **Given** the application window width is less than 1200 pixels
- **When** the user views the main page
- **Then** the two cards (OS Info, Architecture Info) stack vertically

#### Scenario: Wide two-card layout
- **Given** the application window width is 1200 pixels or more
- **When** the user views the main page
- **Then** the two cards display side by side

---

### REQ-PKG-001: MSIX Package Capabilities
**Priority:** Medium

The MSIX package no longer requires the fullTrustProcess extension.

#### Scenario: Simplified package manifest
- **Given** the package manifest is configured
- **When** the application is packaged
- **Then** no fullTrustProcess extension is declared
- **And** the runFullTrust capability is only present if required by WinUI 3

---

### REQ-TEST-001: Test Coverage
**Priority:** Medium

Unit tests cover the remaining functionality without service-related tests.

#### Scenario: Test suite runs without service tests
- **Given** the test project is configured
- **When** tests are executed
- **Then** all tests pass
- **And** no service-related test files exist
