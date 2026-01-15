## 1. Theme Service

- [ ] 1.1 Create ThemeService class
- [ ] 1.2 Implement GetCurrentTheme() method
- [ ] 1.3 Implement SetTheme(theme) method
- [ ] 1.4 Implement theme persistence to ApplicationData.Current.LocalSettings
- [ ] 1.5 Add event for theme changed notifications
- [ ] 1.6 Implement system theme detection (follow system default)

## 2. Light Theme

- [ ] 2.1 Configure default Light theme using WinUI 3 defaults
- [ ] 2.2 Verify card backgrounds, text colors, and borders
- [ ] 2.3 Ensure status indicator colors are visible

## 3. Dark Theme

- [ ] 3.1 Configure default Dark theme using WinUI 3 defaults
- [ ] 3.2 Verify card backgrounds, text colors, and borders
- [ ] 3.3 Ensure status indicator colors are visible

## 4. Cyberpunk Theme

- [ ] 4.1 Create Themes/Cyberpunk.xaml resource dictionary
- [ ] 4.2 Define primary color (neon pink: #FF00FF or cyan: #00FFFF)
- [ ] 4.3 Define background color (dark purple: #1A0A2E or black: #0D0D0D)
- [ ] 4.4 Define accent color (neon green: #00FF00 or yellow: #FFFF00)
- [ ] 4.5 Style cards with optional glow effects
- [ ] 4.6 Maintain standard font (no stylized fonts)
- [ ] 4.7 Ensure text contrast meets accessibility requirements

## 5. High Contrast Dark Theme

- [ ] 5.1 Create Themes/HighContrastDark.xaml resource dictionary
- [ ] 5.2 Use Windows system high contrast colors
- [ ] 5.3 Ensure all text has maximum contrast
- [ ] 5.4 Remove decorative elements that reduce clarity

## 6. High Contrast Light Theme

- [ ] 6.1 Create Themes/HighContrastLight.xaml resource dictionary
- [ ] 6.2 Use Windows system high contrast colors
- [ ] 6.3 Ensure all text has maximum contrast
- [ ] 6.4 Remove decorative elements that reduce clarity

## 7. Theme Switching

- [ ] 7.1 Implement real-time theme application without restart
- [ ] 7.2 Update App.xaml to support dynamic resource loading
- [ ] 7.3 Wire theme selector ComboBox to ThemeService
- [ ] 7.4 Add "System Default" option to follow OS theme

## 8. Windows High Contrast Integration

- [ ] 8.1 Detect when Windows high contrast mode is enabled
- [ ] 8.2 Automatically switch to appropriate high contrast theme
- [ ] 8.3 Subscribe to system theme change events

## 9. Validation

- [ ] 9.1 Verify each theme applies correctly
- [ ] 9.2 Verify theme persists across app restarts
- [ ] 9.3 Verify system default follows OS changes
- [ ] 9.4 Test high contrast mode detection
- [ ] 9.5 Verify Cyberpunk theme meets contrast requirements
