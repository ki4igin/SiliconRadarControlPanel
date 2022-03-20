using SiliconRadarControlPanel.ViewModels.Base;

namespace SiliconRadarControlPanel.ViewModels.DDSViewModels;

public abstract class DDSRegisterViewModel : TitledViewModel
{
    #region NotifyProperty <int> ControlBits

    private readonly int _controlBits;

    public int ControlBits
    {
        get => _controlBits;
        init => Set(ref _controlBits, value);
    }

    #endregion

    #region NotifyProperty <uint> RegisterValue

    // ReSharper disable once InconsistentNaming
    private protected uint _registerValue;

    public uint RegisterValue
    {
        get => _registerValue;
        set =>
            SetValue(ref _registerValue, value)
                .Then(() => UpdateBitFields(value));
    }

    #endregion

    protected abstract void UpdateBitFields(uint registerValue);
}