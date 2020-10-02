using MN.Shell.Core;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.Shell
{
    public class ShellViewModel : ItemsConductorOneActive<IDocument>
    {
        private readonly ApplicationContext _applicationContext;
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

        public ShellViewModel(ApplicationContext applicationContext, IMenuAggregator menuAggregator,
            IEnumerable<IMenuProvider> menuProviders, IStatusBarAggregator statusBarAggregator,
            IEnumerable<IStatusBarProvider> statusBarProviders, IEnumerable<ITool> tools)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));

            applicationContext.ApplicationTitleChanged += OnApplicationTitleChanged;
            applicationContext.ApplicationExitRequested += OnApplicationExitRequested;
            applicationContext.DocumentLoadRequested += OnDocumentLoadRequested;
            OnApplicationTitleChanged(applicationContext, applicationContext.ApplicationTitle);
            OnDocumentLoadRequested(applicationContext, EventArgs.Empty);

            MenuItems = new ObservableCollection<MenuItemViewModel>(menuAggregator.ComposeMenu(menuProviders));

            Tools = new ObservableCollection<ITool>(tools);

            StatusBarItems = new ObservableCollection<StatusBarItemViewModel>(
                statusBarAggregator.ComposeStatusBar(statusBarProviders));
        }

        private void OnApplicationTitleChanged(object sender, string newTitle)
        {
            if (!string.IsNullOrEmpty(newTitle))
                Title = newTitle;
        }

        private void OnApplicationExitRequested(object sender, EventArgs _) => RequestClose();

        private void OnDocumentLoadRequested(object sender, EventArgs _)
        {
            while (_applicationContext.DocumentsToLoad.Count > 0)
                ActivateItem(_applicationContext.DocumentsToLoad.Dequeue());
        }
    }
}
