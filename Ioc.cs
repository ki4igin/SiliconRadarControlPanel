using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Settings;
using SiliconRadarControlPanel.ViewModels;

namespace SiliconRadarControlPanel;

public static class Ioc
{
    public const string SettingsFileName = "settings.json";

    public static T Resolve<T>() where T : notnull => _provider!.GetRequiredService<T>();
    
    public static StatusSink Status { get; set; } = new();


    private static readonly ServiceProvider? _provider;

    static Ioc()
    {
        ConfigurationBuilder config = new();
        config.Sources.Clear();
        config.AddJsonFile(SettingsFileName, true);
        IConfigurationRoot configuration = config.Build();
        
        ServiceCollection services = new();
        services.Configure<DDSSettings>(configuration.GetSection(nameof(DDSSettings)));
        services.Configure<ComPortSettings>(configuration.GetSection(nameof(ComPortSettings)));
        services.AddServices();
        services.AddViewModels();

        _provider = services.BuildServiceProvider();
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.StatusSink(Status)
            .WriteTo.File("logs/myapp.txt")
            .CreateLogger();
        
        Log.Information("Start session");
    }

}
