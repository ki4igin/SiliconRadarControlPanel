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
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };
    }

    public bool Connect()
    {
        string[] portNames = SerialPort.GetPortNames();

        foreach (var portName in portNames)
        {
            _serialPort.PortName = portName;
            if(TryOpenSerialPort(_serialPort) is false)
                continue;

            Debug.WriteLine($"Попытка подключиться к плате через {portName}.....");
            const string testCommand = "test";
            SendCommand(testCommand);
            byte[] rxBuffer = new byte[testCommand.Length];
            TryReadSerialPort(_serialPort, rxBuffer, testCommand.Length);
            string rxCommand = Encoding.ASCII.GetString(rxBuffer, 0, testCommand.Length);
            if (string.Equals(testCommand, rxCommand))
            {
                Debug.WriteLine($"Успех");
                return true;
            }
            _serialPort.Close();
                
        }

        return false;
    }

    private bool TryOpenSerialPort(SerialPort serialPort)
    {
        try
        {
            serialPort.Open();
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }

        return true;
    }
    
    private bool TryReadSerialPort(SerialPort serialPort, byte[] buffer, int count)
    {
        int rxCount = 0;
        try
        {
            rxCount = serialPort.Read(buffer, 0, count);
        }
        catch (TimeoutException)
        {
            return false;
        }
        return count == rxCount;
    }

    public void SendCommand(string command)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes(command);
        _serialPort.Write(sendBytes, 0, sendBytes.Length);
    }
    
}
