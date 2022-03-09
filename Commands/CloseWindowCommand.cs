using SiliconRadarControlPanel.Commands.Base;
using System.Windows;

namespace SiliconRadarControlPanel.Commands;

public class CloseWindowCommand : CommandBase
{
    protected override bool CanExecute(object? parameter) =>
        parameter is Window;

    protected override void Execute(object? parameter) =>
        (parameter as Window)?.Close();
}
