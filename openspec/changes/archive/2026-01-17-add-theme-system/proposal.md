## Why

The application requires five themes (Light, Dark, Cyberpunk, High Contrast Dark, High Contrast Light) with real-time switching support. Users should be able to select themes manually or follow system settings.

## What Changes

- Create theme XAML resource dictionaries for all five themes
- Implement ThemeService for theme management
- Add theme persistence to LocalSettings
- Implement real-time theme switching without restart
- Integrate High Contrast themes with Windows system settings
- Design Cyberpunk theme with neon aesthetic

## Impact

- Affected specs: `themes` (new capability)
- Affected code: Themes/ folder, App.xaml, ThemeService.cs
- Dependencies: Requires `add-project-foundation` and `add-main-ui-layout`
- Related: Theme selector in title bar from `add-main-ui-layout`
