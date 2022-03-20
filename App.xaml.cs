using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Settings;
using SiliconRadarControlPanel.ViewModels;

namespace SiliconRadarControlPanel;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public const string SettingsFileName = "settings.json";

    private static IHost? _hosting;
    private static IHost Hosting => _hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    public static IServiceProvider Services => Hosting.Services;

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("settings.json");
            })
            .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.Configure<DDSSettings>(host.Configuration.GetSection(nameof(DDSSettings)));
        services.Configure<ComPortSettings>(host.Configuration.GetSection(nameof(ComPortSettings)));
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

        Log.Information("Start session");
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        using var host = Hosting;
        await host.StopAsync();

        Log.Information("Close session");
        Log.CloseAndFlush();
    }
}