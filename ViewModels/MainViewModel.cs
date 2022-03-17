using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.ViewModels.Base;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Views.DDSViews;
using SiliconRadarControlPanel.ViewModels.DDSViewModels;

namespace SiliconRadarControlPanel.ViewModels;

public class MainViewModel : TitledViewModel
{
    private readonly Communication _communication;
    private DDSWindow? _ddsWindow;
    private readonly DDSViewModel _ddsViewModel;

    #region Command Connect
    private SimpleCommand? _connect;
    public SimpleCommand Connect => _connect ??= new SimpleCommand(
        () => _communication.Connect()
    );
    #endregion

    #region Command ConnectAsync
    private CommandAsync? _connectAsync;
    public CommandAsync ConnectAsync => _connectAsync ??= new CommandAsync(
        execute: async (prog, ct) =>
        {
            IsConnected = await _communication.ConnectAsync();
        },
        canExecute: () => IsConnected == false
    );
    #endregion


    #region Command DisConnect
    private SimpleCommand? _disconnect;
    public SimpleCommand DisConnect => _disconnect ??= new SimpleCommand(
        execute: () =>
        {
            _communication.DisConnect();
            IsConnected = false;
        },
        canExecute: () => IsConnected
    );
    #endregion


    #region Command OpenDDSWindow
    private SimpleCommand? _openDDSWindow;
    public SimpleCommand OpenDDSWindow => _openDDSWindow ??= new SimpleCommand(
        execute: () =>
        {
            _ddsWindow = new DDSWindow()
            {
                DataContext = _ddsViewModel
            };
            _ddsWindow.Show();
        }
    );
    #endregion


    #region NotifyProperty <bool> IsConnected
    private bool _isConnected;
    public bool IsConnected { get => _isConnected; set => Set(ref _isConnected, value); }
    #endregion

    //public MainViewModel()
    //{
    //    _ddsViewModel = new DDSViewModel(_communication);
    //    _ddsWindow = new DDSWindow()
    //    {
    //        DataContext = _ddsViewModel
    //    };
    //}
    public MainViewModel(Communication communication, DDSViewModel ddsViewModel)
    {
        Title = "TTTT";
        _communication = communication;
        _ddsViewModel = ddsViewModel;
    }
}
