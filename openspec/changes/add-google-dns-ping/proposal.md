## Why

Users need a way to verify external internet connectivity by pinging a known public DNS server (Google DNS at 8.8.8.8). This provides a diagnostic baseline for network troubleshooting, separate from local DNS server tests, confirming that the machine can reach the internet.

## What Changes

- Add a "Google DNS" row at the bottom of the Network Information card
- Display "8.8.8.8" as a static value
- Add a Ping button beside it that pings 8.8.8.8 five times
- Display ping results as "X/5" on the button
- Color-code results: green (5/5), yellow (1-4), red (0/5)
- Clicking the button again resets it to "Ping" state

## Impact

- Affected specs: `network-info`
- Affected code: `MainPage.xaml`, `MainViewModel.cs`, `Resources.resw`
