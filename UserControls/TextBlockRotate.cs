using System.Windows.Controls;
using System.Windows.Media;

namespace SiliconRadarControlPanel.UserControls;

public class TextBlockRotate : TextBlock
{
    public TextBlockRotate()
    {
        LayoutTransform = new RotateTransform(-90, 0.5, 0.5);
    }
}