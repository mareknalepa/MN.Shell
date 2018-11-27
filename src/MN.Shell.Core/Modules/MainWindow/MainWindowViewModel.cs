﻿using Caliburn.Micro;
using MN.Shell.Core.Framework;

namespace MN.Shell.Core.Modules.MainWindow
{
    public class MainWindowViewModel : PropertyChangedBase, IShell, IHaveDisplayName
    {
        private readonly ILog _log;

        public MainWindowViewModel(ILog log)
        {
            _log = log;
            _log.Info("Creating MainWindowViewModel instance...");

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
