## Why

Users need a quick way to verify connectivity to their DNS server without leaving the application. This mirrors the existing Default Gateway ping functionality and provides immediate diagnostic feedback for DNS connectivity issues.

## What Changes

- Add a "Ping" button to the right of the DNS Server copy button
- When clicked, ping the DNS server IP address 5 times
- Display ping results as "X/5" on the button (e.g., "3/5" means 3 successful pings out of 5)
- Button shows "Ping" initially, then progress during pinging, then final result
- Color-code results: green (5/5), yellow (1-4), red (0/5)
- Clicking the button again resets it to "Ping" state

## Impact

- Affected specs: `network-info`
- Affected code: `MainPage.xaml`, `MainViewModel.cs`, `Resources.resw`
