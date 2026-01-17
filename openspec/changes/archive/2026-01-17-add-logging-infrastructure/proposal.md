## Why

The application needs comprehensive logging for diagnostics, debugging, and audit purposes. This includes file-based logging with rotation, ETW provider integration, and service audit logging.

## What Changes

- Create LoggingService with file output to %LocalAppData%\DefaultApp\Logs\
- Implement log rotation (10 MB max, 5 files retained)
- Create ETW provider with auto-generated GUID
- Route ETW events to file log
- Add audit logging for service start/stop events
- Implement log levels (Trace, Debug, Information, Warning, Error)

## Impact

- Affected specs: `logging` (new capability)
- Affected code: Services/LoggingService.cs, all service files
- Dependencies: Requires `add-project-foundation`
- Related: Used by all other components for diagnostics
