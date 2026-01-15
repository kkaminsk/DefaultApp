## 1. Background Task Implementation

- [ ] 1.1 Create TimerBackgroundTask class implementing IBackgroundTask
- [ ] 1.2 Implement Run method with deferral handling
- [ ] 1.3 Create DispatcherTimer with 1-second interval
- [ ] 1.4 Implement timer tick handler to broadcast current time

## 2. Service Controller

- [ ] 2.1 Create ServiceController class in main app
- [ ] 2.2 Implement StartBackgroundTaskAsync() method
- [ ] 2.3 Implement StopBackgroundTask() method
- [ ] 2.4 Track service status (Starting, Running, Stopping, Stopped, Error)
- [ ] 2.5 Track service uptime with stopwatch
- [ ] 2.6 Implement auto-start on application launch

## 3. Background Task Registration

- [ ] 3.1 Implement RegisterBackgroundTask() method
- [ ] 3.2 Unregister existing task before re-registering
- [ ] 3.3 Use ApplicationTrigger for on-demand activation
- [ ] 3.4 Register on every app launch

## 4. Package Manifest

- [ ] 4.1 Add windows.backgroundTasks extension to manifest
- [ ] 4.2 Configure EntryPoint to DefaultApp.BackgroundTasks.TimerBackgroundTask
- [ ] 4.3 Add timer and general task types

## 5. UI Integration

- [ ] 5.1 Wire ServiceController to MainViewModel
- [ ] 5.2 Display current time from timer in service card
- [ ] 5.3 Update status indicator based on service state
- [ ] 5.4 Update uptime display every second
- [ ] 5.5 Bind Start/Stop button to service actions

## 6. Status Colors

- [ ] 6.1 Implement Starting state with Yellow/Orange indicator
- [ ] 6.2 Implement Running state with Green indicator
- [ ] 6.3 Implement Stopping state with Yellow/Orange indicator
- [ ] 6.4 Implement Stopped state with Gray indicator
- [ ] 6.5 Implement Error state with Red indicator

## 7. Service Switching

- [ ] 7.1 Stop current service when switching types
- [ ] 7.2 Start new service after previous stops
- [ ] 7.3 Handle switching during Starting/Stopping states

## 8. Error Handling

- [ ] 8.1 Catch and log background task failures
- [ ] 8.2 Display error dialog prompting restart on start failure
- [ ] 8.3 Set Error status on unrecoverable failures

## 9. Validation

- [ ] 9.1 Verify timer updates UI every second
- [ ] 9.2 Verify uptime increments correctly
- [ ] 9.3 Verify status colors match states
- [ ] 9.4 Verify service stops when app closes
- [ ] 9.5 Verify re-registration on app restart
