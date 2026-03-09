# Proposal: Add Pulsating Background to Sound Button During Playback

## Summary

Add visual feedback to the "Test Audio" button by pulsating its background color while the audio file is playing. This provides clear indication that audio playback is in progress.

## Motivation

Currently, when users click the "Test Audio" button, there is no visual indication that audio is playing. Users may:
- Not realize audio started (especially if volume is low or muted)
- Click the button multiple times thinking it didn't work
- Not know when playback has completed

A pulsating background effect provides immediate, continuous feedback during playback.

## Approach

1. **Track playback state** - Subscribe to `MediaPlayer.MediaEnded` event to know when playback completes
2. **Add observable property** - `IsPlayingAudio` boolean to indicate playback state
3. **Create XAML animation** - Use a `Storyboard` with `ColorAnimation` to pulse the button background
4. **Trigger animation via VisualState** - Bind animation to `IsPlayingAudio` property

### Animation Design

- **Effect**: Smooth pulse between normal background and an accent color
- **Duration**: ~800ms per pulse cycle (400ms fade in, 400ms fade out)
- **Behavior**: Repeats continuously while audio plays, stops immediately when audio ends

## Scope

- **In scope**: Button background animation, playback state tracking
- **Out of scope**: Sound wave visualizations, progress indicators, volume meters

## Risks

| Risk | Mitigation |
|------|------------|
| Animation may be distracting | Use subtle color shift, not dramatic change |
| Accessibility concerns | Ensure sufficient contrast; animation is supplementary, not sole indicator |
| MediaPlayer event timing | Handle edge cases where audio fails to load |
