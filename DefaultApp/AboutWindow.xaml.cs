using Microsoft.UI.Xaml;

namespace DefaultApp;

/// <summary>
/// About window displaying application information.
/// </summary>
public sealed partial class AboutWindow : Window
{
    public AboutWindow()
    {
        this.InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
