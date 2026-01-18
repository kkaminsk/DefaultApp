# Proposal: Add Sonar Sound for Successful Pings

## Summary

Play a sonar sound effect (`sonar.mp3`) for each successful ping response during ping tests. This provides audio feedback that reinforces the visual progress indicator.

## Motivation

Currently, ping operations only provide visual feedback (button text showing "X/5" and color changes). Adding an audible sonar "ping" sound for each successful response:
- Provides immediate audio confirmation of network connectivity
- Creates a satisfying feedback loop (like submarine sonar)
- Helps users monitor tests without constantly watching the screen
- Reinforces the "ping" metaphor with actual ping sounds

## Approach

1. **Add sonar.mp3 to Assets** - Copy `Audio/sonar.mp3` to `Assets/Audio/sonar.mp3` and include in project
2. **Create dedicated MediaPlayer** - Use a separate MediaPlayer instance for ping sounds to avoid conflicts with the Test Audio button
3. **Play on each success** - In `ExecutePingToAddressAsync`, play the sonar sound each time `success == true`
4. **Keep sounds short and non-blocking** - The sound plays independently; ping loop continues immediately

### Implementation Location

The sound will be played in the shared `ExecutePingToAddressAsync` method, so it automatically works for:
- Gateway ping
- DNS server ping
- Google DNS ping

## Scope

- **In scope**: Playing sonar sound on successful ping responses
- **Out of scope**: Different sounds for failures, volume controls, mute option

## Risks

| Risk | Mitigation |
|------|------------|
| Rapid sounds may be annoying | Sonar sound is short (~0.5s); 1-second delay between pings provides spacing |
| Sound overlap if previous still playing | MediaPlayer handles this by restarting; sonar is short enough this won't be noticeable |
| Missing audio file | Log warning and continue silently (same pattern as Test Audio button) |
