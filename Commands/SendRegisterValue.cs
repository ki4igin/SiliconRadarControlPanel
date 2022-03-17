using Microsoft.Extensions.DependencyInjection;
using SiliconRadarControlPanel.Commands.Base;
using SiliconRadarControlPanel.Services;

namespace SiliconRadarControlPanel.Commands;

public class SendRegisterValue : CommandBase
{
    private readonly Communication _communication;
    
    public SendRegisterValue()
    {
        _communication = App.Services.GetRequiredService<Communication>();
    }

    protected override bool CanExecute(object? parameter) =>
        parameter is uint && _communication.IsConnected;

    protected override void Execute(object? parameter)
    {
        if (parameter is uint value)
            _communication.SendRegisterValue(value);
    }
}
