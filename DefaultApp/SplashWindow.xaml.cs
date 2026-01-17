using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using WinRT.Interop;

namespace DefaultApp;

/// <summary>
/// Splash screen window displayed during application startup.
/// </summary>
public sealed partial class SplashWindow : Window
{
    private AppWindow? _appWindow;
    private readonly TaskCompletionSource<bool> _appReady = new();
    private readonly TaskCompletionSource<bool> _splashComplete = new();
    private const int SplashWidth = 500;
    private const int SplashHeight = 400;
    private const int MinDisplayTimeMs = 2000;

    public SplashWindow()
    {
        this.InitializeComponent();

        ConfigureWindow();
        SetVersionText();
        _ = RunSplashSequenceAsync();
    }

    /// <summary>
    /// Gets a task that completes when the splash screen animation is fully complete
    /// and the window is ready to be closed.
    /// </summary>
    public Task SplashCompleteTask => _splashComplete.Task;

    /// <summary>
    /// Signals that the app initialization is complete.
    /// The splash will still wait for minimum display time and animations.
    /// </summary>
    public void SignalAppReady()
    {
        _appReady.TrySetResult(true);
    }

    /// <summary>
    /// Updates the loading status text.
    /// </summary>
    public void SetLoadingStatus(string status)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            LoadingText.Text = status;
        });
    }

    private void ConfigureWindow()
    {
        var hWnd = WindowNative.GetWindowHandle(this);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        _appWindow = AppWindow.GetFromWindowId(windowId);

        if (_appWindow is not null)
        {
            // Hide title bar for clean splash appearance
            if (_appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMinimizable = false;
                presenter.IsMaximizable = false;
                presenter.SetBorderAndTitleBar(false, false);
            }

            // Set fixed size
            _appWindow.Resize(new Windows.Graphics.SizeInt32(SplashWidth, SplashHeight));

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
            var x = (workArea.Width - SplashWidth) / 2 + workArea.X;
            var y = (workArea.Height - SplashHeight) / 2 + workArea.Y;
            _appWindow.Move(new Windows.Graphics.PointInt32(x, y));
        }
    }

    private void SetVersionText()
    {
        try
        {
            var version = Package.Current.Id.Version;
            VersionText.Text = $"Version {version.Major}.{version.Minor}.{version.Build}";
        }
        catch
        {
            // If not running as a packaged app, use a default version
            VersionText.Text = "Version 1.0.0";
        }
    }

    private async Task RunSplashSequenceAsync()
    {
        var startTime = DateTime.UtcNow;

        // Small delay before starting animations
        await Task.Delay(100);

        // Animate content panel fade in
        AnimateOpacity(ContentPanel, 0, 1, TimeSpan.FromMilliseconds(400));

        await Task.Delay(200);

        // Animate loading panel fade in
        AnimateOpacity(LoadingPanel, 0, 1, TimeSpan.FromMilliseconds(300));

        await Task.Delay(100);

        // Animate version text fade in
        AnimateOpacity(VersionText, 0, 1, TimeSpan.FromMilliseconds(300));

        // Wait for app to signal it's ready
        await _appReady.Task;

        // Ensure minimum display time has elapsed
        var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
        if (elapsed < MinDisplayTimeMs)
        {
            await Task.Delay((int)(MinDisplayTimeMs - elapsed));
        }

        // Fade out before closing
        await FadeOutAsync();

        // Signal that splash is complete and can be closed
        _splashComplete.TrySetResult(true);
    }

    private async Task FadeOutAsync()
    {
        AnimateOpacity(RootGrid, 1, 0, TimeSpan.FromMilliseconds(200));
        await Task.Delay(200);
    }

    private void AnimateOpacity(UIElement element, double from, double to, TimeSpan duration)
    {
        var animation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = new Duration(duration),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, "Opacity");
        storyboard.Begin();
    }
}
