using Caliburn.Micro;
using MN.Shell.Core.Dialogs;
using MN.Shell.Core.Framework;
using MN.Shell.Core.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MN.Shell.Demo.DemoPanel
{
    public class DemoPanelViewModel : PropertyChangedBase, IShellContent
    {
        private readonly IWindowManager _windowManager;

        public DemoPanelViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void RunDialog()
        {
            _windowManager.ShowDialog(new MessageBoxViewModel("Info", "Hello world!", new[] { DialogButtonType.Ok, DialogButtonType.Cancel }));
        }
    }
}
