## Why

The application needs to retrieve and display operating system and hardware information. This requires implementing services that use Environment APIs, Registry access, and P/Invoke for activation status and memory information.

## What Changes

- Create SystemInfoService for OS information retrieval
- Create HardwareInfoService for architecture and hardware details
- Create ActivationService with SLIsGenuineLocal P/Invoke
- Create data models for OsInfo and ArchitectureInfo
- Implement graceful degradation for failed retrievals

## Impact

- Affected specs: `system-info` (new capability)
- Affected code: Services/, Models/ folders
- Dependencies: Requires `add-project-foundation` to be complete
- Related: Will be consumed by `add-main-ui-layout`
