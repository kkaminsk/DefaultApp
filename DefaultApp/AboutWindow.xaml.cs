using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace DefaultApp;

/// <summary>
/// About window displaying application information.
/// </summary>
public sealed partial class AboutWindow : Window
{
    public AboutWindow()
    {
        this.InitializeComponent();
        SetWindowSizeAndPosition();
    }

    private void SetWindowSizeAndPosition()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow is not null)
        {
            appWindow.Resize(new Windows.Graphics.SizeInt32(400, 320));
            CenterOverMainWindow(appWindow);
        }
    }

    private void CenterOverMainWindow(AppWindow appWindow)
    {
        var app = Application.Current as App;
        var mainWindow = app?.MainWindow;

        if (mainWindow is null)
        {
            return;
        }

        var mainHwnd = WindowNative.GetWindowHandle(mainWindow);
        var mainWindowId = Win32Interop.GetWindowIdFromWindow(mainHwnd);
        var mainAppWindow = AppWindow.GetFromWindowId(mainWindowId);

        if (mainAppWindow is not null)
        {
            var mainPos = mainAppWindow.Position;
            var mainSize = mainAppWindow.Size;
            var aboutSize = appWindow.Size;

            var x = mainPos.X + (mainSize.Width - aboutSize.Width) / 2;
            var y = mainPos.Y + (mainSize.Height - aboutSize.Height) / 2;

            appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
