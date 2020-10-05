using System.Collections.Generic;

namespace MN.Shell.PluginContracts
{
    public interface IMenuProvider
    {
        IEnumerable<IMenuItem> GetMenuItems();
    }
}
