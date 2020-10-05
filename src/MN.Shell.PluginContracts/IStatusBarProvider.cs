using System.Collections.Generic;

namespace MN.Shell.PluginContracts
{
    public interface IStatusBarProvider
    {
        IEnumerable<IStatusBarItem> GetStatusBarItems();
    }
}
