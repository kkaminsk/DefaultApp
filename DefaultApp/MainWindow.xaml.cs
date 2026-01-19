using DefaultApp.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Runtime.InteropServices;
using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace DefaultApp;

/// <summary>
/// Main window of the application with extended title bar support.
/// </summary>
public sealed partial class MainWindow : Window
{
    private AppWindow? _appWindow;
    private ThemeService? _themeService;
    private AboutWindow? _aboutWindow;
    private bool _isInitializingTheme;
    private const int MinWidth = 800;
    private const int MinHeight = 600;

    // P/Invoke for setting minimum window size
    private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    private WndProcDelegate? _newWndProc;
    private IntPtr _oldWndProc;

    private const uint WM_GETMINMAXINFO = 0x0024;

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private const int GWLP_WNDPROC = -4;

    public MainWindow()
    {
        this.InitializeComponent();

        // Set minimum window size
        SetMinimumWindowSize();

        // Configure extended title bar
        ConfigureExtendedTitleBar();

        // Set up drag region
        SetupTitleBarDragRegion();

        // Initialize theme service
        InitializeThemeService();

        // Clean up window subclassing on close
        this.Closed += OnWindowClosed;
    }

    private void OnWindowClosed(object sender, WindowEventArgs args)
    {
        // Dispose ThemeService to unsubscribe from events
        _themeService?.Dispose();

        // Restore original window procedure to clean up subclassing
        var hWnd = WindowNative.GetWindowHandle(this);
        if (hWnd != IntPtr.Zero && _oldWndProc != IntPtr.Zero)
        {
            SetWindowLongPtr(hWnd, GWLP_WNDPROC, _oldWndProc);
        }
    }

    private void SetMinimumWindowSize()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        _appWindow = AppWindow.GetFromWindowId(windowId);

        if (_appWindow is not null)
        {
            // Set initial size to 800x600
            _appWindow.Resize(new Windows.Graphics.SizeInt32(MinWidth, MinHeight));

            // Center window on the primary display
            CenterWindowOnScreen(hWnd);

            // Set presenter to allow resizing with minimum constraints
            if (_appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = true;
                presenter.IsMinimizable = true;
                presenter.IsMaximizable = true;
            }
        }

