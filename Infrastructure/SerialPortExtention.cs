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
            rxCount = serialPort.Read(buffer, offset, count);
        }
        catch (TimeoutException)
        {
            return false;
        }
        return count == rxCount;
    }

    public static async Task<bool> TryReadAsync(this SerialPort serialPort, byte[] buffer, int offset, int count) =>
        await Task.Run(() => TryRead(serialPort, buffer, offset, count));
}
