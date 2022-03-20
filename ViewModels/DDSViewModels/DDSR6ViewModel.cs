using SiliconRadarControlPanel.Infrastructure;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public class DDSR6ViewModel : DDSRegisterViewModel
{
    public string[] StepSel { get; } = { "1", "2" };

    #region NotifyProperty <int> StepSelSelectIndex

    private int _stepSelSelectIndex;

    public int StepSelSelectIndex
    {
        get => _stepSelSelectIndex;
        set =>
            SetValue(ref _stepSelSelectIndex, value)
                .Then(UpdateRegisterValue);
    }

    #endregion

    #region NotifyProperty <int> StepWord

    private int _stepWord;

    public int StepWord
    {
        get => _stepWord;
        set =>
            SetValueIf(ref _stepWord, value, v => v is >= 0 and < (2 << 20))
                .Then(UpdateRegisterValue);
    }

    #endregion

    public DDSR6ViewModel()
    {
        ControlBits = 6;
        Title = "STEP REGISTER (R6)";
    }

    protected virtual void UpdateRegisterValue()
    {
        int newValue =
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