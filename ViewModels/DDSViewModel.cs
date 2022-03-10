using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.ViewModels;

public class DDSViewModel : Base.ViewModelBase
{

    #region NotifyProperty <bool> RampOn
    private bool _rampOn;
    public bool RampOn { get => _rampOn; set => Set(ref _rampOn, value); }
    #endregion


    #region NotifyProperty <int> IntegerValue
    private int _integerValue;
    public int IntegerValue
    {
        get => _integerValue;
        set
        {
            if (value is >= 0 and <= 4095)
                Set(ref _integerValue, value);
        }
    }
    #endregion


    #region NotifyProperty <int> FractionalValue
    private int _fractionalValue;
    public int FractionalValue
    {
        get => _fractionalValue;
        set
        {
            if (value is >= 0 and <= 4095)
                Set(ref _fractionalValue, value);
        }
    }
    #endregion




}
