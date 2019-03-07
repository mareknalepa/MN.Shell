using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Controls
{
    public class DialogButtons : ItemsControl
    {
        static DialogButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogButtons),
                new FrameworkPropertyMetadata(typeof(DialogButtons)));
        }
    }
}
