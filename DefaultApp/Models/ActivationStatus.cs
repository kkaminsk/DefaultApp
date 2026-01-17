namespace DefaultApp.Models;

/// <summary>
/// Represents the Windows activation status states.
/// </summary>
public enum ActivationStatus
{
    /// <summary>
    /// Windows is genuinely activated.
    /// </summary>
    Activated,

    /// <summary>
    /// Windows is not activated.
    /// </summary>
    NotActivated,

    /// <summary>
    /// Windows is in grace period (initial or rearm).
    /// </summary>
    GracePeriod,

    /// <summary>
    /// Windows is in notification mode (reduced functionality).
    /// </summary>
    NotificationMode,

    /// <summary>
    /// Activation status is currently being checked.
    /// </summary>
    Checking,

    /// <summary>
    /// Unable to determine activation status.
    /// </summary>
    Unavailable
}
