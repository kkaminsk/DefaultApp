using DefaultApp.ViewModels;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;

namespace DefaultApp.Views;

/// <summary>
/// Main page displaying system information in two cards.
/// </summary>
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }
    private ScalarKeyFrameAnimation? _shimmerAnimation;

    public MainPage()
    {
        ViewModel = new MainViewModel();
        this.InitializeComponent();

        // Register for size changed to handle responsive layout
        this.SizeChanged += OnSizeChanged;

        // Load data when page is loaded
        this.Loaded += OnLoaded;

        // Dispose ViewModel when page is unloaded
        this.Unloaded += OnUnloaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Start shimmer animation on skeleton
        StartShimmerAnimation();

        // Load system information
        await ViewModel.LoadDataAsync();

        // Stop shimmer animation when loading completes
        StopShimmerAnimation();
    }

    private void StartShimmerAnimation()
    {
        // Check if Windows reduced motion setting is enabled
        var uiSettings = new Windows.UI.ViewManagement.UISettings();
        if (!uiSettings.AnimationsEnabled)
        {
            return;
        }

        var compositor = ElementCompositionPreview.GetElementVisual(SkeletonContainer).Compositor;

        // Create opacity pulsing animation for a subtle shimmer effect
        _shimmerAnimation = compositor.CreateScalarKeyFrameAnimation();
        _shimmerAnimation.InsertKeyFrame(0f, 0.4f);
        _shimmerAnimation.InsertKeyFrame(0.5f, 0.8f);
        _shimmerAnimation.InsertKeyFrame(1f, 0.4f);
        _shimmerAnimation.Duration = TimeSpan.FromSeconds(1.5);
        _shimmerAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

        var visual = ElementCompositionPreview.GetElementVisual(SkeletonContainer);
        visual.StartAnimation("Opacity", _shimmerAnimation);
    }

    private void StopShimmerAnimation()
    {
        if (_shimmerAnimation != null)
        {
            var visual = ElementCompositionPreview.GetElementVisual(SkeletonContainer);
            visual.StopAnimation("Opacity");
            visual.Opacity = 1f;
            _shimmerAnimation = null;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        // Dispose ViewModel to release resources (e.g., MediaPlayer)
        ViewModel.Dispose();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateLayout(e.NewSize.Width);
    }

    private void UpdateLayout(double width)
    {
        // Find the CardsPanel and update its orientation based on width
        if (CardsContainer.ItemsPanelRoot is StackPanel panel)
        {
            panel.Orientation = width >= 1200 ? Orientation.Horizontal : Orientation.Vertical;
        }
    }

    /// <summary>
    /// Refreshes the page data. Called from the title bar refresh button.
    /// </summary>
    public async Task RefreshAsync()
    {
        if (ViewModel.RefreshCommand.CanExecute(null))
        {
            await ViewModel.RefreshCommand.ExecuteAsync(null);
        }
    }

    /// <summary>
    /// Gets whether a refresh is currently in progress.
    /// </summary>
    public bool IsRefreshing => ViewModel.IsRefreshing;
}
