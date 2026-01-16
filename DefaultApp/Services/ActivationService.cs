using DefaultApp.Models;

namespace DefaultApp.Services;

/// <summary>
/// Service for checking Windows activation status.
/// </summary>
public sealed class ActivationService
{
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
