## 1. MainViewModel

- [ ] 1.1 Create MainViewModel class using CommunityToolkit.Mvvm
- [ ] 1.2 Add ObservableProperties for all OS info fields
- [ ] 1.3 Add ObservableProperties for all hardware info fields
- [ ] 1.4 Add ObservableProperties for service status display
- [ ] 1.5 Add RefreshCommand for manual refresh
- [ ] 1.6 Add IsRefreshing property for spinner state
- [ ] 1.7 Implement LoadDataAsync() to populate all fields

## 2. MainPage Layout

- [ ] 2.1 Create MainPage.xaml with ScrollViewer container
- [ ] 2.2 Create Operating System card with all 7 properties
- [ ] 2.3 Create Device & Hardware card with all 8 properties
- [ ] 2.4 Create Packaged Service card with service controls
- [ ] 2.5 Use consistent label/value layout across cards
- [ ] 2.6 Add localized strings from Resources.resw

## 3. Card Styling

- [ ] 3.1 Create InfoCard user control or style template
- [ ] 3.2 Apply Fluent Design card appearance
- [ ] 3.3 Add consistent padding and spacing
- [ ] 3.4 Style card headers with bold text

## 4. Responsive Layout

- [ ] 4.1 Implement AdaptiveTrigger for window width
- [ ] 4.2 Cards display horizontally above 1200px width
- [ ] 4.3 Cards stack vertically below 1200px width
- [ ] 4.4 Maintain minimum 800x600 window dimensions

## 5. Title Bar Integration

- [ ] 5.1 Configure extended title bar in MainWindow
- [ ] 5.2 Add theme selector ComboBox to title bar
- [ ] 5.3 Add Refresh button to title bar
- [ ] 5.4 Set proper drag regions for title bar

## 6. Service Card Controls

- [ ] 6.1 Add service type ComboBox (Background Task, App Service, Full Trust Process)
- [ ] 6.2 Add status indicator with colored circle
- [ ] 6.3 Add uptime display (HH:MM:SS format)
- [ ] 6.4 Add Start/Stop Service button
- [ ] 6.5 Add service-specific content area

## 7. Accessibility

- [ ] 7.1 Add AutomationProperties.Name to all interactive controls
- [ ] 7.2 Add AutomationProperties.AutomationId to cards
- [ ] 7.3 Configure TabIndex for keyboard navigation order
- [ ] 7.4 Ensure all controls are keyboard accessible

## 8. Refresh Functionality

- [ ] 8.1 Implement refresh button click handler
- [ ] 8.2 Show spinner during refresh
- [ ] 8.3 Disable button during refresh
- [ ] 8.4 Refresh all data including service reconnection

## 9. Validation

- [ ] 9.1 Verify all system info displays correctly
- [ ] 9.2 Verify responsive layout transitions
- [ ] 9.3 Test keyboard navigation order
- [ ] 9.4 Test with screen reader (Narrator)
