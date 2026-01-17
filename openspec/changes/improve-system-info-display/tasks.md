## 1. Update SystemInfoService for Friendly OS Name

- [x] 1.1 Add `GetFriendlyOsName()` method that returns "Windows 11" or "Windows 10" based on build number
- [x] 1.2 Add `GetFullOsDisplayName()` method that combines friendly name with Edition (e.g., "Windows 11 Enterprise")
- [x] 1.3 Update `GetOsInfo()` to use the new friendly name method
- [ ] 1.4 Add unit tests for OS name generation with various build numbers

## 2. Update ActivationService for Registry-based Detection

- [x] 2.1 Add Registry-based activation status check method
- [x] 2.2 Read `LicenseStatus` from `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform`
- [x] 2.3 Map Registry values to ActivationStatus enum (1=Activated, 0=NotActivated, etc.)
- [x] 2.4 Keep P/Invoke as fallback if Registry read fails
- [ ] 2.5 Add unit tests for activation status mapping

## 3. Reorganize UI Layout

- [x] 3.1 Move Machine Name field from Hardware card to OS card (after OS Name)
- [x] 3.2 Update MainPage.xaml field ordering in Operating System card
- [x] 3.3 Remove Machine Name from Device & Hardware card

## 4. Add Copy to Clipboard Functionality

- [x] 4.1 Create reusable CopyButton style/template with copy icon (&#xE8C8;)
- [x] 4.2 Add copy button beside Machine Name field
- [x] 4.3 Add copy button beside Version field
- [x] 4.4 Add `CopyToClipboardCommand` in MainViewModel with parameter for field name
- [x] 4.5 Implement clipboard copy using `Windows.ApplicationModel.DataTransfer.Clipboard`
- [x] 4.6 Add tooltip "Copy to clipboard" and brief visual feedback on copy

## 5. Update Resources

- [x] 5.1 Add localized strings for copy button tooltips
- [x] 5.2 Add accessibility labels for copy buttons

## 6. Validation

- [x] 6.1 Build solution successfully
- [x] 6.2 Run unit tests
- [ ] 6.3 Verify friendly OS name displays correctly
- [ ] 6.4 Verify activation status works in MSIX context
- [ ] 6.5 Verify Machine Name appears in OS card
- [ ] 6.6 Verify copy buttons work and show feedback
- [ ] 6.7 Test with screen reader for accessibility
