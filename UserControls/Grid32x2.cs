using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SiliconRadarControlPanel.UserControls;

public class Grid32X2 : Grid
{
    private readonly List<TextBlock> _bitsTextBlocks;

    public static readonly DependencyProperty RegisterValueProperty = DependencyProperty.Register(
        "RegisterValue",
        typeof(uint),
        typeof(Grid32X2),
        new(0U, UpdateBitsText)
    );

    public uint RegisterValue
    {
        get => (uint)GetValue(RegisterValueProperty);
        set => SetValue(RegisterValueProperty, value);
    }

    public Grid32X2()
    {
        _bitsTextBlocks = new List<TextBlock>();
        for (int i = 0; i < 32; i++)
        {
            ColumnDefinition columnDefinitions = new()
            {
                Width = new GridLength(1.0, GridUnitType.Star)
            };
            ColumnDefinitions.Add(columnDefinitions);
        }

        RowDefinition rowDefinition = new();
        RowDefinitions.Add(rowDefinition);
        for (int i = 0; i < 2; i++)
        {
            rowDefinition = new RowDefinition
            {
                Height = new GridLength(1.0, GridUnitType.Star)
            };
            RowDefinitions.Add(rowDefinition);
        }

        for (int i = 31; i >= 0; i--)
        {
            TextBlock textBlock = new()
            {
                Text = "0",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            textBlock.PreviewMouseDown += OnClickBitText;
            _bitsTextBlocks.Add(textBlock);
            Children.Add(textBlock);
            SetColumn(textBlock, i);
            SetRow(textBlock, 2);
        }
    }

    private void OnClickBitText(object sender, MouseButtonEventArgs e)
    {
        TextBlock textBlock = (TextBlock)sender;
        int bitNumber = _bitsTextBlocks.IndexOf(textBlock);
        RegisterValue ^= (1U << bitNumber);
    }

    private static void UpdateBitsText(DependencyObject @object, DependencyPropertyChangedEventArgs eventArgs)
    {
        List<TextBlock>? bitsTextBlocks = (@object as Grid32X2)?._bitsTextBlocks;
        if (bitsTextBlocks == null)
            return;

        uint value = (uint)eventArgs.NewValue;
        for (int i = 0; i < 32; i++)
        {
            bitsTextBlocks[i].Text = (value & (1U << i)) != 0 ? "1" : "0";
        }
    }
}