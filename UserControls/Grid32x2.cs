using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SiliconRadarControlPanel.UserControls;

public class Grid32x2 : Grid
{
    private readonly  List<TextBlock> _bitsTextBlocks;
    public Grid32x2()
    {
        _bitsTextBlocks = new List<TextBlock>();
        for (int i = 0; i < 32; i++)
        {
            var columnDefinitions = new ColumnDefinition();
            columnDefinitions.Width = new GridLength(1.0, GridUnitType.Star);
            ColumnDefinitions.Add(columnDefinitions);
        }
        for (int i = 0; i < 3; i++)
        {
            var rowDefinition = new RowDefinition();
            RowDefinitions.Add(rowDefinition);
        }

        for (int i = 0; i < 32; i++)
        {
            var textBlock = new TextBlock()
            {
                Text = "0"
            };
            _bitsTextBlocks.Add(textBlock);
            Children.Add(textBlock);
            Grid.SetColumn(textBlock, i);
            Grid.SetRow(textBlock, 2);
        }
    }

    private UpdateBitsTextBloks()
    {
        
    }
}
