using DefaultApp.Models;
using Microsoft.Win32;

namespace DefaultApp.Services;

/// <summary>
/// Service for checking Windows activation status.
/// </summary>
public sealed class ActivationService
{
    private const string SoftwareProtectionRegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform";
    private ActivationStatus? _cachedStatus;
    private readonly object _lock = new();

    /// <summary>
    /// Gets the Windows activation status asynchronously.
    /// Results are cached after the first successful call.
    /// </summary>
    /// <returns>The activation status.</returns>
    public async Task<ActivationStatus> GetActivationStatusAsync()
    {
        lock (_lock)
        {
            if (_cachedStatus.HasValue)
            {
                return _cachedStatus.Value;
            }
        }

        var status = await Task.Run(CheckActivationStatus);

        lock (_lock)
        {
            _cachedStatus = status;
        }

        return status;
    }

    /// <summary>
    /// Gets the display string for an activation status.
    /// </summary>
    public static string GetActivationStatusDisplay(ActivationStatus status)
    {
        return status switch
        {
            ActivationStatus.Activated => "Activated",
            ActivationStatus.NotActivated => "Not Activated",
            ActivationStatus.GracePeriod => "Grace Period",
            ActivationStatus.NotificationMode => "Notification Mode",
            ActivationStatus.Checking => "Checking...",
            ActivationStatus.Unavailable => "Unavailable",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Clears the cached activation status (useful for testing or refresh).
    /// </summary>
    public void ClearCache()
    {
        lock (_lock)
        {
            _cachedStatus = null;
        }
    }

    private static ActivationStatus CheckActivationStatus()
    {
        // Try slmgr first - most reliable for all license types including Volume/Enterprise
        var slmgrStatus = CheckActivationStatusFromSlmgr();
        if (slmgrStatus != ActivationStatus.Unavailable)
        {
            return slmgrStatus;
        }

        // Try SLGetWindowsInformationDWORD - works well for retail licenses
        var slInfoStatus = CheckActivationStatusFromSlInfo();
        if (slInfoStatus == ActivationStatus.Activated)
        {
            // Only trust "Activated" result - "Not Genuine" can be wrong for volume licenses
            return slInfoStatus;
        }

        // Try SLIsGenuineLocal
        var genuineStatus = CheckActivationStatusFromPInvoke();
        if (genuineStatus == ActivationStatus.Activated)
        {
            return genuineStatus;
        }

        // Try Registry-based detection
        var registryStatus = CheckActivationStatusFromRegistry();
        if (registryStatus != ActivationStatus.Unavailable)
        {
            return registryStatus;
        }

        // Return the best non-Unavailable result we got, or Unavailable
        if (slInfoStatus != ActivationStatus.Unavailable)
        {
            return slInfoStatus;
        }
        if (genuineStatus != ActivationStatus.Unavailable)
        {
            return genuineStatus;
        }

        return ActivationStatus.Unavailable;
    }

    /// <summary>
    /// Checks activation status using SLGetWindowsInformationDWORD.
    /// </summary>
    private static ActivationStatus CheckActivationStatusFromSlInfo()
    {
        try
        {
            // Try Kernel-WindowsLicenseStatus first - this directly indicates license status
            // Values: 0=Unlicensed, 1=Licensed, 2=OOBGrace, 3=OOTGrace, 4=NonGenuineGrace, 5=Notification, 6=ExtendedGrace
            var licenseResult = NativeMethods.SLGetWindowsInformationDWORD(
                "Kernel-WindowsLicenseStatus",
                out var licenseStatus);

            if (licenseResult == 0)
            {
                return licenseStatus switch
                {
                    1 => ActivationStatus.Activated,     // Licensed
                    2 or 3 or 6 => ActivationStatus.GracePeriod, // Various grace periods
                    4 or 5 => ActivationStatus.NotificationMode, // Non-genuine or notification
                    0 => ActivationStatus.NotActivated,   // Unlicensed
                    _ => ActivationStatus.Unavailable
                };
            }

            // Fall back to Security-SPP-GenuineLocalStatus
            var genuineResult = NativeMethods.SLGetWindowsInformationDWORD(
                "Security-SPP-GenuineLocalStatus",
                out var genuineStatus);

            // S_OK = 0
            if (genuineResult == 0)
            {
                // 0 = Genuine/Activated, 1 = Not Genuine
                return genuineStatus == 0 ? ActivationStatus.Activated : ActivationStatus.NotActivated;
            }

            return ActivationStatus.Unavailable;
        }
        catch
        {
            return ActivationStatus.Unavailable;
        }
    }

    /// <summary>
    /// Checks activation status using Registry keys.
    /// </summary>
    private static ActivationStatus CheckActivationStatusFromRegistry()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(SoftwareProtectionRegistryKey);
            if (key is null)
            {
                return ActivationStatus.Unavailable;
            }

            // Try to read LicenseStatus (not always present)
            // We can also check activation status via other indicators
            var notificationReason = key.GetValue("NotificationReason");
            if (notificationReason is int reason)
            {
                // NotificationReason 0 = activated, non-zero = not activated
                return reason == 0 ? ActivationStatus.Activated : ActivationStatus.NotActivated;
            }

            // Alternative: Check if genuine ticket exists
            var genuineTicket = key.GetValue("GenuineTicket");
            if (genuineTicket is not null)
            {
                return ActivationStatus.Activated;
            }

            return ActivationStatus.Unavailable;
        }
        catch
        {
            return ActivationStatus.Unavailable;
        }
    }

