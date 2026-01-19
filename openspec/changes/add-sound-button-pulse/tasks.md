# Tasks: Add Sound Button Pulse Animation

## Implementation Tasks

- [x] Add `IsPlayingAudio` observable property to `MainViewModel`
- [x] Subscribe to `MediaPlayer.MediaEnded` event in ViewModel constructor
- [x] Update `PlayAudio()` method to set `IsPlayingAudio = true` before playing
- [x] Create event handler to set `IsPlayingAudio = false` when playback ends
- [x] Handle audio load failure to reset `IsPlayingAudio` if file not found
- [x] Add pulse animation using Composition API in `MainPage.xaml.cs`
- [x] Update Test Audio Button to use x:Name for animation targeting
- [x] Subscribe to ViewModel.PropertyChanged to start/stop animation based on `IsPlayingAudio`
- [x] Ensure animation respects reduced motion accessibility setting
- [x] Dispose MediaPlayer event subscription in ViewModel disposal

## Validation Tasks

- [x] Verify pulse animation starts when button is clicked
- [x] Verify pulse animation stops when audio playback completes
- [x] Verify animation works across all themes (uses opacity, theme-independent)
- [x] Verify no animation if audio file is missing (IsPlayingAudio not set)
- [x] Test rapid button clicks during playback (returns early if already playing)
- [x] Run existing unit tests to ensure no regressions (136 tests pass)
