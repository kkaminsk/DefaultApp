using DefaultApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DefaultApp.Views;

/// <summary>
/// Main page displaying system information in three cards.
/// </summary>
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = new MainViewModel();
        this.InitializeComponent();

        // Register for size changed to handle responsive layout
        this.SizeChanged += OnSizeChanged;

        // Load data when page is loaded
        this.Loaded += OnLoaded;

        // Clean up when page is unloaded
        this.Unloaded += OnUnloaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Load system information
        await ViewModel.LoadDataAsync();

        // Initialize the service controller
        await ViewModel.InitializeServiceControllerAsync(DispatcherQueue);

        // Set up status indicator color updates
        UpdateStatusIndicatorColor();

        // Auto-start the default service (Background Task)
        await ViewModel.AutoStartServiceAsync();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        // Clean up the ViewModel and stop services
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
    /// Updates the status indicator color based on the current service status.
    /// </summary>
    private void UpdateStatusIndicatorColor()
    {
        // Subscribe to property changes to update the status indicator
        ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ViewModel.ServiceStatusColor))
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    StatusIndicator.Fill = ViewModel.ServiceStatusColor switch
                    {
                        "Green" => (SolidColorBrush)Resources["StatusRunningBrush"],
                        "Yellow" => (SolidColorBrush)Resources["StatusStartingBrush"],
                        "Red" => (SolidColorBrush)Resources["StatusErrorBrush"],
                        _ => (SolidColorBrush)Resources["StatusStoppedBrush"]
                    };
                });
            }
        };
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
