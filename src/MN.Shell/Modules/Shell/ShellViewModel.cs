using Caliburn.Micro;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MN.Shell.Modules.Shell
{
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        private readonly ILog _log;

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

        public ShellViewModel(ILog log, IMenuAggregator menuAggregator,
            IEnumerable<IMenuProvider> menuProviders, IEnumerable<ITool> tools, IEnumerable<IDocument> documents)
        {
            _log = log;
            _log.Info("Creating ShellViewModel instance...");

            DisplayName = "MN.Shell";

            MenuItems = new ObservableCollection<MenuItemViewModel>(menuAggregator.ComposeMenu(menuProviders));

            Tools = new ObservableCollection<ITool>(tools);
            Items.AddRange(documents);
        }
    }
}
