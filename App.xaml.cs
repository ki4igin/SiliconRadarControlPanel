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
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var host = Ioc.Hosting;
        await host.StartAsync();

        Log.Information("Start session");
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        using var host = Ioc.Hosting;
        await host.StopAsync();

        Log.Information("Close session");
        Log.CloseAndFlush();
    }
}