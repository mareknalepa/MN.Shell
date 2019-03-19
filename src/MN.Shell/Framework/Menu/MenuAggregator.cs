using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Framework.Menu
{
    public class MenuAggregator : IMenuAggregator
    {
        private static readonly IComparer<MenuItem> _menuItemComparer = new MenuItemComparer();

        public IEnumerable<MenuItemViewModel> ComposeMenu(IEnumerable<IMenuProvider> menuProviders)
        {
            var aggregatedMenuItems = AggregateMenuItems(menuProviders);

            var viewModels = CreateViewModels(aggregatedMenuItems);

            return viewModels;
        }

        private IEnumerable<MenuItem> AggregateMenuItems(IEnumerable<IMenuProvider> menuProviders)
        {
            IEnumerable<MenuItem> result = menuProviders
                .SelectMany(menuProvider => menuProvider.GetMenuItems())
                .OrderBy(menuItem => menuItem, _menuItemComparer);

            return result;
        }

        private IEnumerable<MenuItemViewModel> CreateViewModels(IEnumerable<MenuItem> menuItems)
        {
            var rootViewModel = new MenuItemViewModel()
            {
                Name = "[ROOT]",
            };

            var prevParent = rootViewModel;
            int prevGroupId = 0;

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Command != null && menuItem.IsCheckable)
                    throw new InconsistentMenuException("Item cannot have command and be checkable at the same time");

                if (menuItem.Path?.Length > 0)
                {
                    var viewModel = CreateViewModelForItem(menuItem);
                    var parent = ResolveParentForItem(rootViewModel, menuItem);

                    if (parent.Command != null || parent.IsCheckable)
                        throw new InconsistentMenuException("Command or selectable item cannot have subitems");

                    if (parent == prevParent && menuItem.GroupId != prevGroupId)
                    {
                        parent.SubItems.Add(CreateViewModelForSeparator());
                    }

                    parent.SubItems.Add(viewModel);

                    prevParent = parent;
                    prevGroupId = menuItem.GroupId;
                }
                else
                {
                    rootViewModel.SubItems.Add(CreateViewModelForItem(menuItem));
                }
            }

            return rootViewModel.SubItems;
        }

        private MenuItemViewModel CreateViewModelForItem(MenuItem menuItem)
        {
            return new MenuItemViewModel()
            {
                Name = menuItem.Name,
                Icon = menuItem.Icon,
                IsCheckable = menuItem.IsCheckable,
                OnIsCheckedChanged = menuItem.OnIsCheckedChanged,
                Command = menuItem.Command,
            };
        }

        private MenuItemViewModel CreateViewModelForSeparator()
        {
            return new MenuItemViewModel()
            {
                IsSeparator = true,
            };
        }

        private MenuItemViewModel ResolveParentForItem(MenuItemViewModel rootViewModel, MenuItem menuItem)
        {
            var parent = rootViewModel;
            foreach (var pathComponent in menuItem.Path)
            {
                parent = parent.SubItems.FirstOrDefault(vm => vm.Name == pathComponent);
                if (parent == null)
                    throw new InconsistentMenuException("Cannot find parent item");
            }

            return parent;
        }
    }
}
