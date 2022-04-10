using SiliconRadarControlPanel.ViewModels.DDSViewModels;

namespace SiliconRadarControlPanel.ViewModels;

internal class ViewModelLocator
{
    public static MainViewModel MainViewModel => Ioc.Resolve<MainViewModel>();
    public static DDSViewModel DDSViewModel => Ioc.Resolve<DDSViewModel>();
}
