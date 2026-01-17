using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace DefaultApp;

public sealed partial class SplashWindow : Window
{
    public SplashWindow()
    {
        this.InitializeComponent();
        SetWindowSizeAndCenter();
    }

    private void SetWindowSizeAndCenter()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow is not null)
        {
            // 50% of main window width (800 / 2 = 400), height proportional
            appWindow.Resize(new Windows.Graphics.SizeInt32(400, 300));
            CenterOnScreen(hWnd, appWindow);
        }
    }

    private void CenterOnScreen(nint hWnd, AppWindow appWindow)
    {
        var hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
        var monitorInfo = new MONITORINFO { cbSize = (uint)Marshal.SizeOf<MONITORINFO>() };

        if (GetMonitorInfo(hMonitor, ref monitorInfo))
        {
            var workArea = monitorInfo.rcWork;
            var workWidth = workArea.right - workArea.left;
            var workHeight = workArea.bottom - workArea.top;

            var windowSize = appWindow.Size;
            var x = workArea.left + (workWidth - windowSize.Width) / 2;
            var y = workArea.top + (workHeight - windowSize.Height) / 2;

            appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }

    private const int MONITOR_DEFAULTTONEAREST = 2;

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
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
    private static extern nint MonitorFromWindow(nint hWnd, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(nint hMonitor, ref MONITORINFO lpmi);
}
