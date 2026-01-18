# Tasks

## 1. Remove trailing separators
- [x] Remove separator after "64-bit OS" in OS card (line 208)
- [x] Remove separator after "Activation Status" (line 244)
- [x] Remove separator after "OS Architecture" (line 436)
- [x] Remove separator after "64-bit Process" (line 444)
- [x] Remove separator after "Running Under Emulation" (line 452) - already had no separator
- [x] Remove separator after "Secure Boot Status" (line 610) - already had no separator
- [x] Remove separator after TPM Firmware Version (line 678) - already had no separator
- [ ] Verify visual appearance in Light and Dark themes

## 2. Add copy button feedback
- [x] Add `CopiedFieldName` property to ViewModel
- [x] Implement 1-second timer to revert icon after copy using DispatcherQueueTimer
- [x] Create `CopyIconConverter` to show checkmark glyph on success
- [x] Update all copy button FontIcons to bind with the converter
- [ ] Test feedback visibility in all themes

## 3. Standardize empty state text
- [x] Audit `SystemInfoService` for fallback values - replaced "Unavailable" with "N/A"
- [x] Audit `HardwareInfoService` for fallback values - replaced "Unavailable" with "N/A"
- [x] Audit `NetworkInfoService` for fallback values - replaced "Unavailable" with "N/A"
- [x] Audit `BiosInfoService` for fallback values - replaced "Unavailable" with "N/A"
- [x] Audit `TpmInfoService` for fallback values - model defaults already updated
- [x] Replace any "Unknown", "Not available", or empty strings with "N/A"
- [x] Update unit tests to use "N/A" instead of "Unavailable"

## 4. Add copy button hover state
- [x] Update `CopyButtonStyle` in MainPage.xaml with custom Template
- [x] Add `PointerOver` visual state with `ControlFillColorSecondaryBrush` background
- [x] Add `Pressed` visual state with `ControlFillColorTertiaryBrush` background
- [ ] Verify hover appearance in Light, Dark, and Cyberpunk themes

## 5. Verification
- [x] Run full test suite - all 136 tests pass
- [ ] Manual visual review in all 5 themes
- [ ] Verify accessibility with screen reader
