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
        private readonly IMenuManager _menuManager;
        private readonly IStatusBarManager _statusBarManager;

        public ObservableCollection<MenuItemViewModel> MenuItems => _menuManager.MenuItems;

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

        public ObservableCollection<StatusBarItemViewModel> StatusBarItems => _statusBarManager.StatusBarItems;

        public ShellViewModel(ApplicationContext applicationContext, IMenuManager menuManager,
            IStatusBarManager statusBarManager, IEnumerable<ITool> tools)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _menuManager = menuManager ?? throw new ArgumentNullException(nameof(menuManager));
            _statusBarManager = statusBarManager ?? throw new ArgumentNullException(nameof(statusBarManager));

            applicationContext.ApplicationTitleChanged += OnApplicationTitleChanged;
            applicationContext.ApplicationExitRequested += OnApplicationExitRequested;
            applicationContext.DocumentLoadRequested += OnDocumentLoadRequested;
            OnApplicationTitleChanged(applicationContext, applicationContext.ApplicationTitle);
            OnDocumentLoadRequested(applicationContext, EventArgs.Empty);

            _menuManager.CompileMenu();
            _statusBarManager.CompileStatusBar();

            Tools = new ObservableCollection<ITool>(tools);
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
