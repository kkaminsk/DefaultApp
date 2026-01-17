## ADDED Requirements

### Requirement: GitHub Actions CI/CD

The application SHALL use GitHub Actions for continuous integration and deployment.

#### Scenario: CI runs on push and PR
- **WHEN** code is pushed or a pull request is created
- **THEN** the CI workflow runs automatically
- **AND** build, test, and package steps execute

#### Scenario: CI uses Windows runner
- **WHEN** the workflow runs
- **THEN** it executes on a Windows runner
- **AND** .NET 8 SDK and Windows App SDK are available

### Requirement: Multi-Architecture Builds

The application SHALL build for multiple Windows architectures.

#### Scenario: x64 builds are produced
- **WHEN** building for release
- **THEN** x64 (64-bit Intel/AMD) packages are created

#### Scenario: ARM64 builds are produced
- **WHEN** building for release
- **THEN** ARM64 packages are created

#### Scenario: x86 builds are excluded
- **WHEN** reviewing build configuration
- **THEN** x86 (32-bit) builds are not included

### Requirement: MSIX Package Creation

The application SHALL be packaged as MSIX for distribution.

#### Scenario: MSIX bundle is created
- **WHEN** the build completes
- **THEN** an MSIX bundle containing x64 and ARM64 is created

#### Scenario: Package version follows SemVer
- **WHEN** versioning packages
- **THEN** SemVer format is used (1.0.0, 1.0.1, 1.1.0)

### Requirement: Code Signing

The application SHALL be code signed using Azure Artifact.

#### Scenario: MSIX is signed in pipeline
- **WHEN** MSIX packages are created
- **THEN** they are signed using Azure Artifact
- **AND** certificate from C:\Temp\tsscat\CodeSigning is used

#### Scenario: Signature is verified
- **WHEN** signing completes
- **THEN** the signature is verified
- **AND** packages fail validation if unsigned

### Requirement: Store Submission

The application SHALL support automated Microsoft Store submission.

#### Scenario: Submission is automated
- **WHEN** a release is created
- **THEN** the package is submitted to the Microsoft Store
- **AND** runFullTrust justification is included

### Requirement: Debug Build Features

Debug builds SHALL include additional development tools.

#### Scenario: Verbose logging in debug
- **WHEN** running a debug build
- **THEN** verbose console logging is enabled
- **AND** additional diagnostic information is available

#### Scenario: Debug menu is available
- **WHEN** running a debug build
- **THEN** a debug menu/page is accessible
- **AND** mock data options are available

#### Scenario: Debug features excluded from release
- **WHEN** building for release
- **THEN** debug menu and mock data are not included
- **AND** verbose logging is disabled
