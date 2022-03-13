using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR0ViewModel : DDSRegisterViewModel
{
    #region NotifyProperty <bool> RampOn
    private bool _rampOn;
    public bool RampOn
    {
        get => _rampOn;
        set =>
            SetValue(ref _rampOn, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> IntegerValue
    private int _integerValue;
    public int IntegerValue
    {
        get => _integerValue;
        set =>
            SetValueIf(ref _integerValue, value, (v) => v is >= 0 and < (2 << 12))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> FractionalValue
    private int _fractionalValue;
    public int FractionalValue
    {
        get => _fractionalValue;
        set =>
            SetValueIf(ref _fractionalValue, value, (v) => v is >= 0 and <= 4095)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public readonly string[] _muxControl =
    {
        "THREE-STATE OUTPUT",
        "DV DD",
        "DGND",
        "R DIVIDER OUTPUT",
        "N DIVIDER OUTPUT",
        "RESERVED",
        "DIGITAL LOCK DETECT",
        "SERIAL DATA OUTPUT",
        "RESERVED",
        "RESERVED",
        "CLK DIVIDER OUTPUT",
        "RESERVED",
        "RESERVED",
        "R DIVIDER/2",
        "N DIVIDER/2",
        "READBACK TO MUXOUT"
    };
    public string[] MuxControl { get => _muxControl; }

    #region NotifyProperty <int> MuxControlSelectIndex
    private int _muxControlSelectIndex = 0;
    public int MuxControlSelectIndex
    {
        get => _muxControlSelectIndex;
        set =>
            SetValue(ref _muxControlSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public DDSR0ViewModel()
    {
        ControlBits = 0;
        Title = "FRAC/INT REGISTER (R0)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue = 
            (Convert.ToInt32(RampOn) << 31) |
            (MuxControlSelectIndex << 27) |
            (IntegerValue << 15) |
            (FractionalValue << 3) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        RampOn = Register.IsBitSet(registerValue, 31);
        MuxControlSelectIndex = Register.GetBitFiled(registerValue, 27, 4);
        IntegerValue = Register.GetBitFiled(registerValue, 15, 12);
        FractionalValue = Register.GetBitFiled(registerValue, 3, 12);
    }
}
