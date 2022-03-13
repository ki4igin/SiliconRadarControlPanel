using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR7ViewModel : DDSRegisterViewModel
{

    #region NotifyProperty <bool> TXdataTriggerDelay;
    private bool _txdataTriggerDelay;
    public bool TXdataTriggerDelay
    {
        get => _txdataTriggerDelay;
        set =>
            SetValue(ref _txdataTriggerDelay, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> TriDelay;
    private bool _triDelay;
    public bool TriDelay
    {
        get => _triDelay;
        set =>
            SetValue(ref _triDelay, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> SingFullTri;
    private bool _singFullTri;
    public bool SingFullTri
    {
        get => _singFullTri;
        set =>
            SetValue(ref _singFullTri, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> TXdataTrigger;
    private bool _txdataTrigger;
    public bool TXdataTrigger
    {
        get => _txdataTrigger;
        set =>
            SetValue(ref _txdataTrigger, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> FastRamp;
    private bool _fastRamp;
    public bool FastRamp
    {
        get => _fastRamp;
        set =>
            SetValue(ref _fastRamp, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> RampDelayFL;
    private bool _rampDelayFL;
    public bool RampDelayFL
    {
        get => _rampDelayFL;
        set =>
            SetValue(ref _rampDelayFL, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> RampDelay;
    private bool _rampDelay;
    public bool RampDelay
    {
        get => _rampDelay;
        set =>
            SetValue(ref _rampDelay, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _devClkSel = { "PFD CLK", "PFD CLK × CLK" };
    public string[] DevClkSel { get => _devClkSel; }

    #region NotifyProperty <int> DelClkSelSelectIndex
    private int _devClkSelSelectIndex = 0;
    public int DevClkSelSelectIndex
    {
        get => _devClkSelSelectIndex;
        set =>
            SetValue(ref _devClkSelSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> DelStartEn;
    private bool _delStartEn;
    public bool DelStartEn
    {
        get => _delStartEn;
        set =>
            SetValue(ref _delStartEn, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> DelayStartWord
    private int _delayStartWord = 0;
    public int DelayStartWord
    {
        get => _delayStartWord;
        set =>
            SetValueIf(ref _delayStartWord, value, (v) => v is >= 0 and < (2 << 12))
            .Then(() => UpdateRegisterValue());
    }
    #endregion  

    public DDSR7ViewModel()
    {
        ControlBits = 6;
        Title = "STEP REGISTER (R6)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue =
            (Convert.ToInt32(TXdataTriggerDelay) << 23) |
            (Convert.ToInt32(TriDelay) << 22) |
            (Convert.ToInt32(SingFullTri) << 21) |
            (Convert.ToInt32(TXdataTrigger) << 20) |
            (Convert.ToInt32(FastRamp) << 19) |
            (Convert.ToInt32(RampDelayFL) << 18) |
            (Convert.ToInt32(RampDelay) << 17) |
            (DevClkSelSelectIndex << 16) |
            (Convert.ToInt32(DelStartEn) << 15) |
            (DelayStartWord << 3) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        TXdataTriggerDelay = Register.IsBitSet(registerValue, 23);
        TriDelay = Register.IsBitSet(registerValue, 22);
        SingFullTri = Register.IsBitSet(registerValue, 21);
        TXdataTrigger = Register.IsBitSet(registerValue, 20);
        FastRamp = Register.IsBitSet(registerValue, 19);
        RampDelayFL = Register.IsBitSet(registerValue, 18);
        RampDelay = Register.IsBitSet(registerValue, 17);
        DevClkSelSelectIndex = Register.GetBitFiled(registerValue, 16, 1);
        DelStartEn  = Register.IsBitSet(registerValue, 15);
        DelayStartWord = Register.GetBitFiled(registerValue, 3, 12);
    }
}