using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR4ViewModel : DDSRegisterViewModel
{
    #region NotifyProperty <bool> LESel
    private bool _leSel;
    public bool LESel
    {
        get => _leSel;
        set =>
            SetValue(ref _leSel, value).
            Then(() => UpdateRegisterValue());
    }
    #endregion

    private readonly int[] _ΣΔModulatorModeDistonary = {
        0b00000,
        0b01110
    };

    public readonly string[] _ΣΔModulatorMode = {
        "NORMAL OPERATIO",
        "DISABLED WHEN FRAC = 0"
    };
    public string[] ΣΔModulatroMode { get => _ΣΔModulatorMode; }

    #region NotifyProperty <int> ΣΔModulatroModeSelectIndex
    private int _ΣΔModulatorModeSelectIndex = 0;
    public int ΣΔModulatroModeSelectIndex
    {
        get => _ΣΔModulatorModeSelectIndex;
        set =>
            SetValue(ref _ΣΔModulatorModeSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    private readonly int[] _rampStatusDistonary = {
        0b00000,
        0b00010,
        0b00011,
        0b10000,
        0b10001
    };

    public readonly string[] _rampStatus = {
        "NORMAL OPERATION",
        "READBACK TO MUXOUT",
        "RAMP COMPLETE TO MUXOUT",
        "CHARGE PUMP UP",
        "CHARGE PUMP DOWN"
    };
    public string[] RampStatus { get => _rampStatus; }

    #region NotifyProperty <int> RampStatusSelectIndex
    private int _rampStatusSelectIndex = 0;
    public int RampStatusSelectIndex
    {
        get => _rampStatusSelectIndex;
        set =>
            SetValue(ref _rampStatusSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _clkDivMode = {
        "CLOCK DIVIDER OFF",
        "FAST LOCK DIVIDER",
        "RESERVED",
        "RAMP DIVIDER"
    };
    public string[] ClkDivMode { get => _clkDivMode; }

    #region NotifyProperty <int> ClkDivModeSelectIndex
    private int _clkDivModeSelectIndex = 0;
    public int ClkDivModeSelectIndex
    {
        get => _clkDivModeSelectIndex;
        set =>
            SetValue(ref _clkDivModeSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> Clk2DividerValue
    private int _clk2DividerValue;
    public int Clk2DividerValue
    {
        get => _clk2DividerValue;
        set =>
            SetValueIf(ref _clk2DividerValue, value, (v) => v is >= 0 and < (2 << 12))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _clkDivSel = { "1", "2" };
    public string[] ClkDivSel { get => _clkDivSel; }

    #region NotifyProperty <int> ClkDivSelSelectIndex
    private int _clkDivSelSelectIndex = 0;
    public int ClkDivSelSelectIndex
    {
        get => _clkDivSelSelectIndex;
        set =>
            SetValue(ref _clkDivSelSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public DDSR4ViewModel()
    {
        ControlBits = 4;
        Title = "CLOCK REGISTER (R4)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue =
            (Convert.ToInt32(LESel) << 31) |
            (_ΣΔModulatorModeDistonary[ΣΔModulatroModeSelectIndex] << 26) |
            (_rampStatusDistonary[RampStatusSelectIndex] << 21) |
            (ClkDivModeSelectIndex << 19) |
            (Clk2DividerValue << 7) |
            (ClkDivSelSelectIndex << 6) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        LESel = Register.IsBitSet(registerValue, 31);
        var temp = Array.FindIndex(
            _ΣΔModulatorModeDistonary,
            (v) => v == Register.GetBitFiled(registerValue, 26, 5)
        );
        ΣΔModulatroModeSelectIndex = temp == -1 ? 0 : temp;
        temp = Array.FindIndex(
            _rampStatusDistonary,
            (v) => v == Register.GetBitFiled(registerValue, 21, 5)
        );
        RampStatusSelectIndex = temp == -1 ? 0 : temp;
        ClkDivModeSelectIndex = Register.GetBitFiled(registerValue, 19, 2);
        Clk2DividerValue = Register.GetBitFiled(registerValue, 7, 12);
        ClkDivSelSelectIndex = Register.GetBitFiled(registerValue, 6, 1);
    }
}
