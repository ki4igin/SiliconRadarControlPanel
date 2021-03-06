using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.ViewModels.Base;
using SiliconRadarControlPanel.Services;
using SiliconRadarControlPanel.Views.DDSViews;
using SiliconRadarControlPanel.ViewModels.DDSViewModels;
using Microsoft.Extensions.Options;
using SiliconRadarControlPanel.Settings;
using SiliconRadarControlPanel.Infrastructure;
using System;
using Microsoft.Extensions.Logging;

namespace SiliconRadarControlPanel.ViewModels;

public class MainViewModel : TitledViewModel
{
    private readonly Communication _communication;
    private DDSWindow? _ddsWindow;
    private readonly DDSViewModel _ddsViewModel;
    private readonly ComPortSettings _comPortSettings;

    #region Command Connect

    private SimpleCommand? _connect;

    public SimpleCommand Connect => _connect ??= new(
        () => _communication.Connect()
    );

    #endregion

    #region Command ConnectAsync

    private CommandAsync? _connectAsync;

    public CommandAsync ConnectAsync => _connectAsync ??= new(
        execute: async (progress, _) => {
            void Handler(object? obj, double arg) => ProgressBarValue = arg;
            progress.ProgressChanged += Handler;
            IsConnected = await _communication.ConnectAsync(progress);
            progress.ProgressChanged -= Handler;
        },
        canExecute: () => IsConnected == false
    );

    #endregion

    #region Command DisConnect

    private SimpleCommand? _disconnect;

    public SimpleCommand DisConnect => _disconnect ??= new(
        execute: () =>
        {
            _communication.DisConnect();
            IsConnected = false;
        },
        canExecute: () => IsConnected
    );

    #endregion
    
    

    #region Command SendCommand

    private SimpleCommand? _sendCommand;
    public SimpleCommand SendCommand => _sendCommand ??= new(
        execute: () => _communication.SendCommand(RadarCommandValue),
        canExecute: () => IsConnected && RadarCommandValue.Length == 4
        );
    


    #endregion

    #region Command OpenDDSWindow

    private SimpleCommand? _openDDSWindow;

    public SimpleCommand OpenDDSWindow => _openDDSWindow ??= new(
        execute: () =>
        {
            _ddsWindow = new()
            {
                DataContext = _ddsViewModel
            };
            _ddsWindow.Show();
        }
    );

    #endregion

    #region NotifyProperty <bool> IsConnected

    private bool _isConnected;

    public bool IsConnected
    {
        get => _isConnected;
        set => Set(ref _isConnected, value);
    }

    #endregion


    #region NotifyProperty <double> ProgressBarValue
    private double _progressBarValue;
    public double ProgressBarValue { get => _progressBarValue; set => Set(ref _progressBarValue, value); }
    #endregion

    #region NotifyProperty <string> RadarCommandValue
    private string _radarCommandValue;
    public string RadarCommandValue { get => _radarCommandValue; set => Set(ref _radarCommandValue, value); }
    #endregion

    #region SaveSettigns SaveSettigns

    private SimpleCommand? _saveSettings;

    public SimpleCommand Save => _saveSettings ??= new(
        () => _comPortSettings.SaveToFile()
    );

    #endregion

    public MainViewModel(Communication communication, DDSViewModel ddsViewModel, IOptions<ComPortSettings> options)
    {
        Title = "Title";
        _communication = communication;
        _communication.IsConnectedChanged += () => IsConnected = _communication.IsConnected;
        _ddsViewModel = ddsViewModel;
        _comPortSettings = options.Value;
        _radarCommandValue = string.Empty;
    }
}