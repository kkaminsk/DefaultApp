## ADDED Requirements

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
