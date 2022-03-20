using Microsoft.Extensions.Options;
using Serilog;
using SiliconRadarControlPanel.Infrastructure;
using SiliconRadarControlPanel.Settings;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.Services;

public class Communication
{
    private readonly SerialPort _serialPort;

    public bool IsConnected { get; private set; }

    public Communication() :
        this(new OptionsWrapper<ComPortSettings>(new ComPortSettings()))
    {
    }

    public Communication(IOptions<ComPortSettings> options)
    {
        ComPortSettings comPortSettings = options.Value;
        _serialPort = new SerialPort
        {
            PortName = comPortSettings.PortName,
            BaudRate = comPortSettings.BaudRate,
            DataBits = 8,
            ReadTimeout = 2000,
            WriteTimeout = 2000
        };
    }

    public bool Connect()
    {
        IsConnected = false;

        if (Connect(_serialPort.PortName) is true)
        {
            IsConnected = true;
            return IsConnected;
        }

        string[] portNames = SerialPort.GetPortNames();
        foreach (string? portName in portNames)
        {
            if (Connect(portName) is false)
                continue;
            
            IsConnected = true;
            break;
        }

        return IsConnected;
    }

    private bool Connect(string portName)
    {
        _serialPort.PortName = portName;
        if (_serialPort.TryOpen() is false)
            return false;

        Log.Information("Попытка подключиться к плате через {portName}.....", portName);
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
        {
            Log.Information("Успех");
            return true;
        }

        _serialPort.Close();
        return false;
    }

    public async Task<bool> ConnectAsync()
    {
        IsConnected = false;

        string[] portNames = SerialPort.GetPortNames();
        foreach (string? portName in portNames)
        {
            _serialPort.PortName = portName;
            if (_serialPort.TryOpen() is false)
                continue;

            Log.Information("Попытка подключиться к плате через {portName}.....", portName);
            _serialPort.DiscardInBuffer();
            const string testCommand = "test";
            SendCommand(testCommand);
            byte[] rxBuffer = new byte[testCommand.Length];

            if (await _serialPort.TryReadAsync(rxBuffer, testCommand.Length) is false)
            {
                _serialPort.Close();
                continue;
            }

            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
            {
                Log.Information("Успех");
                IsConnected = true;
                break;
            }

            _serialPort.Close();
        }

        return IsConnected;
    }

    public void DisConnect()
    {
        if (IsConnected is false)
            return;
        
        _serialPort.Close();
        IsConnected = false;
    }

    public void SendCommand(string command)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(command);
        _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }

    public void SendRegisterValue(uint value)
    {
        byte[] sendBytes = BitConverter.GetBytes(value);
        _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }
}