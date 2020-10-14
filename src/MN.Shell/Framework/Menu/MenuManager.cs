using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MN.Shell.Framework.Menu
{
    public class MenuManager : IMenuManager, IMenuBuilder
    {
        private readonly IEnumerable<IMenuProvider> _menuProviders;

        public MenuManager(IEnumerable<IMenuProvider> menuProviders)
        {
            _menuProviders = menuProviders;
            foreach (var menuProvider in _menuProviders)
                menuProvider.BuildMenu(this);
        }

        #region "Menu builder"

        protected MenuItemDefinition RootItemDefinition { get; } = new MenuItemDefinition("ROOT");

        public IMenuItemBuilder AddItem(string path, string localizedName)
        {
            var currentDefinition = RootItemDefinition;

            foreach (var pathComponent in ParsePath(path))
            {
                var subItem = currentDefinition.SubItems.FirstOrDefault(d => d.Name == pathComponent);
                if (subItem == null)
                {
                    subItem = new MenuItemDefinition(pathComponent);
                    currentDefinition.SubItems.Add(subItem);
                }
                currentDefinition = subItem;
            }

            currentDefinition.LocalizedName = localizedName;

            return currentDefinition;
        }

        public void RemoveItem(string path, bool forceRemoveIfNonEmpty = false)
        {
            var parentDefinition = RootItemDefinition;
            var currentDefinition = RootItemDefinition;

            foreach (var pathComponent in ParsePath(path))
            {
                var subItem = currentDefinition.SubItems.FirstOrDefault(d => d.Name == pathComponent);
                if (subItem == null)
                {
                    parentDefinition = null;
                    break;
                }

                parentDefinition = currentDefinition;
                currentDefinition = subItem;
            }

            if (parentDefinition != null && (currentDefinition.SubItems.Count == 0 || forceRemoveIfNonEmpty))
                parentDefinition.SubItems.Remove(currentDefinition);
        }

        private static IEnumerable<string> ParsePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Cannot use empty path in menu builder");

            return path.Split('/').Where(p => !string.IsNullOrWhiteSpace(p));
        }

        #endregion

        #region "Menu compiling"

        protected MenuItemViewModel RootViewModel { get; } = new MenuItemViewModel();

        public ObservableCollection<MenuItemViewModel> MenuItems => RootViewModel.SubItems;

        public void CompileMenu()
        {
            RootViewModel.SubItems.Clear();
            ProcessItemDefinition(RootItemDefinition, RootViewModel);
        }

        private void ProcessItemDefinition(MenuItemDefinition itemDefinition, MenuItemViewModel parent)
        {
            var subItems = itemDefinition.SubItems.OrderBy(s => s.Section).ThenBy(s => s.Order).ToList();
            if (subItems.Count > 0)
            {
                int currentSection = subItems[0].Section;
                foreach (var subItem in subItems)
                {
                    if (subItem.Section != currentSection)
                    {
                        parent.SubItems.Add(new MenuItemViewModel() { IsSeparator = true });
                        currentSection = subItem.Section;
                    }

                    var newViewModel = new MenuItemViewModel()
                    {
                        Name = string.IsNullOrEmpty(subItem.LocalizedName) ? subItem.Name : subItem.LocalizedName,
                        Command = subItem.Command,
                        IsCheckable = subItem.IsCheckbox,
                    };

                    parent.SubItems.Add(newViewModel);
                    ProcessItemDefinition(subItem, newViewModel);
                }
            }
        }

        #endregion
    }
}
