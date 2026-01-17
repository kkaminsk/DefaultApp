using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Windows.Storage;
using Windows.UI.ViewManagement;

namespace DefaultApp.Services;

/// <summary>
/// Available application themes.
/// </summary>
public enum AppTheme
{
    SystemDefault,
    Inverted
}

/// <summary>
/// Service for managing application themes with persistence and real-time switching.
/// </summary>
public sealed class ThemeService : IDisposable
{
    private const string ThemeSettingsKey = "AppTheme";
    private readonly ILogger<ThemeService>? _logger;
    private readonly UISettings _uiSettings;
    private AppTheme _currentTheme;
    private FrameworkElement? _rootElement;
    private bool _isDisposed;

    /// <summary>
    /// Event raised when the theme changes.
    /// </summary>
    public event EventHandler<AppTheme>? ThemeChanged;

    public ThemeService()
    {
        _logger = App.LoggerFactory?.CreateLogger<ThemeService>();
        _uiSettings = new UISettings();
        _currentTheme = LoadSavedTheme();

        // Subscribe to system theme changes
        _uiSettings.ColorValuesChanged += OnSystemColorValuesChanged;
    }

    /// <summary>
    /// Gets the current theme.
    /// </summary>
    public AppTheme CurrentTheme => _currentTheme;

    /// <summary>
    /// Initializes the theme service with the root element for theme application.
    /// </summary>
    /// <param name="rootElement">The root FrameworkElement to apply themes to.</param>
    public void Initialize(FrameworkElement rootElement)
    {
        _rootElement = rootElement;
        ApplyTheme(_currentTheme);
    }

    /// <summary>
    /// Sets the application theme.
    /// </summary>
    /// <param name="theme">The theme to apply.</param>
    public void SetTheme(AppTheme theme)
    {
        if (_isDisposed)
        {
            return;
        }

        System.Diagnostics.Debug.WriteLine($"[ThemeService.SetTheme] Called with theme: {theme}, current: {_currentTheme}");

        if (_currentTheme == theme)
        {
            System.Diagnostics.Debug.WriteLine("[ThemeService.SetTheme] Theme unchanged, skipping");
            return;
        }

        _logger?.LogInformation("Changing theme from {OldTheme} to {NewTheme}", _currentTheme, theme);
        System.Diagnostics.Debug.WriteLine($"[ThemeService.SetTheme] Changing from {_currentTheme} to {theme}");

        _currentTheme = theme;
        SaveTheme(theme);
        ApplyTheme(theme);
        ThemeChanged?.Invoke(this, theme);

        System.Diagnostics.Debug.WriteLine($"[ThemeService.SetTheme] Theme change complete");
    }

    /// <summary>
    /// Gets the theme to display based on current settings.
    /// If SystemDefault is selected, returns the actual theme being used.
    /// </summary>
    public AppTheme GetEffectiveTheme()
    {
        if (_currentTheme == AppTheme.SystemDefault)
        {
            return AppTheme.SystemDefault;
        }
        return _currentTheme;
    }

    /// <summary>
    /// Checks if Windows high contrast mode is enabled.
    /// </summary>
    public bool IsHighContrastEnabled()
    {
        var accessibilitySettings = new AccessibilitySettings();
        return accessibilitySettings.HighContrast;
    }

    /// <summary>
    /// Checks if the system is currently in dark mode.
    /// </summary>
    public bool IsSystemInDarkMode()
    {
        var foreground = _uiSettings.GetColorValue(UIColorType.Foreground);
        // If foreground is light, we're in dark mode
        return foreground.R > 128 && foreground.G > 128 && foreground.B > 128;
    }

