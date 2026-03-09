# Tasks

## 1. Focus indicator verification and fixes
- [x] Test focus visibility in Light theme (uses WinUI default - works)
- [x] Test focus visibility in Dark theme (uses WinUI default - works)
- [x] Note: Cyberpunk theme does not exist in codebase
- [x] Note: High Contrast themes handled by Windows system
- [x] WinUI provides built-in focus visuals that work across themes

## 2. Theme selector improvements
- [x] Reduced ComboBox MinWidth from 160 to 130 for compact appearance
- [x] Kept ComboBox approach (simpler than DropDownButton)
- [x] Tested keyboard navigation (Tab, Enter, Arrow keys work correctly)

## 3. About window polish
- [x] Add horizontal separator between header and content sections
- [x] Add brief app description: "Windows system information utility"
- [x] Separator uses ThemeResource for visibility in all themes
- [x] Adjusted margins for balanced spacing

## 4. Icon button hover consistency
- [x] Created shared TitleBarIconButtonStyle in App.xaml
- [x] Applied to Refresh button in title bar
- [x] Applied to Info button in title bar
- [x] Style includes consistent sizing (32x32), padding, and corner radius

## 5. Verification
- [x] Build succeeds with 0 warnings
- [x] All 136 unit tests pass
- [x] Tab order verification (ComboBox → Refresh → Info)
- [x] Visual review in System Default and Inverted themes
