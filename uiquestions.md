# UI Enhancement Questions

Questions to refine enhancements into OpenSpec proposals.

---

## 1. Visual Consistency

### Trailing Separators
- Should we remove trailing separators everywhere, or only before action buttons (like "Test Audio")? Before action buttons is preferred.
- Do you want separators between cards as well, or just internal cleanup? yes

### Card Spacing
- Is the current 16px card padding acceptable, or should we adjust? this is ok
- Do you prefer consistent 12px row spacing, or keep the current variable margins? yes

---

## 2. Copy Button Feedback

### Feedback Style
Which copy confirmation style do you prefer?
- A) Tooltip changes to "Copied!" for 1 second no
- B) Icon briefly changes to a checkmark yes
- C) Subtle background flash/pulse no


### Hover State
- Should copy buttons show a visible background on hover? yes
- If yes, should this apply to all icon-only buttons (info button, etc.)? yes

---

## 3. Ping Buttons

### Current Behavior
The ping buttons currently change their text to show results (e.g., "12ms" or "Failed").

Preferred approach?
- A) Keep current behavior (text changes to result) yes
- B) Fixed "Ping" label with colored status indicator (green/red dot) no
- C) Fixed "Ping" label with result shown as tooltip no
- D) Add a separate status text next to the button no

### Button Width
- Is the button width jumping acceptable, or should we use `MinWidth` to stabilize? yes

---

## 4. Property Grouping

### Subsection Headers
- Do you want visual subgroups within cards (e.g., "Processor" and "Memory" sections in Hardware card)?
- If yes, should these be:
  - A) Bold text labels with extra spacing yes
  - B) Subtle divider with label yes
  - C) Collapsible sections (expanders) yes

### Card Reorganization
- Are you open to reorganizing which properties appear in which cards? no
- Or should card structure remain exactly as-is? yes

---

## 5. Loading States

### Initial Load
Is the current splash screen sufficient, or do you want:
- A) Keep splash screen as-is no
- B) Replace splash with skeleton/shimmer placeholders in MainPage yes
- C) Add progress indicator to splash showing what's loading yes

### Refresh Behavior
- Is there currently a refresh button/command in the app?
- If so, should it show inline loading state or is the current behavior acceptable?

---

## 6. Title Bar

### Theme Selector
Current implementation uses a ComboBox. Preferences?
- A) Keep ComboBox, adjust width/styling no
- B) Replace with icon button that opens flyout menu yes
- C) Move theme selection to About window or Settings no
- D) No change needed no

### App Icon
- Should the title bar icon have a transparent background? no
- Is 16x16 the right size, or should it be larger (20x20)? yes, 20x20

---

## 7. Accessibility

### Focus Indicators
- Have you noticed any visibility issues with focus rings in specific themes? no
- Should we explicitly verify and document focus visibility for all themes? no

### Keyboard Shortcuts
Are any keyboard shortcuts desired? yes
- Ctrl+R for Refresh? yes
- Ctrl+C to copy all info? yes
- Escape to close About window? yes
- Other?

---

## 8. Empty States

### Missing Data Text
What text should appear when data is unavailable?
- A) "N/A"

---

## 9. About Window

### Layout Changes
- Add separator line between header and content? yes
- Add brief app description (e.g., "Windows system information utility")? yes
- Add link to GitHub repo or documentation? yes

---

## 10. Scope and Priority

### Proposal Grouping
How would you like to group these into proposals?
- A) One proposal per section (8 proposals)
Number propsosals in sequence of build. i.e. 1,2,4,5

### Items to Skip
Are there any suggestions you want to drop entirely? no

### Additional Ideas
Any UI improvements not listed that you'd like to add? no

---

## Next Steps

After answering these questions:
1. I'll create focused OpenSpec proposals based on your preferences
2. Each proposal will include specific requirements and acceptance criteria
3. Proposals can be reviewed and applied incrementally
