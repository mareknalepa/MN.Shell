using MN.Shell.PluginContracts;
using System.Collections.Generic;

namespace MN.Shell.Framework.StatusBar
{
    public interface IStatusBarAggregator
    {
        IEnumerable<IStatusBarItem> ComposeStatusBar(IEnumerable<IStatusBarProvider> statusBarProviders);
    }
}
