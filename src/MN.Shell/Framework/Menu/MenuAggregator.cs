using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Framework.Menu
{
    public class MenuAggregator : IMenuAggregator
    {
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
                .OrderBy(menuItem => menuItem.Path?.Length ?? 0)
                .ThenBy(menuItem => menuItem.GroupId)
                .ThenBy(menuItem => menuItem.GroupOrder);

            return result;
        }

        private IEnumerable<MenuItemViewModel> CreateViewModels(IEnumerable<MenuItem> menuItems)
        {
            var rootViewModel = new MenuItemViewModel()
            {
                Name = "[ROOT]",
            };

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Path?.Length > 0)
                {
                    var viewModel = CreateViewModelForItem(menuItem);
                    var parent = ResolveParentForItem(rootViewModel, menuItem);
                    parent.SubItems.Add(viewModel);
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
                Command = menuItem.Command,
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
