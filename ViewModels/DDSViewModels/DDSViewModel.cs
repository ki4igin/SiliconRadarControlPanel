using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.ViewModels.Base;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSViewModel : TitledViewModel
{
    private readonly Communication _communication;

    public DDSR0ViewModel DDSR0ViewModel { get; init; }
    public DDSR1ViewModel DDSR1ViewModel { get; init; }
    public DDSR2ViewModel DDSR2ViewModel { get; init; }
    public DDSR3ViewModel DDSR3ViewModel { get; init; }
    public DDSR4ViewModel DDSR4ViewModel { get; init; }
    public DDSR5ViewModel DDSR5ViewModel { get; init; }
    public DDSR6ViewModel DDSR6ViewModel { get; init; }
    public DDSR7ViewModel DDSR7ViewModel { get; init; }

    public DDSViewModel() : this(new Communication()) { }

    public DDSViewModel(Communication communication)
    {
        DDSR0ViewModel = new DDSR0ViewModel(communication);
        DDSR1ViewModel = new DDSR1ViewModel(communication);
        DDSR2ViewModel = new DDSR2ViewModel(communication);
        DDSR3ViewModel = new DDSR3ViewModel(communication);
        DDSR4ViewModel = new DDSR4ViewModel(communication);
        DDSR5ViewModel = new DDSR5ViewModel(communication);
        DDSR6ViewModel = new DDSR6ViewModel(communication);
        DDSR7ViewModel = new DDSR7ViewModel(communication);
        _communication = communication;
    }
}