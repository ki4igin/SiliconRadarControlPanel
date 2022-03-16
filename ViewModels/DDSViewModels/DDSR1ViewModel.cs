using SiliconRadarControlPanel.Infrastructure;
using SiliconRadarControlPanel.Services;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR1ViewModel : DDSRegisterViewModel
{

    #region NotifyProperty <bool> PhaseAdjust
    private bool _phaseAdjust;
    public bool PhaseAdjust
    {
        get => _phaseAdjust;
        set =>
            SetValue(ref _phaseAdjust, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> FractionalValue
    private int _fractionalValue;
    public int FractionalValue
    {
        get => _fractionalValue;
        set =>
            SetValueIf(ref _fractionalValue, value, (v) => v is >= 0 and < (2 << 13))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    #region NotifyProperty <int> PhaseValue
    private int _phaseValue;
    public int PhaseValue
    {
        get => _phaseValue;
        set =>
            SetValueIf(ref _phaseValue, value, (v) => v is >= -(2 << 11) and < (2 << 11))
            .Then(() => UpdateRegisterValue());
    }
    #endregion

    public DDSR1ViewModel() : this(new Communication()) { }
    public DDSR1ViewModel(Communication communication) : base(communication)
    {
        ControlBits = 1;
        Title = "LSB FRAC REGISTER (R1)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue = (Convert.ToInt32(PhaseValue) << 28) | (FractionalValue << 15) | ControlBits;
        newValue = (int)Register.SetBitFiled((uint)newValue, PhaseValue, 3, 12);
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        PhaseAdjust = Register.IsBitSet(registerValue, 28);
        FractionalValue = Register.GetBitFiled(registerValue, 15, 13);
        int phaseValue = Register.GetBitFiled(registerValue, 3, 12);
        PhaseValue = phaseValue switch
        {
            < 2048 => phaseValue,
            _ => phaseValue - 4096,
        };
    }
}
