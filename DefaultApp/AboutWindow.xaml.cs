using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel;
using WinRT.Interop;

namespace DefaultApp;

/// <summary>
/// About window displaying application version and release information.
/// </summary>
public sealed partial class AboutWindow : Window
{
    private AppWindow? _appWindow;
    private const int WindowWidth = 400;
    private const int WindowHeight = 450;

    public AboutWindow()
    {
        this.InitializeComponent();

        ConfigureWindow();
        SetVersionText();
    }

    private void ConfigureWindow()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        _appWindow = AppWindow.GetFromWindowId(windowId);

        if (_appWindow is not null)
        {
            // Configure as a fixed-size dialog
            if (_appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMinimizable = false;
                presenter.IsMaximizable = false;
            }

            // Set fixed size
            _appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));

            // Center on screen
            CenterOnScreen();
        }
    }

    private void CenterOnScreen()
    {
        if (_appWindow is null)
        {
            return;
        }

        var displayArea = DisplayArea.GetFromWindowId(
            _appWindow.Id,
            DisplayAreaFallback.Primary);

        if (displayArea is not null)
        {
            var workArea = displayArea.WorkArea;
            var x = (workArea.Width - WindowWidth) / 2 + workArea.X;
            var y = (workArea.Height - WindowHeight) / 2 + workArea.Y;
            _appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }

    private void SetVersionText()
    {
        try
        {
            var version = Package.Current.Id.Version;
            VersionText.Text = $"Version {version.Major}.{version.Minor}";
        }
        catch
        {
            // If not running as a packaged app, use default
            VersionText.Text = "Version 1.1";
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
