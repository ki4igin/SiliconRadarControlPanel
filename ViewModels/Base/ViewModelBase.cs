using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SiliconRadarControlPanel.ViewModels.Base;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new(propertyName));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value) is true)
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected bool SetIf<T>(ref T field, T value, Func<T, bool> valueCheck, [CallerMemberName] string propertyName = "") =>
       valueCheck(value) switch
       {
           true => Set(ref field, value, propertyName),
           false => false
       };

    protected SetValueResult<T> SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value) == true)
            return new(false, field, value);

        var oldValue = field;
        field = value;
        OnPropertyChanged(propertyName);
        return new(true, oldValue, value);
    }

    protected SetValueResult<T> SetValueIf<T>(ref T field, T value, Func<T, bool> valueCheck, [CallerMemberName] string propertyName = "") =>
        valueCheck(value) switch
        {
            true => SetValue(ref field, value, propertyName),
            false => new(false, field, value)
        };
}

public readonly ref struct SetValueResult<T>
{
    private readonly bool _result;
    private readonly T _oldValue;
    private readonly T _newValue;

    public SetValueResult(bool result, in T oldValue, in T newValue)
    {
        _result = result;
        _oldValue = oldValue;
        _newValue = newValue;
    }

    public SetValueResult<T> Then(Action action)
    {
        if (_result)
            action();

        return this;
    }
}
