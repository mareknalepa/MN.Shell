using MN.Shell.PluginContracts;
using System.Collections.Generic;

namespace MN.Shell.Framework.Menu
{
    public interface IMenuAggregator
    {
        IEnumerable<MenuItemViewModel> ComposeMenu(IEnumerable<IMenuProvider> menuProviders);
    }
}
