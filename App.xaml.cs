using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.ViewModels;

namespace SiliconRadarControlPanel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static IHost? _hosting;
    public static IHost Hosting => _hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    public static IServiceProvider Services => Hosting.Services;

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.AddServices();
        services.AddViewModels();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var host = Hosting;

        await host.StartAsync();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/myapp.txt")
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Start sesion");
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        using var host = Hosting;
        await host.StopAsync();

        Log.Information("Close sesion");
        Log.CloseAndFlush();
    }
}
