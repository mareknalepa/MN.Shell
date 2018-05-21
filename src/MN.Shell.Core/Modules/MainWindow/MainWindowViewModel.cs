using MN.Shell.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Core.Modules.MainWindow
{
    [Export(typeof(IShell))]
    public class MainWindowViewModel : IShell
    {
    }
}
