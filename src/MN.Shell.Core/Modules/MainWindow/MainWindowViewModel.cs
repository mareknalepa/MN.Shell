using Caliburn.Micro;
using MN.Shell.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Core.Modules.MainWindow
{
    public class MainWindowViewModel : PropertyChangedBase, IShell, IHaveDisplayName
    {
        public MainWindowViewModel()
        {
            DisplayName = "MN.Shell Main Window";
        }

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set { _displayName = value; NotifyOfPropertyChange(); }
        }
    }
}
