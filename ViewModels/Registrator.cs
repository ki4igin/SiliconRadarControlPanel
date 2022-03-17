using Microsoft.Extensions.DependencyInjection;
using SiliconRadarControlPanel.ViewModels.DDSViewModels;

namespace SiliconRadarControlPanel.ViewModels;

internal static class Registrator
{
    public static void AddViewModels(this IServiceCollection services) =>
        services
        .AddSingleton<MainViewModel>()
        .AddSingleton<DDSViewModel>();
}