using Caliburn.Micro;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.Shell
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        private readonly ILog _log;

        private bool _isClosing;

        public ObservableCollection<MenuItemViewModel> MenuItems { get; }

        public ObservableCollection<ITool> Tools { get; }

        private ILayoutModule _activeLayoutModule;

        public ILayoutModule ActiveLayoutModule
        {
            get => _activeLayoutModule;
            set
            {
                _activeLayoutModule = value;
                if (_activeLayoutModule is IDocument document)
                    ActivateItem(document);

                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<StatusBarItemViewModel> StatusBarItems { get; }

        public ShellViewModel(ILog log, IMenuAggregator menuAggregator, IEnumerable<IMenuProvider> menuProviders,
            IStatusBarAggregator statusBarAggregator, IEnumerable<IStatusBarProvider> statusBarProviders,
            IEnumerable<ITool> tools, IEnumerable<IDocument> documents)
        {
            _log = log;
            _log.Info("Creating ShellViewModel instance...");

            DisplayName = "MN.Shell";

            MenuItems = new ObservableCollection<MenuItemViewModel>(menuAggregator.ComposeMenu(menuProviders));

            Tools = new ObservableCollection<ITool>(tools);
            Items.AddRange(documents);

            StatusBarItems = new ObservableCollection<StatusBarItemViewModel>(
                statusBarAggregator.ComposeStatusBar(statusBarProviders));
        }

        public override void ActivateItem(IDocument item)
        {
            if (_isClosing)
                return;

            base.ActivateItem(item);
        }

        protected override void OnDeactivate(bool close)
        {
            _isClosing = true;

            base.OnDeactivate(close);
        }
    }
}
