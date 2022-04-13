using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR5ViewModel : DDSRegisterViewModel
{
    #region NotifyProperty <bool> TXdataInvert

    private bool _txDataInvert;

    public bool TxDataInvert
    {
        get => _txDataInvert;
        set =>
            SetValue(ref _txDataInvert, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    public string[] TxRampClk { get; } = { "CLK DIV", "Txdata" };

    #region NotifyProperty <int> TXrampClkSelectIndex

    private int _txRampClkSelectIndex;

    public int TxRampClkSelectIndex
    {
        get => _txRampClkSelectIndex;
        set =>
            SetValue(ref _txRampClkSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> FSKRamp

    private bool _parabolicRamp;

    public bool ParabolicRamp
    {
        get => _parabolicRamp;
        set =>
            SetValue(ref _parabolicRamp, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    public string[] Interrupt { get; } =
    {
        "INTERRUPT OFF",
        "LOAD CHANNEL CONTINUE SWEEP",
        "NOT USED",
        "LOAD CHANNEL STOP SWEEP"
    };

    #region NotifyProperty <int> InterruptSelectIndex

    private int _interruptSelectIndex;

    public int InterruptSelectIndex
    {
        get => _interruptSelectIndex;
        set =>
            SetValue(ref _interruptSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> FSKRamp

    private bool _fskRamp;

    public bool FSKRamp
    {
        get => _fskRamp;
        set =>
            SetValue(ref _fskRamp, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> DualRamp

    private bool _dualRamp;

    public bool DualRamp
    {
        get => _dualRamp;
        set =>
            SetValue(ref _dualRamp, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    public string[] DevSel { get; } = { "1", "2" };

    #region NotifyProperty <int> DevSelSelectIndex

    private int _devSelSelectIndex;

    public int DevSelSelectIndex
    {
        get => _devSelSelectIndex;
        set =>
            SetValue(ref _devSelSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <int> DeviationOffsetWord

    private int _deviationOffsetWord;

    public int DeviationOffsetWord
    {
        get => _deviationOffsetWord;
        set =>
            SetValueIf(ref _deviationOffsetWord, value, v => v is >= 0 and < 10)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <int> DeviationWord

    private int _deviationWord;

    public int DeviationWord
    {
        get => _deviationWord;
        set =>
            SetValueIf(ref _deviationWord, value, v => v is >= -(1 << 15) and < (1 << 15))
                .Then(UpdateRegisterValue);
    }

    #endregion

    public DDSR5ViewModel()
    {
        ControlBits = 5;
        Title = "DEVIATION REGISTER (R5)";
    }

    protected virtual void UpdateRegisterValue()
    {
        int newValue =
            (Convert.ToInt32(TxDataInvert) << 30) |
            (TxRampClkSelectIndex << 29) |
            (Convert.ToInt32(ParabolicRamp) << 28) |
            (InterruptSelectIndex << 26) |
            (Convert.ToInt32(FSKRamp) << 25) |
            (Convert.ToInt32(DualRamp) << 24) |
            (DevSelSelectIndex << 23) |
            (DeviationOffsetWord << 19) |
            ControlBits;
        newValue = (int)Register.SetBitFiled((uint)newValue, DeviationWord, 3, 16);
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        TxDataInvert = Register.IsBitSet(registerValue, 30);
        TxRampClkSelectIndex = Register.GetBitFiled(registerValue, 29, 1);
        ParabolicRamp = Register.IsBitSet(registerValue, 28);
        InterruptSelectIndex = Register.GetBitFiled(registerValue, 26, 2);
        FSKRamp = Register.IsBitSet(registerValue, 25);
        DualRamp = Register.IsBitSet(registerValue, 24);
        DevSelSelectIndex = Register.GetBitFiled(registerValue, 23, 1);
        DeviationWord = Register.GetBitFiled(registerValue, 19, 4);
        int temp = Register.GetBitFiled(registerValue, 3, 16);
        DeviationWord = temp switch
        {
            < (1 << 15) => temp,
            _ => temp - (1 << 16)
        };
    }
}