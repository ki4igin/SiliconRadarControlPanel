using Microsoft.Extensions.Options;
using Serilog;
using SiliconRadarControlPanel.Infrastructure;
using SiliconRadarControlPanel.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.Services;

public class Communication
{
    private readonly SerialPort _serialPort;    
    private readonly ComPortSettings _comPortSettings;


    private bool _isConnected;
    public bool IsConnected
    {
        get => _isConnected;
        private set
        {
            _isConnected = value;
            IsConnectedChanged?.Invoke();
        }
    }

    public event Action? IsConnectedChanged;

    public Communication() :
        this(new OptionsWrapper<ComPortSettings>(new ComPortSettings()))
    {
    }

    public Communication(IOptions<ComPortSettings> options)
    {
        _comPortSettings = options.Value;
        _serialPort = new SerialPort
        {
            PortName = _comPortSettings.PortName,
            BaudRate = _comPortSettings.BaudRate,
            DataBits = 8,
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };
    }

    public bool Connect()
    {
        IsConnected = false;

        List<string> portNames = new(SerialPort.GetPortNames());
        portNames.Remove(_serialPort.PortName);
        portNames.Insert(0, _serialPort.PortName);

        foreach (string portName in portNames)
        {
            Log.Information("Попытка подключиться к плате через {portName}.....", portName);

            if (Connect(portName) is false)
                continue;

            Log.Information("Успех");
            IsConnected = true;
            break;
        }

        return IsConnected;
    }

    public async Task<bool> ConnectAsync(IProgress<double> progress)
    {
        IsConnected = false;

        List<string> portNames = new(SerialPort.GetPortNames());
        if (portNames.Remove(_serialPort.PortName) is true)
            portNames.Insert(0, _serialPort.PortName);

        double cnt = 0;
        foreach (string portName in portNames)
        {
            Log.Information("Попытка подключиться к плате через {portName}.....", portName);
            progress.Report(cnt++ / portNames.Count);

            if (await ConnectAsync(portName) is false)
                continue;

            Log.Information("Успех");
            IsConnected = true;
            _ = Task.Run(ConnectionProcess);
            break;
        }
        progress.Report(1);
        return IsConnected;
    }

    public void DisConnect()
    {
        if (IsConnected is false)
            return;

        IsConnected = false;        
        _serialPort.Close();
    }

    public void SendCommand(string command)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(command);
       _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }

    public void SendRegisterValue(uint value)
    {
        SendCommand("plwr");

        byte[] sendBytes = BitConverter.GetBytes(value);
        _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }

    private bool Connect(string portName)
    {
        _serialPort.PortName = portName;
        if (_serialPort.TryOpen() is false)
            return false;


        const string testCommand = "test";
        SendCommand(testCommand);
        byte[] rxBuffer = new byte[testCommand.Length];

        if (_serialPort.TryRead(rxBuffer, testCommand.Length) is false)
        {
            _serialPort.Close();
            return false;
        }

        string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
        if (string.Equals(testCommand, rxCommand))
            return true;

        _serialPort.Close();
        return false;
    }

    private async Task<bool> ConnectAsync(string portName)
    {
        _serialPort.PortName = portName;
        if (_serialPort.TryOpen() is false)
            return false;

        _serialPort.DiscardInBuffer();

        if (await TestConnectionAsync() is true)
        {            
            return true;
        }

        _serialPort.Close();
        return false;
    }

    private async Task<bool> TestConnectionAsync()
    {
        const string testCommand = "test";

        try
        {
            _serialPort.DiscardInBuffer();
            SendCommand(testCommand);
        } catch (InvalidOperationException)
        {
            return false;
        }            

        byte[] rxBuffer = new byte[testCommand.Length];

        if (await _serialPort.TryReadAsync(rxBuffer, testCommand.Length) is true)
        {
            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
                return true;
        }

        return false;
    }

    private bool TestConnection()
    {
        const string testCommand = "test";

        try
        {
            _serialPort.DiscardInBuffer();
            SendCommand(testCommand);
        }
        catch (Exception ex) when (ex is InvalidOperationException or IOException)
        {
            return false;
        }

        byte[] rxBuffer = new byte[testCommand.Length];

        if (_serialPort.TryRead(rxBuffer, testCommand.Length) is true)
        {
            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
                return true;
        }

        return false;
    }

    private void ConnectionProcess()
    {
        while (IsConnected)
        {
            IsConnected = TestConnection();
            Thread.Sleep(1000);
        }
        _serialPort.Close();
    }
}