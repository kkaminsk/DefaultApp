# Clarifications Document 3: Default App

This document identifies remaining implementation details and edge cases requiring clarification before development begins.

---

## 1. Background Task Implementation

### 1.1 Timer Execution Model

**Question:** The spec states "Timer that ticks every second and updates the UI with the current time." However, standard `TimeTrigger` background tasks have a minimum 15-minute interval. How should this be implemented?

| Option | Description |
|--------|-------------|
| A) In-process background task | Use `IBackgroundTask` with `ApplicationTrigger`, run timer loop in-process |


**Recommendation needed:** Clarify whether the "Background Task" demo should be a true out-of-process background task or an in-process timer demonstration.

### 1.2 Background Task Registration

**Question:** When should the background task be registered?

- B) Every app launch (re-register)


---

## 2. App Service Details

### 2.1 Event Log Scope

**Question:** Which Windows Event Logs should the App Service read?

| Log | Include? |
|-----|----------|
| Application | ? |

**Additional Questions:**
- How many recent events should be retrieved per query (10, 50, 100)? 10
- Should events be filtered by level (Error, Warning, Information)? warning and error
- What timeframe for "recent" events (last hour, last 24 hours, since app launch)? last 24 hours

### 2.2 Event Log Display Format

**Question:** What information should be displayed for each event?

| Field | Include? |
|-------|----------|
| Event ID | ? |
| Source | ? |
| Level (Error/Warning/Info) | ? |
| Timestamp | ? |
| Message (truncated) | ? |

---

## 3. Full Trust Process Details

### 3.1 Named Pipe Configuration

**Question:** What naming convention should be used for the Named Pipe?

- B) Instance-specific: `\\.\pipe\DefaultApp_{ProcessId}`


### 3.2 IPC Protocol Format

**Question:** What format should be used for messages over Named Pipes?

| Option | Pros | Cons |
|--------|------|------|
| JSON | Human-readable, easy debugging | Parsing overhead |


### 3.3 Reconnection Strategy

**Question:** If the Named Pipe connection is lost:

- Should the main app automatically attempt reconnection? Yes
- How many retry attempts before showing error? 5
- What interval between retries (1s, 5s, exponential backoff)? 5s

### 3.4 Event Log Writing

**Question:** What should the Full Trust Process log to the Windows Event Log?


- B) Only errors and warnings - by default
- C) Configurable log level

**Additional Question:** Should a custom Event Source be registered, or use an existing one?

---

## 4. Service UI Behavior

### 4.1 Default Service Type

**Question:** Which service type should be selected by default on app launch?

- A) Background Task (first in list)


### 4.2 Uptime Display

**Question:** The UI mockup shows "Uptime: 00:05:32". Clarify:

- Format always `HH:MM:SS` or shorter formats for under 1 hour (`5:32`)?
- What displays when service is stopped? ("--:--:--", "N/A", hidden) N/A
- Should uptime persist across app restarts if service persists? (N/A since services stop on close) N/A

### 4.3 Service-Specific UI Content

**Question:** Each service type shows different data. What should be displayed in the "Packaged Service" card for each?

| Service Type | Content to Display |
|--------------|-------------------|
| Background Task | Current time from timer |
| App Service | Event log summary/count |
| Full Trust Process | Log entries written and connection status |

---

## 5. Theme Implementation

### 5.1 Cyberpunk Theme Specifics

**Question:** What visual characteristics should the Cyberpunk theme have?

| Element | Specification Needed |
|---------|---------------------|
| Primary color | (e.g., neon pink, cyan) |
| Background color | (e.g., dark purple, black) |
| Accent color | (e.g., neon green, yellow) |
| Font style | Standard or stylized? |
| Glow effects | Should cards/buttons have glow? |

### 5.2 High Contrast Themes

**Question:** Should the High Contrast themes:

- A) Use Windows system high contrast colors
- B) Use custom fixed high contrast palette
- C) Adapt to user's Windows high contrast settings

### 5.3 Theme Selector Location

**Question:** The UI mockup shows theme selector in the title bar area. Should it be:

- A) In the title bar yes
- B) In a settings flyout/page no
- C) In the app's command bar no

---

## 6. Window and Title Bar

### 6.1 Title Bar Style

**Question:** Should the app use:

- A) Standard Windows title bar
- B) Extended view into title bar (modern WinUI pattern)
- C) Fully custom title bar

### 6.2 Window State Persistence

**Question:** Should window size and position be remembered between sessions?

---

## 7. P/Invoke Implementation

### 7.1 SLIsGenuineLocal Threading

**Question:** The `SLIsGenuineLocal` call can take several hundred milliseconds. Should it:

- A) Run on background thread, show "Checking..." initially yes
- B) Run synchronously on startup no
- C) Be cached after first successful call yes

### 7.2 Memory Information API Choice

**Question:** The spec lists two options for Total RAM:
- `Windows.System.MemoryManager`
- `MEMORYSTATUSEX` via P/Invoke - fallback