    /// <summary>
    /// Checks activation status using slmgr script output.
    /// This is the most reliable method for all license types including Volume/Enterprise.
    /// </summary>
    private static ActivationStatus CheckActivationStatusFromSlmgr()
    {
        try
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cscript.exe",
                Arguments = "//nologo C:\\Windows\\System32\\slmgr.vbs /dli",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process is null)
            {
                return ActivationStatus.Unavailable;
            }

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(5000);

            if (string.IsNullOrEmpty(output))
            {
                return ActivationStatus.Unavailable;
            }

            // Check for license status in output
            // The output contains "License Status: Licensed" or similar
            if (output.Contains("License Status: Licensed", StringComparison.OrdinalIgnoreCase))
            {
                return ActivationStatus.Activated;
            }

            if (output.Contains("License Status: Notification", StringComparison.OrdinalIgnoreCase))
            {
                return ActivationStatus.NotificationMode;
            }

            if (output.Contains("License Status: Initial Grace", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("License Status: Extended Grace", StringComparison.OrdinalIgnoreCase) ||
                output.Contains("Grace", StringComparison.OrdinalIgnoreCase))
            {
                return ActivationStatus.GracePeriod;
            }

            if (output.Contains("License Status: Unlicensed", StringComparison.OrdinalIgnoreCase))
            {
                return ActivationStatus.NotActivated;
            }

            return ActivationStatus.Unavailable;
        }
        catch
        {
            return ActivationStatus.Unavailable;
        }
    }

    /// <summary>
    /// Checks activation status using P/Invoke (SLIsGenuineLocal).
    /// </summary>
    private static ActivationStatus CheckActivationStatusFromPInvoke()
    {
        try
        {
            var windowsSlid = NativeMethods.WINDOWS_SLID;
            var result = NativeMethods.SLIsGenuineLocal(
                ref windowsSlid,
                out var genuineState,
                IntPtr.Zero);

            // S_OK = 0, successful call
            if (result == 0)
            {
                return genuineState switch
                {
                    NativeMethods.SL_GENUINE_STATE.SL_GEN_STATE_IS_GENUINE => ActivationStatus.Activated,
                    NativeMethods.SL_GENUINE_STATE.SL_GEN_STATE_INVALID_LICENSE => ActivationStatus.NotActivated,
                    NativeMethods.SL_GENUINE_STATE.SL_GEN_STATE_TAMPERED => ActivationStatus.NotActivated,
                    NativeMethods.SL_GENUINE_STATE.SL_GEN_STATE_OFFLINE => ActivationStatus.GracePeriod,
                    _ => ActivationStatus.Unavailable
                };
            }

            // Handle specific HRESULT codes
            return MapHResultToActivationStatus(result);
        }
        catch
        {
            return ActivationStatus.Unavailable;
        }
    }

    private static ActivationStatus MapHResultToActivationStatus(int hResult)
    {
        // Common HRESULT codes for licensing
        const int SL_E_GRACE_TIME_EXPIRED = unchecked((int)0xC004F009);
        const int SL_E_NOT_GENUINE = unchecked((int)0xC004F200);
        const int SL_E_NOTIFICATION_RULES_FAILURE = unchecked((int)0xC004F057);

        return hResult switch
        {
            SL_E_GRACE_TIME_EXPIRED => ActivationStatus.NotificationMode,
            SL_E_NOT_GENUINE => ActivationStatus.NotActivated,
            SL_E_NOTIFICATION_RULES_FAILURE => ActivationStatus.NotificationMode,
            _ => ActivationStatus.Unavailable
        };
    }
}