        // Subclass window to enforce minimum size
        SubclassWindow(hWnd);
    }

    private void SubclassWindow(IntPtr hWnd)
    {
        _newWndProc = new WndProcDelegate(WndProc);
        _oldWndProc = GetWindowLongPtr(hWnd, GWLP_WNDPROC);
        SetWindowLongPtr(hWnd, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(_newWndProc));
    }

    private void CenterWindowOnScreen(IntPtr hWnd)
    {
        if (_appWindow is null)
        {
            return;
        }

        var hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
        var monitorInfo = new MONITORINFO { cbSize = (uint)Marshal.SizeOf<MONITORINFO>() };

        if (GetMonitorInfo(hMonitor, ref monitorInfo))
        {
            var workArea = monitorInfo.rcWork;
            var workWidth = workArea.right - workArea.left;
            var workHeight = workArea.bottom - workArea.top;

            var windowSize = _appWindow.Size;
            var x = workArea.left + (workWidth - windowSize.Width) / 2;
            var y = workArea.top + (workHeight - windowSize.Height) / 2;

            _appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_GETMINMAXINFO)
        {
            var dpi = GetDpiForWindow(hWnd);
            var scaleFactor = dpi / 96.0;

            var minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
            minMaxInfo.ptMinTrackSize.x = (int)(MinWidth * scaleFactor);
            minMaxInfo.ptMinTrackSize.y = (int)(MinHeight * scaleFactor);
            Marshal.StructureToPtr(minMaxInfo, lParam, true);

            return IntPtr.Zero;
        }

        return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
    }

    [DllImport("user32.dll")]
    private static extern uint GetDpiForWindow(IntPtr hWnd);

    // P/Invoke for window centering
    private const int MONITOR_DEFAULTTONEAREST = 2;

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    private void ConfigureExtendedTitleBar()
    {
        // Extend content into the title bar area
        ExtendsContentIntoTitleBar = true;

        // IMPORTANT: Set only the DragRegion as the title bar, NOT the entire AppTitleBar
        // This allows interactive controls (ComboBox, Buttons) to receive mouse input
        SetTitleBar(DragRegion);

        if (_appWindow is not null)
        {
            // Configure title bar for theme selector placement
            var titleBar = _appWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;

            // Set title bar colors (will be updated by theme system later)
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }

    private void SetupTitleBarDragRegion()
    {
        // The drag region is set by the SetTitleBar call
        // Interactive elements (ComboBox, Button) are automatically excluded
    }

    private void InitializeThemeService()
    {
        _themeService = new ThemeService();

        // Initialize with the root element (RootGrid)
        _themeService.Initialize(RootGrid);

        // Set the ComboBox selection to match the saved theme
        _isInitializingTheme = true;
        ThemeSelector.SelectedIndex = ThemeService.ToComboBoxIndex(_themeService.CurrentTheme);
        _isInitializingTheme = false;

        // Wire up SelectionChanged event AFTER initialization to avoid timing issues
        ThemeSelector.SelectionChanged += ThemeSelector_SelectionChanged;

        // Subscribe to theme changes from the service (e.g., system theme changes)
        _themeService.ThemeChanged += OnThemeServiceThemeChanged;
    }

    private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Avoid handling during initialization
        if (_isInitializingTheme || _themeService is null)
        {
            return;
        }

        var selectedTheme = ThemeService.FromComboBoxIndex(ThemeSelector.SelectedIndex);

        _themeService.SetTheme(selectedTheme);

        // Update title bar button colors based on theme
        UpdateTitleBarColors(selectedTheme);
    }

    private void OnThemeServiceThemeChanged(object? sender, AppTheme theme)
    {
        // Update ComboBox if theme changed from outside (e.g., system theme change)
        if (!_isInitializingTheme)
        {
            _isInitializingTheme = true;
            ThemeSelector.SelectedIndex = ThemeService.ToComboBoxIndex(theme);
            _isInitializingTheme = false;
        }
    }

    private void InfoButton_Click(object sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow();
        aboutWindow.Activate();
    }

    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        // Disable button and show progress ring
        RefreshButton.IsEnabled = false;
        RefreshIcon.Visibility = Visibility.Collapsed;
        RefreshProgressRing.Visibility = Visibility.Visible;
        RefreshProgressRing.IsActive = true;

        try
        {
            // Call refresh on the MainPage
            await MainPageContent.RefreshAsync();
        }
        finally
        {
            // Re-enable button and hide progress ring
            RefreshButton.IsEnabled = true;
            RefreshProgressRing.IsActive = false;
            RefreshProgressRing.Visibility = Visibility.Collapsed;
            RefreshIcon.Visibility = Visibility.Visible;
        }
    }

    private void UpdateTitleBarColors(AppTheme theme)
    {
        if (_appWindow is null)
        {
            return;
        }

        var titleBar = _appWindow.TitleBar;

        // Set button colors based on theme
        switch (theme)
        {
            case AppTheme.Inverted:
                // For inverted theme, we need to check what the effective theme will be
                var uiSettings = new UISettings();
                var foreground = uiSettings.GetColorValue(UIColorType.Foreground);
                var isSystemDark = foreground.R > 128 && foreground.G > 128 && foreground.B > 128;
                // Inverted means opposite of system, so if system is dark, we show light (black text)
                if (isSystemDark)
                {
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ButtonHoverForegroundColor = Colors.Black;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(20, 0, 0, 0);
                }
                else
                {
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonHoverForegroundColor = Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(20, 255, 255, 255);
                }
                break;

            case AppTheme.SystemDefault:
            default:
                titleBar.ButtonForegroundColor = null; // System default
                titleBar.ButtonHoverForegroundColor = null;
                titleBar.ButtonHoverBackgroundColor = null;
                break;
        }
    }

    private void AboutButton_Click(object sender, RoutedEventArgs e)
    {
        // Close existing About window if open
        if (_aboutWindow is not null)
        {
            try
            {
                _aboutWindow.Activate();
                return;
            }
            catch
            {
                // Window was closed, create a new one
                _aboutWindow = null;
            }
        }

        _aboutWindow = new AboutWindow();
        _aboutWindow.Closed += (s, args) => _aboutWindow = null;
        _aboutWindow.Activate();
    }
}