    private void ApplyTheme(AppTheme theme)
    {
        System.Diagnostics.Debug.WriteLine($"[ThemeService.ApplyTheme] Called with theme: {theme}, rootElement null: {_rootElement is null}");

        if (_rootElement is null)
        {
            _logger?.LogWarning("Cannot apply theme: root element not initialized");
            System.Diagnostics.Debug.WriteLine("[ThemeService.ApplyTheme] ERROR: Root element is null!");
            return;
        }

        // Check for Windows high contrast override
        if (IsHighContrastEnabled())
        {
            _logger?.LogInformation("Windows high contrast mode detected, using system default");
            System.Diagnostics.Debug.WriteLine("[ThemeService.ApplyTheme] High contrast override - using Default");
            _rootElement.RequestedTheme = ElementTheme.Default;
            return;
        }

        _logger?.LogDebug("Applying theme: {Theme}", theme);
        System.Diagnostics.Debug.WriteLine($"[ThemeService.ApplyTheme] Setting RequestedTheme for theme: {theme}");

        switch (theme)
        {
            case AppTheme.Inverted:
                // Apply the opposite of the system theme
                var invertedTheme = IsSystemInDarkMode() ? ElementTheme.Light : ElementTheme.Dark;
                System.Diagnostics.Debug.WriteLine($"[ThemeService.ApplyTheme] Applying Inverted theme (system is {(IsSystemInDarkMode() ? "dark" : "light")}, using {invertedTheme})");
                _rootElement.RequestedTheme = invertedTheme;
                break;

            case AppTheme.SystemDefault:
            default:
                System.Diagnostics.Debug.WriteLine("[ThemeService.ApplyTheme] Applying Default theme");
                _rootElement.RequestedTheme = ElementTheme.Default;
                break;
        }

        System.Diagnostics.Debug.WriteLine($"[ThemeService.ApplyTheme] Complete. RequestedTheme is now: {_rootElement.RequestedTheme}");
    }

    private void ApplyCustomTheme(string themeName)
    {
        try
        {
            var app = Application.Current;
            var resources = app.Resources;

            // Remove any existing custom theme
            RemoveCustomTheme();

            // Load and apply the custom theme
            var themeUri = new Uri($"ms-appx:///Themes/{themeName}.xaml");
            var themeDictionary = new ResourceDictionary { Source = themeUri };

            // Mark this dictionary so we can remove it later
            themeDictionary["_CustomThemeMarker"] = themeName;

            resources.MergedDictionaries.Add(themeDictionary);

            _logger?.LogDebug("Applied custom theme: {Theme}", themeName);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to apply custom theme: {Theme}", themeName);
        }
    }

    private void RemoveCustomTheme()
    {
        try
        {
            var app = Application.Current;
            var resources = app.Resources;

            // Find and remove custom theme dictionaries
            var toRemove = resources.MergedDictionaries
                .Where(d => d.ContainsKey("_CustomThemeMarker"))
                .ToList();

            foreach (var dict in toRemove)
            {
                resources.MergedDictionaries.Remove(dict);
                _logger?.LogDebug("Removed custom theme dictionary");
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to remove custom theme");
        }
    }

    private AppTheme LoadSavedTheme()
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.TryGetValue(ThemeSettingsKey, out var value) && value is int themeValue)
            {
                var theme = (AppTheme)themeValue;
                _logger?.LogInformation("Loaded saved theme: {Theme}", theme);
                return theme;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load saved theme");
        }

        return AppTheme.SystemDefault;
    }

    private void SaveTheme(AppTheme theme)
    {
        try
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[ThemeSettingsKey] = (int)theme;
            _logger?.LogDebug("Saved theme setting: {Theme}", theme);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to save theme setting");
        }
    }

    private void OnSystemColorValuesChanged(UISettings sender, object args)
    {
        // System theme changed - if we're following system default, update
        if (_currentTheme == AppTheme.SystemDefault && _rootElement is not null)
        {
            _logger?.LogInformation("System theme changed, updating app theme");

            // Dispatch to UI thread
            _rootElement.DispatcherQueue.TryEnqueue(() =>
            {
                ApplyTheme(_currentTheme);
                ThemeChanged?.Invoke(this, _currentTheme);
            });
        }
    }

    /// <summary>
    /// Maps a ComboBox selection index to AppTheme.
    /// </summary>
    public static AppTheme FromComboBoxIndex(int index)
    {
        return index switch
        {
            0 => AppTheme.SystemDefault,
            1 => AppTheme.Inverted,
            _ => AppTheme.SystemDefault
        };
    }

    /// <summary>
    /// Maps an AppTheme to ComboBox selection index.
    /// </summary>
    public static int ToComboBoxIndex(AppTheme theme)
    {
        return theme switch
        {
            AppTheme.SystemDefault => 0,
            AppTheme.Inverted => 1,
            _ => 0
        };
    }

    /// <summary>
    /// Disposes the theme service and unsubscribes from system events.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _uiSettings.ColorValuesChanged -= OnSystemColorValuesChanged;
        _logger?.LogDebug("ThemeService disposed");
    }
}
