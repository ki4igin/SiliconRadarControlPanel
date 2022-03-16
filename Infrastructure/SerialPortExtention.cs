using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace SiliconRadarControlPanel.Infrastructure;

public static class SerialPortExtention
{
    public static bool TryOpen(this SerialPort serialPort)
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

    public static bool TryRead(this SerialPort serialPort, byte[] buffer, int offset, int count)
    {
        int rxCount = 0;
        try
        {
            do
            {
                rxCount += serialPort.Read(buffer, rxCount, count - rxCount);
            } while (rxCount != count);
            
        }
        catch (TimeoutException)
        {
            return false;
        }
        return true;
    }

    public static async Task<bool> TryReadAsync(this SerialPort serialPort, byte[] buffer, int offset, int count) =>
        await Task.Run(() => TryRead(serialPort, buffer, offset, count));

    public static async Task ReadAsync(this SerialPort serialPort, byte[] buffer, int offset, int count) =>
        await Task.Run(() =>
        {
            try
            {
                serialPort.Read(buffer, offset, count);
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
           
        });
}
