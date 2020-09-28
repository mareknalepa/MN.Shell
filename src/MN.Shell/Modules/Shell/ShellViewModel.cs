using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using MN.Shell.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.Shell
{
    public class ShellViewModel : ItemsConductorOneActive<IDocument>
    {
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

                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<StatusBarItemViewModel> StatusBarItems { get; }

        public ShellViewModel(IMenuAggregator menuAggregator, IEnumerable<IMenuProvider> menuProviders,
            MainMenuProvider mainMenuProvider, IStatusBarAggregator statusBarAggregator,
            IEnumerable<IStatusBarProvider> statusBarProviders, IEnumerable<ITool> tools,
            IEnumerable<IDocument> documents)
        {
            Title = "MN.Shell";

            mainMenuProvider.ApplicationExitHandler = () => RequestClose();

            MenuItems = new ObservableCollection<MenuItemViewModel>(menuAggregator.ComposeMenu(menuProviders));

            Tools = new ObservableCollection<ITool>(tools);
            foreach (var document in documents)
                ItemsCollection.Add(document);

            StatusBarItems = new ObservableCollection<StatusBarItemViewModel>(
                statusBarAggregator.ComposeStatusBar(statusBarProviders));
        }
    }
}
