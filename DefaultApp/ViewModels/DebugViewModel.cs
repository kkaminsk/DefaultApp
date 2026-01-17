#if DEBUG
using CommunityToolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace DefaultApp.ViewModels;

/// <summary>
/// ViewModel for the debug page with mock data options.
/// Only available in DEBUG builds.
/// </summary>
public partial class DebugViewModel : ObservableObject
{
    private const string UseMockDataKey = "Debug_UseMockData";
    private const string SimulateSlowLoadingKey = "Debug_SimulateSlowLoading";
    private const string SimulateErrorsKey = "Debug_SimulateErrors";

    private readonly ApplicationDataContainer _localSettings;

    public DebugViewModel()
    {
        _localSettings = ApplicationData.Current.LocalSettings;
        LoadSettings();
    }

    [ObservableProperty]
    private bool _useMockData;

    [ObservableProperty]
    private bool _simulateSlowLoading;

    [ObservableProperty]
    private bool _simulateErrors;

    partial void OnUseMockDataChanged(bool value)
    {
        _localSettings.Values[UseMockDataKey] = value;
    }

    partial void OnSimulateSlowLoadingChanged(bool value)
    {
        _localSettings.Values[SimulateSlowLoadingKey] = value;
    }

    partial void OnSimulateErrorsChanged(bool value)
    {
        _localSettings.Values[SimulateErrorsKey] = value;
    }

    private void LoadSettings()
    {
        if (_localSettings.Values.TryGetValue(UseMockDataKey, out var useMockData))
        {
            UseMockData = (bool)useMockData;
        }

        if (_localSettings.Values.TryGetValue(SimulateSlowLoadingKey, out var slowLoading))
        {
            SimulateSlowLoading = (bool)slowLoading;
        }

        if (_localSettings.Values.TryGetValue(SimulateErrorsKey, out var errors))
        {
            SimulateErrors = (bool)errors;
        }
    }

    /// <summary>
    /// Gets whether mock data is enabled.
    /// Can be called from anywhere to check the setting.
    /// </summary>
    public static bool IsMockDataEnabled
    {
        get
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(UseMockDataKey, out var value))
            {
                return (bool)value;
            }
            return false;
        }
    }

    /// <summary>
    /// Gets whether slow loading simulation is enabled.
    /// </summary>
    public static bool IsSlowLoadingEnabled
    {
        get
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(SimulateSlowLoadingKey, out var value))
            {
                return (bool)value;
            }
            return false;
        }
    }

    /// <summary>
    /// Gets whether error simulation is enabled.
    /// </summary>
    public static bool IsErrorSimulationEnabled
    {
        get
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(SimulateErrorsKey, out var value))
            {
                return (bool)value;
            }
            return false;
        }
    }
}
#endif
