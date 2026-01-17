# Tasks: Add About Window

## Implementation Tasks

- [x] **1. Create AboutWindow.xaml**
  - Create new window with dark themed background
  - Add app icon, name, and version
  - Add release notes section (v1.1 only)
  - Add author/organization/license info
  - Add close button
  - Style to match app theme

- [x] **2. Create AboutWindow.xaml.cs**
  - Configure window (size, centered, no resize)
  - Add close button handler
  - Set version from package manifest

- [x] **3. Add info button to MainWindow.xaml**
  - Add button with info icon (&#xE946;) next to theme selector
  - Style to match title bar aesthetic
  - Add tooltip "About"

- [x] **4. Add click handler in MainWindow.xaml.cs**
  - Create and show AboutWindow on button click
  - Ensure only one About window can be open at a time

- [x] **5. Build and test**
  - Verify button appears correctly
  - Verify About window opens and displays correct content
  - Verify close button works
  - Test with different themes

## Dependencies

- None (standalone feature)

## Validation

- Info button visible in title bar
- Clicking button opens About window
- About window shows version 1.1 content only
- Close button dismisses window
- Window is centered and appropriately sized
