using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Serilog;

namespace SiliconRadarControlPanel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/myapp.txt")
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Start sesion");
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        Log.Information("Close sesion");
        Log.CloseAndFlush();
    }
}


