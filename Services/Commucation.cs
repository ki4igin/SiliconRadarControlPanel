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

    public Communication()
    {
        _serialPort = new SerialPort()
        {
            BaudRate = 115200,
            DataBits = 8,
            ReadTimeout = 5000,
            WriteTimeout = 5000
        };
    }

    public bool Connect()
    {
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
                continue;

            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
            {
                Log.Information("Успех");
                return true;
            }
            _serialPort.Close();
        }
        return false;
    }

    public async Task<bool> ConnectAsync()
    {
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
                continue;

            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
            {
                Log.Information("Успех");
                return true;
            }
            _serialPort.Close();
        }
        return false;
    }



    public void SendCommand(string command)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(command);
        _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }

}
