## Why

Users need a quick way to verify network connectivity to their default gateway without leaving the application. A Ping button provides immediate diagnostic feedback.

## What Changes

- Add a "Ping" button to the right of the Default Gateway copy button
- When clicked, ping the gateway IP address 5 times
- Display ping results as "X/5" on the button (e.g., "3/5" means 3 successful pings out of 5)
- Button shows "Ping" initially, then progress during pinging, then final result

## Impact

- Affected specs: `network-info`
- Affected code: `MainPage.xaml`, `MainViewModel.cs`, `NetworkInfoService.cs`
