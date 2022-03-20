using SiliconRadarControlPanel.Infrastructure;

namespace SiliconRadarControlPanel.Settings;

public class ComPortSettings : IWritableSettings
{
    public string PortName { get; set; } = "COM1";
    public int BaudRate { get; set; } = 115200;
}
