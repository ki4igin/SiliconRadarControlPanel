﻿using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.ViewModels.Base;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public abstract class DDSRegisterViewModel : TitledViewModel
{
    #region NotifyProperty <int> ControlBits
    private int _controlBits;
    public int ControlBits { get => _controlBits; init => Set(ref _controlBits, value); }
    #endregion

    #region NotifyProperty <uint> RegisterValue
    protected uint _registerValue;
    public uint RegisterValue
    {
        get => _registerValue;
        set =>
            SetValue(ref _registerValue, value)
            .Then(() => UpdateBitFields(value));
    }
    #endregion

    public DDSRegisterViewModel()
    {
        _communication = new Communication();
    }
    public DDSRegisterViewModel(Communication communication)
    {
        _communication = communication;
    }

    protected abstract void UpdateRegisterValue();
    protected abstract void UpdateBitFields(uint registerValue);

    #region Command TestCommand
    private SimpleCommand? _testCommand;
    private readonly Communication _communication;

    public SimpleCommand TestCommand => _testCommand ??= new SimpleCommand(
        execute: () => _communication.SendRegisterValue(RegisterValue),
        canExecute: () => _communication.IsConnected
    );
    #endregion

}

