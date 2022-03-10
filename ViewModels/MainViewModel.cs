using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.ViewModels.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Views;

namespace SiliconRadarControlPanel.ViewModels;

public class MainViewModel : TitledViewModel
{
    private readonly Communication _communication;
    private readonly DDSWindow _ddsWindow;

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


    #region Command OpenDDSWindow
    private SimpleCommand? _openDDSWindow;
    public SimpleCommand OpenDDSWindow => _openDDSWindow ??= new SimpleCommand(
        execute: () => _ddsWindow.Show()
    );
    #endregion



    public MainViewModel()
    {
        _communication = new Communication();
        _ddsWindow = new DDSWindow();
    }
}
