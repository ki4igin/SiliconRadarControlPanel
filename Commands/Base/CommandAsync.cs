using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SiliconRadarControlPanel.Commands.Base;

public class CommandAsync : CommandBase
{
    private readonly Func<object?, Progress<double>, CancellationToken, Task> _execute;
    private readonly Func<object?, bool> _canExecute;

    private readonly Progress<double> _progress;
    private CancellationTokenSource _cts;

    #region Command Cancel
    private ICommand? _cancel;
    public ICommand Cancel => _cancel ??= new SimpleCommand(
        execute: () => _cts.Cancel(),
        canExecute: () => IsExecuting
    );
    #endregion

    #region NotifyProperty <bool> IsExecuting
    private bool _isExecuting;
    public bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            if (Set(ref _isExecuting, value))
                CommandManager.InvalidateRequerySuggested();
        }
    }
    #endregion

    #region NotifyProperty <double> ProgressValue
    private double _progressValue;
    public double ProgressValue { get => _progressValue; set => Set(ref _progressValue, value); }
    #endregion

    public CommandAsync(Func<Progress<double>, CancellationToken, Task> execute) :
        this((_, pros, ct) => execute(pros, ct), _ => true)
    { }
    public CommandAsync(Func<object?, Progress<double>, CancellationToken, Task> execute) :
        this(execute, _ => true)
    { }
    public CommandAsync(
        Func<Progress<double>, CancellationToken, Task> execute,
        Func<bool> canExecute
        ) :
        this((_, pros, ct) => execute(pros, ct), _ => canExecute())
    { }
    public CommandAsync(
        Func<object?, Progress<double>, CancellationToken, Task> execute,
        Func<bool> canExecute
        ) :
        this(execute, _ => canExecute())
    { }
    public CommandAsync(
        Func<Progress<double>, CancellationToken, Task> execute,
        Func<object?, bool> canExecute
        ) :
        this((_, pros, ct) => execute(pros, ct), canExecute)
    { }
    public CommandAsync(
        Func<object?, Progress<double>, CancellationToken, Task> execute,
        Func<object?, bool> canExecute
        )
    {
        _execute = execute;
        _canExecute = canExecute;
        _cts = new CancellationTokenSource();
        _progress = new Progress<double>(p => ProgressValue = p);
    }

    protected override bool CanExecute(object? parameter) =>
        _canExecute(parameter) && IsExecuting is false;

    protected override async void Execute(object? parameter)
    {
        if (CanExecute(parameter) is false)
            return;

        IsExecuting = true;
        try
        {
            await _execute(parameter, _progress, _cts.Token).ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            ProgressValue = 0;
        }
        IsExecuting = false;
    }

    private bool _disposed;
    protected override void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;

        _cts.Dispose();
        _disposed = true;
    }
}
