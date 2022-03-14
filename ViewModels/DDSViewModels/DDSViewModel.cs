using SiliconRadarControlPanel.ViewModels.Base;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSViewModel : TitledViewModel
{
    public DDSR0ViewModel DDSR0ViewModel { get; set; }

    public DDSViewModel()
    {
        DDSR0ViewModel = new DDSR0ViewModel();
    }
}