using Microsoft.Extensions.Options;
using SiliconRadarControlPanel.Infrastructure;
using SiliconRadarControlPanel.Settings;
using SiliconRadarControlPanel.ViewModels.Base;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSViewModel : TitledViewModel, IDisposable
{
    private readonly DDSSettings _ddsSettings;

    public DDSR0ViewModel DDSR0ViewModel { get; init; }
    public DDSR1ViewModel DDSR1ViewModel { get; init; }
    public DDSR2ViewModel DDSR2ViewModel { get; init; }
    public DDSR3ViewModel DDSR3ViewModel { get; init; }
    public DDSR4ViewModel DDSR4ViewModel { get; init; }
    public DDSR5ViewModel DDSR5ViewModel { get; init; }
    public DDSR6ViewModel DDSR6ViewModel { get; init; }
    public DDSR7ViewModel DDSR7ViewModel { get; init; }

    public DDSViewModel() :
        this(new OptionsWrapper<DDSSettings>(new DDSSettings()))
    {
    }

    public DDSViewModel(IOptions<DDSSettings> options)
    {
        _ddsSettings = options.Value;
        DDSR0ViewModel = new DDSR0ViewModel { RegisterValue = _ddsSettings.R0 };
        DDSR1ViewModel = new DDSR1ViewModel { RegisterValue = _ddsSettings.R1 };
        DDSR2ViewModel = new DDSR2ViewModel { RegisterValue = _ddsSettings.R2 };
        DDSR3ViewModel = new DDSR3ViewModel { RegisterValue = _ddsSettings.R3 };
        DDSR4ViewModel = new DDSR4ViewModel { RegisterValue = _ddsSettings.R4 };
        DDSR5ViewModel = new DDSR5ViewModel { RegisterValue = _ddsSettings.R5 };
        DDSR6ViewModel = new DDSR6ViewModel { RegisterValue = _ddsSettings.R6 };
        DDSR7ViewModel = new DDSR7ViewModel { RegisterValue = _ddsSettings.R7 };
    }

    public void Dispose()
    {
        _ddsSettings.R0 = DDSR0ViewModel.RegisterValue;
        _ddsSettings.R1 = DDSR1ViewModel.RegisterValue;
        _ddsSettings.R2 = DDSR2ViewModel.RegisterValue;
        _ddsSettings.R3 = DDSR3ViewModel.RegisterValue;
        _ddsSettings.R4 = DDSR4ViewModel.RegisterValue;
        _ddsSettings.R5 = DDSR5ViewModel.RegisterValue;
        _ddsSettings.R6 = DDSR6ViewModel.RegisterValue;
        _ddsSettings.R7 = DDSR7ViewModel.RegisterValue;

        _ddsSettings.SaveToFile();
        
        GC.SuppressFinalize(this);
    }
}