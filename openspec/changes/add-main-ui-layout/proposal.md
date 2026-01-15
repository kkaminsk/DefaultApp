## Why

The application needs a main user interface with three information cards (Operating System, Device & Hardware, Packaged Service) arranged in a responsive layout. This establishes the visual structure and data binding for displaying system information.

## What Changes

- Create MainPage.xaml with three-card layout
- Create MainViewModel with MVVM bindings
- Implement responsive card stacking for narrow windows
- Add extended title bar with theme selector and refresh button
- Configure keyboard navigation order
- Add accessibility automation properties

## Impact

- Affected specs: `main-ui` (new capability)
- Affected code: Views/MainPage.xaml, ViewModels/MainViewModel.cs, MainWindow.xaml
- Dependencies: Requires `add-project-foundation` and `add-system-info-services`
- Related: Will be extended by `add-theme-system` and service proposals
