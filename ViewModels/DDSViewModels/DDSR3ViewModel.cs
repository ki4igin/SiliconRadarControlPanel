using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR3ViewModel : DDSRegisterViewModel
{
    public string[] NegBleedCurrent { get; } =
    {
        "3.73", "11.03", "25.25", "53.1",
        "109.7", "224.7", "454.7", "916.4"
    };

    #region NotifyProperty <int> NegBleedCurrentSelectIndex

    private int _negBleedCurrentSelectIndex;

    public int NegBleedCurrentSelectIndex
    {
        get => _negBleedCurrentSelectIndex;
        set =>
            SetValue(ref _negBleedCurrentSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> NegBleedEnable

    private bool _negBleedEnable;

    public bool NegBleedEnable
    {
        get => _negBleedEnable;
        set =>
            SetValue(ref _negBleedEnable, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> LOL

    private bool _lol;

    public bool LOL
    {
        get => _lol;
        set =>
            SetValue(ref _lol, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> NSel

    private bool _nSel;

    public bool NSel
    {
        get => _nSel;
        set =>
            SetValue(ref _nSel, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> SdReset

    private bool _sdReset;

    public bool SdReset
    {
        get => _sdReset;
        set =>
            SetValue(ref _sdReset, value).Then(UpdateRegisterValue);
    }

    #endregion

    public string[] RampMode { get; } =
    {
        "CONTINUOUS SAWTOOTH",
        "CONTINUOUS TRIANGULAR",
        "SINGLE SAWTOOTH BURST",
        "SINGLE RAMP BURST"
    };

    #region NotifyProperty <int> RampModeSelectIndex

    private int _rampModeSelectIndex;

    public int RampModeSelectIndex
    {
        get => _rampModeSelectIndex;
        set =>
            SetValue(ref _rampModeSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> PSK

    private bool _psk;

    public bool PSK
    {
        get => _psk;
        set =>
            SetValue(ref _psk, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> FSK

    private bool _fsk;

    public bool FSK
    {
        get => _fsk;
        set =>
            SetValue(ref _fsk, value).Then(UpdateRegisterValue);
    }

    #endregion

    public string[] LDP { get; } =
    {
        "14ns",
        "6ns"
    };

    #region NotifyProperty <int> LDPSelectIndex

    private int _ldpSelectIndex;

    public int LDPSelectIndex
    {
        get => _ldpSelectIndex;
        set =>
            SetValue(ref _ldpSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    public string[] PDPolarity { get; } =
    {
        "N",
        "P"
    };

    #region NotifyProperty <int> PDPolaritySelectIndex

    private int _pdPolaritySelectIndex;

    public int PDPolaritySelectIndex
    {
        get => _pdPolaritySelectIndex;
        set =>
            SetValue(ref _pdPolaritySelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> PowerDown

    private bool _powerDown;

    public bool PowerDown
    {
        get => _powerDown;
        set =>
            SetValue(ref _powerDown, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> CPThreeState

    private bool _cpThreeState;

    public bool CPThreeState
    {
        get => _cpThreeState;
        set =>
            SetValue(ref _cpThreeState, value).Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <bool> CounterReset

    private bool _counterReset;

    public bool CounterReset
    {
        get => _counterReset;
        set =>
            SetValue(ref _counterReset, value).Then(UpdateRegisterValue);
    }

    #endregion

    public DDSR3ViewModel()
    {
        ControlBits = 3;
        Title = "FUNCTION REGISTER (R3)";
    }

    protected virtual void UpdateRegisterValue()
    {
        int newValue =
            (NegBleedCurrentSelectIndex << 22) |
            (Convert.ToInt32(NegBleedEnable) << 21) |
            (Convert.ToInt32(LOL) << 16) |
            (Convert.ToInt32(NSel) << 15) |
            (Convert.ToInt32(SdReset) << 14) |
            (RampModeSelectIndex << 10) |
            (Convert.ToInt32(PSK) << 9) |
            (Convert.ToInt32(FSK) << 8) |
            (LDPSelectIndex << 7) |
            (PDPolaritySelectIndex << 6) |
            (Convert.ToInt32(PowerDown) << 5) |
            (Convert.ToInt32(CPThreeState) << 4) |
            (Convert.ToInt32(CounterReset) << 3) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        NegBleedCurrentSelectIndex = Register.GetBitFiled(registerValue, 22, 3);
        NegBleedEnable = Register.IsBitSet(registerValue, 21);
        LOL = Register.IsBitSet(registerValue, 16);
        NSel = Register.IsBitSet(registerValue, 15);
        SdReset = Register.IsBitSet(registerValue, 14);
        RampModeSelectIndex = Register.GetBitFiled(registerValue, 10, 2);
        PSK = Register.IsBitSet(registerValue, 9);
        FSK = Register.IsBitSet(registerValue, 8);
        LDPSelectIndex = Register.GetBitFiled(registerValue, 7, 1);
        PDPolaritySelectIndex = Register.GetBitFiled(registerValue, 6, 1);
        PowerDown = Register.IsBitSet(registerValue, 5);
        CPThreeState = Register.IsBitSet(registerValue, 4);
        CounterReset = Register.IsBitSet(registerValue, 3);
    }
}