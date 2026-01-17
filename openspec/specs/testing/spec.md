# testing Specification

## Purpose
TBD - created by archiving change 4-add-unit-tests. Update Purpose after archive.
## Requirements
### Requirement: Service Layer Unit Tests

All service classes MUST have unit tests covering their public methods.

#### Scenario: SystemInfoService tests
- **WHEN** SystemInfoService tests are executed
- **THEN** all public methods SHALL be covered
- **AND** edge cases for unavailable data SHALL be tested

#### Scenario: HardwareInfoService tests
- **WHEN** HardwareInfoService tests are executed
- **THEN** all public methods SHALL be covered
- **AND** formatting logic (RAM, CPU models) SHALL be validated

#### Scenario: ActivationService tests
- **WHEN** ActivationService tests are executed
- **THEN** display string mapping SHALL be verified for all enum values
- **AND** caching behavior SHALL be tested

#### Scenario: NetworkInfoService tests
- **WHEN** NetworkInfoService tests are executed
- **THEN** ping with invalid addresses SHALL return false gracefully
- **AND** network info retrieval SHALL handle missing adapters

#### Scenario: BiosInfoService tests
- **WHEN** BiosInfoService tests are executed
- **THEN** BIOS field retrieval SHALL be tested
- **AND** Secure Boot status detection SHALL be validated

#### Scenario: TpmInfoService tests
- **WHEN** TpmInfoService tests are executed
- **THEN** TPM info retrieval SHALL be tested
- **AND** manufacturer ID conversion SHALL be validated

#### Scenario: ThemeService tests
- **WHEN** ThemeService tests are executed
- **THEN** ComboBox index mapping SHALL be verified
- **AND** round-trip conversion SHALL maintain consistency

### Requirement: ViewModel Unit Tests

ViewModel classes MUST have unit tests for key business logic.

#### Scenario: MainViewModel tests
- **WHEN** MainViewModel tests are executed
- **THEN** CPU model formatting SHALL be verified
- **AND** clipboard command logic SHALL be tested
- **AND** refresh command state SHALL be validated

### Requirement: Code Coverage Target

The service layer MUST achieve at least 80% code coverage.

#### Scenario: Coverage report generation
- **WHEN** tests are run with coverage enabled
- **THEN** a coverage report SHALL be generated
- **AND** service layer coverage SHALL be at least 80%

#### Scenario: Coverage regression
- **WHEN** new code is added to services
- **THEN** corresponding tests SHALL be added
- **AND** coverage SHALL NOT drop below 80%

### Requirement: Unit Test Coverage

The application SHALL have unit tests covering the service layer with 80% minimum coverage.

#### Scenario: Service layer has 80% coverage
- **WHEN** running test coverage analysis
- **THEN** Services/ folder has at least 80% code coverage

#### Scenario: All services have test classes
- **WHEN** reviewing test project
- **THEN** SystemInfoService, HardwareInfoService, ActivationService, and ServiceController have corresponding test classes

### Requirement: Unit Test Framework

The application SHALL use xUnit for unit testing.

#### Scenario: Tests run with xUnit
- **WHEN** executing dotnet test
- **THEN** all tests run using xUnit framework
- **AND** test results are reported

### Requirement: ViewModel Testing

MainViewModel SHALL have unit tests for its functionality.

#### Scenario: Data loading is tested
- **WHEN** LoadDataAsync() is called
- **THEN** all observable properties are populated
- **AND** IsRefreshing state transitions correctly

#### Scenario: Commands are tested
- **WHEN** RefreshCommand is executed
- **THEN** data is reloaded
- **AND** appropriate state changes occur

### Requirement: UI Automation Testing

The application SHALL have UI automation tests using Microsoft.Windows.Apps.Test.

#### Scenario: Main window can be tested
- **WHEN** running UI automation tests
- **THEN** the main window launches successfully
- **AND** controls can be located and interacted with

#### Scenario: Key user flows are tested
- **WHEN** running UI tests
- **THEN** theme selection, refresh, and service selection are tested

### Requirement: Accessibility Testing

The application SHALL have automated accessibility tests in the CI pipeline.

#### Scenario: Automation properties are validated
- **WHEN** running accessibility tests
- **THEN** all interactive controls have AutomationProperties.Name set

#### Scenario: Keyboard navigation is validated
- **WHEN** running accessibility tests
- **THEN** all controls are reachable via Tab key
- **AND** navigation order follows specification

### Requirement: Graceful Degradation Testing

Tests SHALL verify graceful degradation behavior.

#### Scenario: Registry failure handling is tested
- **WHEN** Registry access is simulated to fail
- **THEN** "Unavailable" is returned
- **AND** no exception propagates

#### Scenario: P/Invoke failure handling is tested
- **WHEN** P/Invoke is simulated to fail
- **THEN** "Unavailable" is returned
- **AND** application continues functioning

