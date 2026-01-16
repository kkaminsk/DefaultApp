## Why

The current Device & Hardware card displays CPU Model in the middle of the list. Users want to see the most important hardware information—CPU Model, Processor Count, and Total RAM—at the top of the card for faster scanning.

## What Changes

- Reorder fields in the Device & Hardware card to place CPU Model at the top
- Place Processor Count immediately after CPU Model
- Place Total RAM immediately after Processor Count
- Other fields remain in their current relative order below these three

## Impact

- Affected specs: `main-ui`
- Affected code: `DefaultApp/Views/MainPage.xaml`
