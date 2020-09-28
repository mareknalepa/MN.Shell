using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using MN.Shell.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.Shell
{
    public class ShellViewModel : ItemsConductorOneActive<IDocument>
    {
        private readonly ShellContext _shellContext;

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

        public ShellViewModel(ShellContext shellContext, IMenuAggregator menuAggregator,
            IEnumerable<IMenuProvider> menuProviders, IStatusBarAggregator statusBarAggregator,
            IEnumerable<IStatusBarProvider> statusBarProviders, IEnumerable<ITool> tools,
            IEnumerable<IDocument> documents)
        {
            _shellContext = shellContext;

            _shellContext.ApplicationTitleChanged += OnApplicationTitleChanged;
            _shellContext.ApplicationExitRequested += OnApplicationExitRequested;
            OnApplicationTitleChanged(_shellContext, _shellContext.ApplicationTitle);

            MenuItems = new ObservableCollection<MenuItemViewModel>(menuAggregator.ComposeMenu(menuProviders));

            Tools = new ObservableCollection<ITool>(tools);
            foreach (var document in documents)
                ItemsCollection.Add(document);

            StatusBarItems = new ObservableCollection<StatusBarItemViewModel>(
                statusBarAggregator.ComposeStatusBar(statusBarProviders));
        }

        private void OnApplicationTitleChanged(object sender, string newTitle)
        {
            if (!string.IsNullOrEmpty(newTitle))
                Title = newTitle;
        }

        private void OnApplicationExitRequested(object sender, EventArgs e) => RequestClose();
    }
}
