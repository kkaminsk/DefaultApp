#if DEBUG
using DefaultApp.Services;
using DefaultApp.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DefaultApp.Views;

/// <summary>
/// Debug page with development tools and mock data options.
/// Only available in DEBUG builds.
/// </summary>
public sealed partial class DebugPage : Page
{
    public DebugViewModel ViewModel { get; }

    /// <summary>
    /// Event raised when the debug panel should be closed.
    /// </summary>
    public event EventHandler? CloseRequested;

    public DebugPage()
    {
        ViewModel = new DebugViewModel();
        this.InitializeComponent();
        LoadBuildInfo();
    }

    private void LoadBuildInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version;

        BuildConfigText.Text = "Configuration: DEBUG";
        BuildVersionText.Text = $"Version: {version}";
        RuntimeText.Text = $"Runtime: {RuntimeInformation.FrameworkDescription}";

        // Get build time from assembly
        var buildDate = GetBuildDate(assembly);
        BuildTimeText.Text = buildDate.HasValue
            ? $"Build Time: {buildDate.Value:yyyy-MM-dd HH:mm:ss}"
            : "Build Time: Unknown";

        // Show log path
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DefaultApp", "Logs");
        LogPathText.Text = $"Log Path: {logPath}";
    }

    private static DateTime? GetBuildDate(Assembly assembly)
    {
        try
        {
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion is not null)
            {
                // Try to parse version info for build date
                return DateTime.Now; // Fallback to current time
            }
        }
        catch
        {
            // Ignore errors
        }
        return null;
    }

    private void UseMockDataToggle_Toggled(object sender, RoutedEventArgs e)
    {
        App.Logger?.WriteLog(LogLevel.Debug, "DebugPage",
            $"Mock data toggled: {ViewModel.UseMockData}");
    }

    private void OpenLogFolderButton_Click(object sender, RoutedEventArgs e)
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DefaultApp", "Logs");

        if (Directory.Exists(logPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = logPath,
                UseShellExecute = true
            });
        }
        else
        {
            App.Logger?.WriteLog(LogLevel.Warning, "DebugPage",
                $"Log folder does not exist: {logPath}");
        }
    }

    private void LogLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var level = LogLevelComboBox.SelectedIndex switch
        {
            0 => LogLevel.Trace,
            1 => LogLevel.Debug,
            2 => LogLevel.Information,
            3 => LogLevel.Warning,
            4 => LogLevel.Error,
            _ => LogLevel.Debug
        };

        App.Logger?.WriteLog(LogLevel.Information, "DebugPage",
            $"Log level changed to: {level}");
    }

    private void WriteTestLogButton_Click(object sender, RoutedEventArgs e)
    {
        App.Logger?.WriteLog(LogLevel.Information, "DebugPage",
            "Test log entry written from debug panel");
        App.Logger?.WriteLog(LogLevel.Debug, "DebugPage",
            "Debug level test entry");
        App.Logger?.WriteLog(LogLevel.Warning, "DebugPage",
            "Warning level test entry");
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
#endif
