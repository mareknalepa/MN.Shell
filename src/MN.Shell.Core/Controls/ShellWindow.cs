using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MN.Shell.Core.Controls
{
    public class ShellWindow : Window
    {
        static ShellWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShellWindow),
                new FrameworkPropertyMetadata(typeof(ShellWindow)));
        }
    }
}
