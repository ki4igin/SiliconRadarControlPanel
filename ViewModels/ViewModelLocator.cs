using Microsoft.Extensions.DependencyInjection;
using SiliconRadarControlPanel.ViewModels.DDSViewModels;

namespace SiliconRadarControlPanel.ViewModels;

internal class ViewModelLocator
{
    public static MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();
    public static DDSViewModel DDSViewModel => App.Services.GetRequiredService<DDSViewModel>();
}
