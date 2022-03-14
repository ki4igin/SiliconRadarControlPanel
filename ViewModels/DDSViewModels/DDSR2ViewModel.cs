using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR2ViewModel : DDSRegisterViewModel
{

    #region NotifyProperty <bool> CSR
    private bool _csr;
    public bool CSR
    {
        get => _csr;
        set =>
            SetValue(ref _csr, value).
            Then(() => UpdateRegisterValue());
    }
    #endregion

    private readonly string[] _cpCurrentSetting =
        {
            "0.31", "0.63", "0.94", "1.25",
            "1.57", "1.88", "2.19", "2.5",
            "2.81", "3.13", "3.44", "3.75",
            "4.06", "4.38", "4.69", "5.0"
        };
    public string[] CpCurrentSetting { get => _cpCurrentSetting; }

    #region NotifyProperty <int> CpCurrentSettingSelectIndex
    private int _cpCurrentSettingSelectIndex = 0;
    public int CpCurrentSettingSelectIndex
    {
        get => _cpCurrentSettingSelectIndex;
        set =>
            SetValue(ref _cpCurrentSettingSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    private readonly string[] _prescaler = { "4/5", "8/9" };
    public string[] Prescaler { get => _prescaler; }

    #region NotifyProperty <int> PrescalerSelectIndex
    private int _prescalerSelectIndex = 0;
    public int PrescalerSelectIndex
    {
        get => _prescalerSelectIndex;
        set =>
            SetValue(ref _prescalerSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> Rdiv2
    private bool _rDiv2;
    public bool Rdiv2
    {
        get => _rDiv2;
        set =>
            SetValue(ref _rDiv2, value).
            Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <bool> ReferenceDoubler
    private bool _referenceDoubler;
    public bool ReferenceDoubler
    {
        get => _referenceDoubler;
        set =>
            SetValue(ref _referenceDoubler, value).
            Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> RCounter
    private int _rCounter;
    public int RCounter
    {
        get => _rCounter;
        set =>
            SetValueIf(ref _rCounter, value, (v) => v is > 0 and <= 32)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> Clk1DividerValue
    private int _clk1DividerValue;
    public int Clk1DividerValue
    {
        get => _clk1DividerValue;
        set =>
            SetValueIf(ref _clk1DividerValue, value, (v) => v is >= 0 and < (2 << 12))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public DDSR2ViewModel()
    {
        ControlBits = 2;
        Title = "R DIVIDER REGISTER (R2)";
    }

    protected override void UpdateRegisterValue()
    {
        var rcounter = RCounter == 32 ? 0 : RCounter;
        var newValue =
            (Convert.ToInt32(CSR) << 28) |
            (CpCurrentSettingSelectIndex << 24) |
            (PrescalerSelectIndex << 22) |
            (Convert.ToInt32(Rdiv2) << 21) |
            (Convert.ToInt32(ReferenceDoubler) << 20) |
            (rcounter << 15) |
            (Clk1DividerValue << 3) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        CSR = Register.IsBitSet(registerValue, 28);
        CpCurrentSettingSelectIndex = Register.GetBitFiled(registerValue, 24, 4);
        PrescalerSelectIndex = Register.GetBitFiled(registerValue, 22, 1);
        Rdiv2 = Register.IsBitSet(registerValue, 21);
        ReferenceDoubler = Register.IsBitSet(registerValue, 20);
        var rcounter = Register.GetBitFiled(registerValue, 15, 5);
        RCounter = rcounter == 0 ? 32 : rcounter;
        Clk1DividerValue = Register.GetBitFiled(registerValue, 3, 12);
    }
}
