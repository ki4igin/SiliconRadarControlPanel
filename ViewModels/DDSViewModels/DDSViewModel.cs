using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.Infrastructure;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.ViewModels.Base;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSViewModel : TitledViewModel, IDisposable
{
    public DDSR0ViewModel DDSR0ViewModel { get; init; }
    public DDSR1ViewModel DDSR1ViewModel { get; init; }
    public DDSR2ViewModel DDSR2ViewModel { get; init; }
    public DDSR3ViewModel DDSR3ViewModel { get; init; }
    public DDSR4ViewModel DDSR4ViewModel { get; init; }
    public DDSR5ViewModel DDSR5ViewModel { get; init; }
    public DDSR6ViewModel DDSR6ViewModel { get; init; }
    public DDSR7ViewModel DDSR7ViewModel { get; init; }

    //public DDSViewModel() : this(new Communication()) { }

    public DDSViewModel(Communication communication)
    {
        DDSModel ddsModel = new(0, 0, 0, 0, 0, 0, 0, 0);
        ddsModel.Read();
        DDSR0ViewModel = new DDSR0ViewModel(communication) { RegisterValue = ddsModel.R0 };
        DDSR1ViewModel = new DDSR1ViewModel(communication) { RegisterValue = ddsModel.R1 };
        DDSR2ViewModel = new DDSR2ViewModel(communication) { RegisterValue = ddsModel.R2 };
        DDSR3ViewModel = new DDSR3ViewModel(communication) { RegisterValue = ddsModel.R3 };
        DDSR4ViewModel = new DDSR4ViewModel(communication) { RegisterValue = ddsModel.R4 };
        DDSR5ViewModel = new DDSR5ViewModel(communication) { RegisterValue = ddsModel.R5 };
        DDSR6ViewModel = new DDSR6ViewModel(communication) { RegisterValue = ddsModel.R6 };
        DDSR7ViewModel = new DDSR7ViewModel(communication) { RegisterValue = ddsModel.R7 };
    }

    public record DDSModel(uint R0, uint R1, uint R2, uint R3, uint R4, uint R5, uint R6, uint R7) : ISettings;

    public void Dispose()
    {
        DDSModel ddsModel = new(
            DDSR0ViewModel.RegisterValue,
            DDSR1ViewModel.RegisterValue,
            DDSR2ViewModel.RegisterValue,
            DDSR3ViewModel.RegisterValue,
            DDSR4ViewModel.RegisterValue,
            DDSR5ViewModel.RegisterValue,
            DDSR6ViewModel.RegisterValue,
            DDSR7ViewModel.RegisterValue
        );
        ddsModel.Save();
    }
}