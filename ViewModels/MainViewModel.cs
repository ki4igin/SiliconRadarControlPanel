using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.ViewModels.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SiliconRadarControlPanel.Services;

namespace SiliconRadarControlPanel.ViewModels;

public class MainViewModel : TitledViewModel
{
    private readonly Communication _communication; 
    
    #region Command Connect
    private SimpleCommand? _connect;
    public SimpleCommand Connect => _connect ??= new SimpleCommand(
        () => _communication.Connect()
    );
    #endregion


    #region Command ConnectAsync
    private CommandAsync? _connectAsync;
    public CommandAsync ConnectAsync => _connectAsync ??= new CommandAsync(
        execute: (prog, ct) => _communication.ConnectAsync()
    );
    #endregion


    public MainViewModel()
    {
        _communication = new Communication();
    }
}