Which should be used? Is there a preferred fallback order?

---

## 8. CPU Information

### 8.1 Registry vs Alternative Sources

**Question:** The spec uses Registry for CPU model. Should the implementation:

- A) Use Registry only (`HKLM\HARDWARE\DESCRIPTION\System\CentralProcessor\*`)
- B) Have fallback to `Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")`
- C) Display "Unknown" if Registry fails

### 8.2 Multi-Processor Display Format

**Question:** For systems with multiple physical CPUs, the spec says "display all processors in a list." Example format:

```
CPU Model:
  • Intel Xeon E5-2680 v4
  • Intel Xeon E5-2680 v4
```

Or consolidated:

```
CPU Model: Intel Xeon E5-2680 v4 (×2)
```

Which format is preferred?

---

## 9. Real-time Updates

### 9.1 Polling Frequency

**Question:** The spec states 500ms polling for service status. Should this:

- A) Remain constant at 500ms
- B) Reduce to 2-5 seconds when app is in background/minimized - yes
- C) Use event-driven updates instead of polling where possible - no

### 9.2 Timer Service Update

**Question:** The Background Task timer updates UI every second. Mechanism:

- A) DispatcherTimer in main app triggered by background task yes
- B) Background task sends message to main app no
- C) Main app polls background task state no

---

## 10. Error Handling Details

### 10.1 Service Failure Dialog

**Question:** The spec says "prompt user to restart app" on service failure. Dialog specifics:

- Title text?
- Message format (include error details)?
- Buttons: "Restart Now" / "Close" or just "OK"?
- Should "Restart Now" actually restart the app programmatically?

### 10.2 Graceful Degradation

**Question:** If individual system info items fail to retrieve, should:

- A) Show "Unavailable" for that specific item yes
- B) Show "Error" with tooltip explaining why no
- C) Hide the failed item entirely no

---

## 11. Build Configuration

### 11.1 Debug vs Release Behavior

**Question:** Should debug builds include:

- A) Verbose console logging yes
- B) Debug menu/page for testing yes
- C) Mock data options for service testing yes

### 11.2 Architecture-Specific Builds

**Question:** The spec mentions x64 and ARM64 builds. Should there also be:

- A) x86 (32-bit) build no
- B) AnyCPU build yes

---

## 12. Accessibility Specifics

### 12.1 AutomationProperties

**Question:** Should AutomationProperties be set on:

- A) All interactive controls only yes
- B) All controls including static text no
- C) Cards/groups with automation peers yes

### 12.2 Keyboard Navigation Order

**Question:** What should the tab order be?

1. Theme selector → Refresh button → Service type dropdown → Start/Stop button → ?  yes
2. Or different order? no

---

## Summary of Required Decisions

| # | Category | Question |
|---|----------|----------|
| 1 | Background Task | In-process or out-of-process timer implementation? |
| 2 | App Service | Which event logs to read and how many events? |
| 3 | Full Trust Process | Named pipe naming convention? |
| 4 | Full Trust Process | IPC protocol format (JSON, binary)? |
| 5 | Service UI | Default service type on launch? |
| 6 | Service UI | Content displayed per service type? |
| 7 | Cyberpunk Theme | Color palette specification? |
| 8 | High Contrast | Use system colors or custom? |
| 9 | Title Bar | Standard or extended/custom? |
| 10 | Multi-CPU | List format or consolidated with count? |
| 11 | Polling | Constant 500ms or adaptive? |
| 12 | Error Dialog | Button options and auto-restart capability? |
| 13 | Builds | Include x86/AnyCPU? |
| 14 | Accessibility | Tab navigation order? |

---

## Next Steps

Please provide decisions for the questions above. These implementation details will ensure consistent development and avoid rework during implementation.

---

## Resolution Status

**Status: RESOLVED**

All decisions from this document have been applied to `APPLICATION_SPECIFICATION.md`. The following key decisions were incorporated:

1. **Background Task**: In-process with `ApplicationTrigger`, re-registers every launch
2. **App Service**: Application log only, 10 events, Warning/Error, last 24 hours
3. **Named Pipe**: Instance-specific naming with JSON protocol
4. **IPC Reconnection**: Auto-reconnect with 5 retries at 5-second intervals
5. **Default Service**: Background Task on app startup
6. **Theme Selector**: Located in title bar
7. **High Contrast**: Uses Windows system colors
8. **Activation API**: Background thread with caching
9. **Memory API**: MemoryManager primary, MEMORYSTATUSEX fallback
10. **Polling**: Adaptive (500ms foreground, 2-5s background)
11. **Graceful Degradation**: Show "Unavailable" for failed items
12. **Debug Builds**: Verbose logging, debug menu, mock data
13. **Architectures**: x64, ARM64, AnyCPU (no x86)
14. **Accessibility**: AutomationProperties on interactive controls and card groups
