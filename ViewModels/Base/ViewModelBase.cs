using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SiliconRadarControlPanel.ViewModels.Base;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value) is true)
            return false;
        
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
