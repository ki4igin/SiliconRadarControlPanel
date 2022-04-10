using System.Windows;
using Serilog;

namespace SiliconRadarControlPanel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // var host = Ioc.Hosting;
        // await host.StartAsync();
        Ioc.Init();

        Log.Information("Start session");
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        // using var host = Ioc.Hosting;
        // await host.StopAsync();

        Log.Information("Close session");
        Log.CloseAndFlush();
    }
}