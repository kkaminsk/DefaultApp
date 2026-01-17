using DefaultApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DefaultApp.Views;

/// <summary>
/// Main page displaying system information in two cards.
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

        // Dispose ViewModel when page is unloaded
        this.Unloaded += OnUnloaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Load system information
        await ViewModel.LoadDataAsync();
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
