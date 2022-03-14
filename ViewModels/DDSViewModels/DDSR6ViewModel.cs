using SiliconRadarControlPanel.Infrastructure;
using System;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR6ViewModel : DDSRegisterViewModel
{
    public readonly string[] _stepSel = { "1", "2" };
    public string[] StepSel { get => _stepSel; }

    #region NotifyProperty <int> StepSelSelectIndex
    private int _stepSelSelectIndex = 0;
    public int StepSelSelectIndex
    {
        get => _stepSelSelectIndex;
        set =>
            SetValue(ref _stepSelSelectIndex, value)
            .Then(() => UpdateRegisterValue());
    }
    #endregion    

    #region NotifyProperty <int> StepWord
    private int _stepWord = 0;
    public int StepWord
    {
        get => _stepWord;
        set =>
            SetValueIf(ref _stepWord, value, (v) => v is >= 0 and < (2 << 20))
            .Then(() => UpdateRegisterValue());
    }
    #endregion  

    public DDSR6ViewModel()
    {
        ControlBits = 6;
        Title = "STEP REGISTER (R6)";
    }

    protected override void UpdateRegisterValue()
    {
        var newValue =
            (StepSelSelectIndex << 23) |
            (StepWord << 3) |
            ControlBits;
        Set(ref _registerValue, (uint)newValue, nameof(RegisterValue));
    }

    protected override void UpdateBitFields(uint registerValue)
    {
        StepSelSelectIndex = Register.GetBitFiled(registerValue, 23, 1);
        StepWord = Register.GetBitFiled(registerValue, 3, 20);
    }
}
