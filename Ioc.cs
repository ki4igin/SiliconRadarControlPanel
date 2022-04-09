using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Settings;
using SiliconRadarControlPanel.ViewModels;
using System;

namespace SiliconRadarControlPanel;

public static class Ioc
{
    public const string SettingsFileName = "settings.json";

    private static IHost? _hosting;
    public static T Resolve<T>() where T : notnull => Hosting.Services.GetRequiredService<T>();

    public static StatusSink Status { get; set; } = new();
    public static IHost Hosting => _hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.Sources.Clear();
                config.AddJsonFile("settings.json", true);
            })
            .ConfigureLoggingSerilog()
            .ConfigureServices(ConfigureServices);

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
    {
        services.Configure<DDSSettings>(host.Configuration.GetSection(nameof(DDSSettings)));
        services.Configure<ComPortSettings>(host.Configuration.GetSection(nameof(ComPortSettings)));
        services.AddServices();
        services.AddViewModels();
    }

    private static IHostBuilder ConfigureLoggingSerilog(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.StatusSink(Status)
            .WriteTo.File("logs/myapp.txt")
            .CreateLogger();
        return hostBuilder;
    }
}
