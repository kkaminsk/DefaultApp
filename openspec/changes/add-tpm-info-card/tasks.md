# Tasks: Add TPM Information Card

## Implementation Tasks

- [x] **1. Create TpmInfo model**
  - Create `Models/TpmInfo.cs` with TPM properties

- [x] **2. Create TpmInfoService**
  - Create `Services/TpmInfoService.cs`
  - Read TPM data from Registry
  - Handle missing TPM gracefully

- [x] **3. Add TPM properties to MainViewModel**
  - Add observable properties for each TPM field
  - Add TpmInfoService instantiation
  - Load TPM data in LoadDataAsync
  - Update CopyToClipboard handler

- [x] **4. Add TPM Information card to MainPage.xaml**
  - Add new card at bottom of page
  - Include all four TPM properties with copy buttons
  - Follow existing card styling

- [x] **5. Build and test**
  - Verify TPM card appears at bottom
  - Verify all fields display correctly
  - Verify copy buttons work
  - Test on system without TPM (graceful fallback)

## Dependencies

- None (standalone feature)

## Validation

- TPM Information card visible at bottom of page
- All four TPM fields displayed with values or "Unavailable"
- Copy buttons functional for each field
- No crashes on systems without TPM
