using Serilog;
using SiliconRadarControlPanel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.Services;

public class Communication
{
    private readonly SerialPort _serialPort;

    public bool IsConnected { get; private set; }

    public Communication()
    {
        _serialPort = new SerialPort()
        {
            BaudRate = 115200,
            DataBits = 8,
            ReadTimeout = 2000,
            WriteTimeout = 2000
        };
    }

    public bool Connect()
    {
        IsConnected = false;

        string[] portNames = SerialPort.GetPortNames();
        foreach (var portName in portNames)
        {
            _serialPort.PortName = portName;
            if (_serialPort.TryOpen() is false)
                continue;

            Log.Information("Попытка подключиться к плате через {portName}.....", portName);
            const string testCommand = "test";
            SendCommand(testCommand);           
            byte[] rxBuffer = new byte[testCommand.Length];

            if (_serialPort.TryRead(rxBuffer, 0, testCommand.Length) is false)
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

    public async Task<bool> ConnectAsync()
    {
        IsConnected = false;

        string[] portNames = SerialPort.GetPortNames();
        foreach (var portName in portNames)
        {
            _serialPort.PortName = portName;
            if (_serialPort.TryOpen() is false)
                continue;

            Log.Information("Попытка подключиться к плате через {portName}.....", portName);
            const string testCommand = "test";
            SendCommand(testCommand);
            byte[] rxBuffer = new byte[testCommand.Length];

            if (await _serialPort.TryReadAsync(rxBuffer, 0, testCommand.Length) is false)
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


    //public async Task<bool> ConnectAsync()
    //{
    //    IsConnected = false;

    //    string[] portNames = SerialPort.GetPortNames();
    //    foreach (var portName in portNames)
    //    {
    //        try
    //        {
    //            _serialPort.PortName = portName;
    //            _serialPort.Open();

    //            const string testCommand = "test";
    //            SendCommand(testCommand);
    //            byte[] rxBuffer = new byte[testCommand.Length];
    //            await _serialPort.ReadAsync(rxBuffer, 0, testCommand.Length);

    //            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
    //            if (string.Equals(testCommand, rxCommand))
    //            {
    //                Log.Information("Успех");
    //                IsConnected = true;
    //                break;
    //            }
    //        }
    //        catch (UnauthorizedAccessException)
    //        {                
    //        }
    //    }
    //    IsConnected = false;
    //    return false;
    //}

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
