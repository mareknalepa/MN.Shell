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
