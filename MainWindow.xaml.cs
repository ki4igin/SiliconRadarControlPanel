using System;
using Serilog;
using Serilog.Sinks.RichTextBox.Themes;

namespace SiliconRadarControlPanel;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Ioc.Status.Status = Status;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        App.Current.Shutdown();
    }
}