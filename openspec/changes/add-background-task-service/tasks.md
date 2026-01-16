## 1. Background Task Implementation

- [x] 1.1 Create TimerBackgroundTask class implementing IBackgroundTask
- [x] 1.2 Implement Run method with deferral handling
- [x] 1.3 Create DispatcherTimer with 1-second interval
- [x] 1.4 Implement timer tick handler to broadcast current time

## 2. Service Controller

- [x] 2.1 Create ServiceController class in main app
- [x] 2.2 Implement StartBackgroundTaskAsync() method
- [x] 2.3 Implement StopBackgroundTask() method
- [x] 2.4 Track service status (Starting, Running, Stopping, Stopped, Error)
- [x] 2.5 Track service uptime with stopwatch
- [x] 2.6 Implement auto-start on application launch

## 3. Background Task Registration

- [x] 3.1 Implement RegisterBackgroundTask() method
- [x] 3.2 Unregister existing task before re-registering
- [x] 3.3 Use ApplicationTrigger for on-demand activation
- [x] 3.4 Register on every app launch

## 4. Package Manifest

- [x] 4.1 Add windows.backgroundTasks extension to manifest
- [x] 4.2 Configure EntryPoint to DefaultApp.BackgroundTasks.TimerBackgroundTask
- [x] 4.3 Add timer and general task types

## 5. UI Integration

- [x] 5.1 Wire ServiceController to MainViewModel
- [x] 5.2 Display current time from timer in service card
- [x] 5.3 Update status indicator based on service state
- [x] 5.4 Update uptime display every second
- [x] 5.5 Bind Start/Stop button to service actions

## 6. Status Colors

- [x] 6.1 Implement Starting state with Yellow/Orange indicator
- [x] 6.2 Implement Running state with Green indicator
- [x] 6.3 Implement Stopping state with Yellow/Orange indicator
- [x] 6.4 Implement Stopped state with Gray indicator
- [x] 6.5 Implement Error state with Red indicator

## 7. Service Switching

- [x] 7.1 Stop current service when switching types
- [x] 7.2 Start new service after previous stops
- [x] 7.3 Handle switching during Starting/Stopping states

## 8. Error Handling

- [x] 8.1 Catch and log background task failures
- [x] 8.2 Display error dialog prompting restart on start failure (via ErrorOccurred event)
- [x] 8.3 Set Error status on unrecoverable failures

## 9. Validation

- [x] 9.1 Verify timer updates UI every second
- [x] 9.2 Verify uptime increments correctly
- [x] 9.3 Verify status colors match states
- [x] 9.4 Verify service stops when app closes
- [x] 9.5 Verify re-registration on app restart
