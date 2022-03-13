using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SiliconRadarControlPanel.UserControls;

public class Grid32x2 : Grid
{
    private readonly List<TextBlock> _bitsTextBlocks;


    public static readonly DependencyProperty RegisterValueProperty = DependencyProperty.Register(
        "RegisterValue",
        typeof(uint),
        typeof(Grid32x2),
        new PropertyMetadata(0U, UpdateBitsTextBloks)
        );

    public uint RegisterValue
    {
        get => (uint)GetValue(RegisterValueProperty);
        set => SetValue(RegisterValueProperty, value);
    }

    public Grid32x2()
    {
        _bitsTextBlocks = new List<TextBlock>();
        for (int i = 0; i < 32; i++)
        {
            var columnDefinitions = new ColumnDefinition
            {
                Width = new GridLength(1.0, GridUnitType.Star)                
            };
            ColumnDefinitions.Add(columnDefinitions);
        }
        for (int i = 0; i < 3; i++)
        {
            var rowDefinition = new RowDefinition();
            RowDefinitions.Add(rowDefinition);
        }

        for (int i = 31; i >= 0; i--)
        {
            var textBlock = new TextBlock()
            {
                Text = "0",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            textBlock.PreviewMouseDown += OnClickBitText;
            _bitsTextBlocks.Add(textBlock);
            Children.Add(textBlock);
            Grid.SetColumn(textBlock, i);
            Grid.SetRow(textBlock, 2);
        }
        //UpdateBitsTextBloks(10);
    }

    private void OnClickBitText(object sender, MouseButtonEventArgs e)
    {
        TextBlock textBlock = (TextBlock)sender;
        int bitNumber = _bitsTextBlocks.IndexOf(textBlock);
        RegisterValue ^= (1U << bitNumber);
    }

    private static void UpdateBitsTextBloks(DependencyObject @object, DependencyPropertyChangedEventArgs eventArgs)
    {

        List<TextBlock>? bitsTextBlocks = (@object as Grid32x2)?._bitsTextBlocks;
        if (bitsTextBlocks == null)
            return;

        uint value = (uint)eventArgs.NewValue;
        for (int i = 0; i < 32; i++)
        {
            bitsTextBlocks[i].Text = (value & (1U << i)) != 0 ? "1" : "0";
        }
    }


}
