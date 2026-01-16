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
    Light,
    Dark,
    Cyberpunk,
    HighContrastDark,
    HighContrastLight
}

/// <summary>
/// Service for managing application themes with persistence and real-time switching.
/// </summary>
public sealed class ThemeService
{
    private const string ThemeSettingsKey = "AppTheme";
    private readonly ILogger<ThemeService>? _logger;
    private readonly UISettings _uiSettings;
    private AppTheme _currentTheme;
    private FrameworkElement? _rootElement;

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
        if (_currentTheme == theme)
        {
            return;
        }

        _logger?.LogInformation("Changing theme from {OldTheme} to {NewTheme}", _currentTheme, theme);

        _currentTheme = theme;
        SaveTheme(theme);
        ApplyTheme(theme);
        ThemeChanged?.Invoke(this, theme);
    }

    /// <summary>
    /// Gets the theme to display based on current settings.
    /// If SystemDefault is selected, returns the actual theme being used.
    /// </summary>
    public AppTheme GetEffectiveTheme()
    {
        if (_currentTheme == AppTheme.SystemDefault)
        {
            return IsSystemInDarkMode() ? AppTheme.Dark : AppTheme.Light;
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
        if (_rootElement is null)
        {
            _logger?.LogWarning("Cannot apply theme: root element not initialized");
            return;
        }

        var effectiveTheme = theme;
        if (theme == AppTheme.SystemDefault)
        {
            effectiveTheme = IsSystemInDarkMode() ? AppTheme.Dark : AppTheme.Light;
        }

        // Check for Windows high contrast override
        if (IsHighContrastEnabled() && theme != AppTheme.HighContrastDark && theme != AppTheme.HighContrastLight)
        {
            _logger?.LogInformation("Windows high contrast mode detected, overriding theme");
            // Let WinUI handle high contrast automatically
            _rootElement.RequestedTheme = ElementTheme.Default;
            return;
        }

        _logger?.LogDebug("Applying effective theme: {Theme}", effectiveTheme);

        switch (effectiveTheme)
        {
            case AppTheme.Light:
                _rootElement.RequestedTheme = ElementTheme.Light;
                RemoveCustomTheme();
                break;

            case AppTheme.Dark:
                _rootElement.RequestedTheme = ElementTheme.Dark;
                RemoveCustomTheme();
                break;

            case AppTheme.Cyberpunk:
                _rootElement.RequestedTheme = ElementTheme.Dark; // Base on dark theme
                ApplyCustomTheme("Cyberpunk");
                break;

            case AppTheme.HighContrastDark:
                _rootElement.RequestedTheme = ElementTheme.Dark;
                ApplyCustomTheme("HighContrastDark");
                break;

            case AppTheme.HighContrastLight:
                _rootElement.RequestedTheme = ElementTheme.Light;
                ApplyCustomTheme("HighContrastLight");
                break;

            default:
                _rootElement.RequestedTheme = ElementTheme.Default;
                RemoveCustomTheme();
                break;
        }
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
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            3 => AppTheme.Cyberpunk,
            4 => AppTheme.HighContrastDark,
            5 => AppTheme.HighContrastLight,
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
            AppTheme.Light => 1,
            AppTheme.Dark => 2,
            AppTheme.Cyberpunk => 3,
            AppTheme.HighContrastDark => 4,
            AppTheme.HighContrastLight => 5,
            _ => 0
        };
    }
}
