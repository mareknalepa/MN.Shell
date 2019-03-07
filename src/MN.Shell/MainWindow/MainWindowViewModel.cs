using Caliburn.Micro;
using MN.Shell.Core;
using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.MainWindow
{
    public class MainWindowViewModel : Screen, IShell
    {
        private readonly ILog _log;

        public MainWindowViewModel(ILog log)
        {
            _log = log;
            _log.Info("Creating MainWindowViewModel instance...");

            DisplayName = "MN.Shell";
        }

        private object _content;

        public object Content
        {
            get => _content;
            set { _content = value; NotifyOfPropertyChange(); }
        }
    }
}
