## 1. Data Models

- [ ] 1.1 Create OsInfo model with all OS properties
- [ ] 1.2 Create ArchitectureInfo model with hardware properties
- [ ] 1.3 Create ActivationStatus enum (Activated, NotActivated, GracePeriod, NotificationMode)

## 2. SystemInfoService

- [ ] 2.1 Create SystemInfoService class
- [ ] 2.2 Implement GetOsName() using Environment.OSVersion.Platform
- [ ] 2.3 Implement GetOsVersion() using Environment.OSVersion.Version
- [ ] 2.4 Implement GetBuildNumber() using Environment.OSVersion.Version.Build
- [ ] 2.5 Implement GetEdition() using Registry HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\EditionID
- [ ] 2.6 Implement GetIs64BitOs() using Environment.Is64BitOperatingSystem
- [ ] 2.7 Implement GetSystemLocale() using CultureInfo.CurrentCulture
- [ ] 2.8 Add error handling to return "Unavailable" on failures

## 3. ActivationService

- [ ] 3.1 Create ActivationService class
- [ ] 3.2 Implement SLIsGenuineLocal P/Invoke declaration
- [ ] 3.3 Implement GetActivationStatusAsync() running on background thread
- [ ] 3.4 Implement result caching after first successful call
- [ ] 3.5 Map HRESULT codes to ActivationStatus enum values
- [ ] 3.6 Return "Checking..." placeholder during async operation

## 4. HardwareInfoService

- [ ] 4.1 Create HardwareInfoService class
- [ ] 4.2 Implement GetProcessorArchitecture() using RuntimeInformation.ProcessArchitecture
- [ ] 4.3 Implement GetOsArchitecture() using RuntimeInformation.OSArchitecture
- [ ] 4.4 Implement GetMachineName() using Environment.MachineName
- [ ] 4.5 Implement GetProcessorCount() using Environment.ProcessorCount
- [ ] 4.6 Implement GetCpuModels() using Registry HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor\*
- [ ] 4.7 Implement GetTotalRam() using Windows.System.MemoryManager with MEMORYSTATUSEX fallback
- [ ] 4.8 Implement GetIs64BitProcess() using Environment.Is64BitProcess
- [ ] 4.9 Implement GetIsRunningUnderEmulation() comparing architectures
- [ ] 4.10 Format RAM as GB with 1 decimal place

## 5. P/Invoke Declarations

- [ ] 5.1 Create NativeMethods class for P/Invoke declarations
- [ ] 5.2 Add SLIsGenuineLocal declaration from slc.dll
- [ ] 5.3 Add MEMORYSTATUSEX structure definition
- [ ] 5.4 Add GlobalMemoryStatusEx declaration from kernel32.dll

## 6. Validation

- [ ] 6.1 Write unit tests for SystemInfoService
- [ ] 6.2 Write unit tests for HardwareInfoService
- [ ] 6.3 Write unit tests for ActivationService (with mocked P/Invoke)
- [ ] 6.4 Verify graceful degradation when Registry access fails
- [ ] 6.5 Verify multi-processor display returns list format
