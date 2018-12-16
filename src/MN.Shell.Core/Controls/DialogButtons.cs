using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Core.Controls
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
