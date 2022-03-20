using Microsoft.Extensions.DependencyInjection;

namespace SiliconRadarControlPanel.Services;

internal static class Registrator
{
    public static void AddServices(this IServiceCollection services) =>
        services
        .AddSingleton<Communication>()
    ;
}
