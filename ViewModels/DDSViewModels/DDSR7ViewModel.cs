using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR7ViewModel : DDSRegisterViewModel
{
    #region NotifyProperty <bool> TXdataTriggerDelay;

    private bool _txDataTriggerDelay;

    public bool TxDataTriggerDelay
    {
        get => _txDataTriggerDelay;
        set =>
            SetValue(ref _txDataTriggerDelay, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> TriDelay;

    private bool _triDelay;

    public bool TriDelay
    {
        get => _triDelay;
        set =>
            SetValue(ref _triDelay, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> SingFullTri;

    private bool _singFullTri;

    public bool SingFullTri
    {
        get => _singFullTri;
        set =>
            SetValue(ref _singFullTri, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> TXdataTrigger;

    private bool _txDataTrigger;

    public bool TxDataTrigger
    {
        get => _txDataTrigger;
        set =>
            SetValue(ref _txDataTrigger, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> FastRamp;

    private bool _fastRamp;

    public bool FastRamp
    {
        get => _fastRamp;
        set =>
            SetValue(ref _fastRamp, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> RampDelayFL;

    private bool _rampDelayFL;

    public bool RampDelayFL
    {
        get => _rampDelayFL;
        set =>
            SetValue(ref _rampDelayFL, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> RampDelay;

    private bool _rampDelay;

    public bool RampDelay
    {
        get => _rampDelay;
        set =>
            SetValue(ref _rampDelay, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    public string[] DevClkSel { get; } = { "PFD CLK", "PFD CLK × CLK" };

    #region NotifyProperty <int> DelClkSelSelectIndex

    private int _devClkSelSelectIndex;

    public int DevClkSelSelectIndex
    {
        get => _devClkSelSelectIndex;
        set =>
            SetValue(ref _devClkSelSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> DelStartEn;

    private bool _delStartEn;

    public bool DelStartEn
    {
        get => _delStartEn;
        set =>
            SetValue(ref _delStartEn, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <int> DelayStartWord

    private int _delayStartWord;

    public int DelayStartWord
    {
        get => _delayStartWord;
        set =>
            SetValueIf(ref _delayStartWord, value, v => v is >= 0 and < (2 << 12))
                .Then(UpdateRegisterValue);
    }

    #endregion

    public DDSR7ViewModel()
    {
        ControlBits = 7;
        Title = "DELAY REGISTER (R7)";
    }

    protected virtual void UpdateRegisterValue()
    {
        int newValue =
            (Convert.ToInt32(TxDataTriggerDelay) << 23) |
            (Convert.ToInt32(TriDelay) << 22) |
            (Convert.ToInt32(SingFullTri) << 21) |
            (Convert.ToInt32(TxDataTrigger) << 20) |
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
        TxDataTriggerDelay = Register.IsBitSet(registerValue, 23);
        TriDelay = Register.IsBitSet(registerValue, 22);
        SingFullTri = Register.IsBitSet(registerValue, 21);
        TxDataTrigger = Register.IsBitSet(registerValue, 20);
        FastRamp = Register.IsBitSet(registerValue, 19);
        RampDelayFL = Register.IsBitSet(registerValue, 18);
        RampDelay = Register.IsBitSet(registerValue, 17);
        DevClkSelSelectIndex = Register.GetBitFiled(registerValue, 16, 1);
        DelStartEn = Register.IsBitSet(registerValue, 15);
        DelayStartWord = Register.GetBitFiled(registerValue, 3, 12);
    }
}