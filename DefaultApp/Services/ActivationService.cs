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
        // Try SLGetWindowsInformationDWORD first (most reliable)
        var slInfoStatus = CheckActivationStatusFromSlInfo();
        if (slInfoStatus != ActivationStatus.Unavailable)
        {
            return slInfoStatus;
        }

        // Try SLIsGenuineLocal next
        var genuineStatus = CheckActivationStatusFromPInvoke();
        if (genuineStatus != ActivationStatus.Unavailable)
        {
            return genuineStatus;
        }

        // Fall back to Registry-based detection
        return CheckActivationStatusFromRegistry();
    }

    /// <summary>
    /// Checks activation status using SLGetWindowsInformationDWORD.
    /// </summary>
    private static ActivationStatus CheckActivationStatusFromSlInfo()
    {
        try
        {
            var result = NativeMethods.SLGetWindowsInformationDWORD(
                "Security-SPP-GenuineLocalStatus",
                out var genuineStatus);

            // S_OK = 0
            if (result == 0)
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
