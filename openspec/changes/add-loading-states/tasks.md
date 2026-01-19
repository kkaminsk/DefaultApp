# Tasks

## 1. Create skeleton card templates
- [x] Design skeleton placeholder layout matching card structure
- [x] Create placeholder rectangles for header and property rows
- [x] Apply theme-appropriate placeholder colors
- [x] Test skeleton appearance in all themes

## 2. Add loading state to ViewModel
- [x] Add `IsLoading` property to MainViewModel
- [x] Set `IsLoading = true` before data fetch begins
- [x] Set `IsLoading = false` after all data is loaded
- [x] Expose property for XAML binding

## 3. Implement skeleton display in MainPage
- [x] Add visibility toggle between skeleton and real content
- [x] Bind visibility to `IsLoading` property
- [x] Ensure smooth transition when loading completes

## 4. Add shimmer effect (optional)
- [x] Create opacity pulsing animation for skeleton placeholders
- [x] Apply subtle pulse effect using Composition API
- [x] Ensure animation doesn't cause performance issues
- [x] Make shimmer respect reduced motion accessibility setting

## 5. Refresh loading indicator
- [x] Add refresh button to title bar
- [x] Disable button during refresh operation
- [x] Show ProgressRing inside button during refresh
- [x] Re-enable button when refresh completes

## 6. Testing and verification
- [x] Build succeeds with no warnings
- [x] Test skeleton appearance in all themes (uses theme resources)
- [x] Verify accessibility with screen reader (automation properties present)
- [x] Test with Windows reduced motion setting enabled (checked in code)
- [x] Run full test suite (136 tests pass)
