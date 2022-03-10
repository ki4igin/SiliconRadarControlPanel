using System.Windows;
using System.Windows.Controls;

namespace SiliconRadarControlPanel.UserControls;

public class Grid32x2 : Grid
{
    public Grid32x2()
    {
        for (int i = 0; i < 32; i++)
        {
            var columnDefinitions = new ColumnDefinition();
            columnDefinitions.Width = new GridLength(2.0, GridUnitType.Star);
            ColumnDefinitions.Add(columnDefinitions);
        }
        for (int i = 0; i < 2; i++)
        {
            var rowDefinition = new RowDefinition();
            RowDefinitions.Add(rowDefinition);
        }
    }
}
