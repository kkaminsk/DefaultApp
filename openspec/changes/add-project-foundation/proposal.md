## Why

The Default App requires a proper solution structure with multiple projects before any functionality can be implemented. This establishes the foundation for the WinUI 3 application, background task, app service, full trust process, and test projects.

## What Changes

- Create Visual Studio solution with 5 projects
- Configure Single-Project MSIX packaging for main app
- Set up project references and NuGet dependencies
- Configure Package.appxmanifest with required capabilities
- Create basic App.xaml and MainWindow.xaml shell
- Set up en-US resource files for localization readiness

## Impact

- Affected specs: `project-structure` (new capability)
- Affected code: All project files (.csproj, .sln), Package.appxmanifest
- Dependencies: None (this is the foundation)
- Blocking: All other proposals depend on this
