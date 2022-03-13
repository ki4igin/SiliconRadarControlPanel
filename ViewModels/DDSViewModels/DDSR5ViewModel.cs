using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR5ViewModel : DDSRegisterViewModel
{

    #region NotifyProperty <bool> TXdataInvert
    private bool _txdataInvert;
    public bool TXdataInvert
    {
        get => _txdataInvert;
        set =>
            SetValue(ref _txdataInvert, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _txRampClk = { "CLK DIV", "Txdata" };
    public string[] TXrampClk { get => _txRampClk; }

    #region NotifyProperty <int> TXrampClkSelectIndex
    private int _txRampClkSelectIndex = 0;
    public int TXrampClkSelectIndex
    {
        get => _txRampClkSelectIndex;
        set =>
            SetValue(ref _txRampClkSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> FSKRamp
    private bool _parabolicRamp;
    public bool ParabolicRamp
    {
        get => _parabolicRamp;
        set =>
            SetValue(ref _parabolicRamp, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _interrupt = {
        "INTERRUPT OFF",
        "LOAD CHANNEL CONTINUE SWEEP",
        "NOT USED",
        "LOAD CHANNEL STOP SWEEP"
    };
    public string[] Interrupt { get => _interrupt; }

    #region NotifyProperty <int> InterruptSelectIndex
    private int _interruptSelectIndex = 0;
    public int InterruptSelectIndex
    {
        get => _interruptSelectIndex;
        set =>
            SetValue(ref _interruptSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> FSKRamp
    private bool fskRamp;
    public bool FSKRamp
    {
        get => fskRamp;
        set =>
            SetValue(ref fskRamp, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> DualRamp
    private bool _dualRamp;
    public bool DualRamp
    {
        get => _dualRamp;
        set =>
            SetValue(ref _dualRamp, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _devSel = { "1", "2" };
    public string[] DevSel { get => _devSel; }

    #region NotifyProperty <int> DevSelSelectIndex
    private int _devSelSelectIndex = 0;
    public int DevSelSelectIndex
    {
        get => _devSelSelectIndex;
        set =>
            SetValue(ref _devSelSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> DeviationOffsetWord
    private int _deviationOffsetWord;
    public int DeviationOffsetWord
    {
        get => _deviationOffsetWord;
        set =>
            SetValueIf(ref _deviationOffsetWord, value, (v) => v is >= 0 and < 10)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> DeviationWord
    private int _deviationWord;
    public int DeviationWord
    {
        get => _deviationWord;
        set =>
            SetValueIf(ref _deviationWord, value, (v) => v is >= -(1 << 15) and < (1 << 15))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public DDSR5ViewModel()
    {
        ControlBits = 5;
        Title = "DEVIATION REGISTER (R5)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue =
            (Convert.ToInt32(TXdataInvert) << 30) |
            (TXrampClkSelectIndex << 29) |
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
        TXdataInvert = Register.IsBitSet(registerValue, 30);
        TXrampClkSelectIndex = Register.GetBitFiled(registerValue, 29, 1);
        ParabolicRamp = Register.IsBitSet(registerValue, 28);
        InterruptSelectIndex = Register.GetBitFiled(registerValue, 26, 2);
        FSKRamp = Register.IsBitSet(registerValue, 25);
        DualRamp = Register.IsBitSet(registerValue, 24);
        DevSelSelectIndex = Register.GetBitFiled(registerValue, 23, 1);
        DeviationWord = Register.GetBitFiled(registerValue, 19, 4);
        var temp = Register.GetBitFiled(registerValue, 3, 16);
        DeviationWord = temp switch
        {
            < (2 << 15) => temp,
            _ => temp - (2 << 16)
        };
    }
}
