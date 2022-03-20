using SiliconRadarControlPanel.Infrastructure;

namespace SiliconRadarControlPanel.Settings;

public class DDSSettings : IWritableSettings
{
    public uint R0 { get; set; }
    public uint R1 { get; set; }
    public uint R2 { get; set; }
    public uint R3 { get; set; }
    public uint R4 { get; set; }
    public uint R5 { get; set; }
    public uint R6 { get; set; }
    public uint R7 { get; set; }
}