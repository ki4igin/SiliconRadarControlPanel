using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SiliconRadarControlPanel.Commands.Base;

public abstract class CommandBase : ICommand, INotifyPropertyChanged, IDisposable
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    protected virtual bool CanExecute(object? parameter) => true;

    protected abstract void Execute(object? parameter);

    #region ICommand implementations
    bool ICommand.CanExecute(object? parameter) =>
        CanExecute(parameter);

    void ICommand.Execute(object? parameter) =>
        Execute(parameter);
    #endregion

    #region INotifyPropertyChanged implementations
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion

    #region IDisposable implementations
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;
        _disposed = true;
    }
    #endregion
}
