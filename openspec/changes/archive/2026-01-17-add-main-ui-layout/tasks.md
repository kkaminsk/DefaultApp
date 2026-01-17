## 1. MainViewModel

- [x] 1.1 Create MainViewModel class using CommunityToolkit.Mvvm
- [x] 1.2 Add ObservableProperties for all OS info fields
- [x] 1.3 Add ObservableProperties for all hardware info fields
- [x] 1.4 Add ObservableProperties for service status display
- [x] 1.5 Add RefreshCommand for manual refresh
- [x] 1.6 Add IsRefreshing property for spinner state
- [x] 1.7 Implement LoadDataAsync() to populate all fields

## 2. MainPage Layout

- [x] 2.1 Create MainPage.xaml with ScrollViewer container
- [x] 2.2 Create Operating System card with all 7 properties
- [x] 2.3 Create Device & Hardware card with all 8 properties
- [x] 2.4 Create Packaged Service card with service controls
- [x] 2.5 Use consistent label/value layout across cards
- [x] 2.6 Add localized strings from Resources.resw

## 3. Card Styling

- [x] 3.1 Create InfoCard user control or style template
- [x] 3.2 Apply Fluent Design card appearance
- [x] 3.3 Add consistent padding and spacing
- [x] 3.4 Style card headers with bold text

## 4. Responsive Layout

- [x] 4.1 Implement AdaptiveTrigger for window width
- [x] 4.2 Cards display horizontally above 1200px width
- [x] 4.3 Cards stack vertically below 1200px width
- [x] 4.4 Maintain minimum 800x600 window dimensions

## 5. Title Bar Integration

- [x] 5.1 Configure extended title bar in MainWindow
- [x] 5.2 Add theme selector ComboBox to title bar
- [x] 5.3 Add Refresh button to title bar
- [x] 5.4 Set proper drag regions for title bar

## 6. Service Card Controls

- [x] 6.1 Add service type ComboBox (Background Task, App Service, Full Trust Process)
- [x] 6.2 Add status indicator with colored circle
- [x] 6.3 Add uptime display (HH:MM:SS format)
- [x] 6.4 Add Start/Stop Service button
- [x] 6.5 Add service-specific content area

## 7. Accessibility

- [x] 7.1 Add AutomationProperties.Name to all interactive controls
- [x] 7.2 Add AutomationProperties.AutomationId to cards
- [x] 7.3 Configure TabIndex for keyboard navigation order
- [x] 7.4 Ensure all controls are keyboard accessible

## 8. Refresh Functionality

- [x] 8.1 Implement refresh button click handler
- [x] 8.2 Show spinner during refresh
- [x] 8.3 Disable button during refresh
- [x] 8.4 Refresh all data including service reconnection

## 9. Validation

- [x] 9.1 Verify all system info displays correctly
- [x] 9.2 Verify responsive layout transitions
- [x] 9.3 Test keyboard navigation order
- [x] 9.4 Test with screen reader (Narrator)
