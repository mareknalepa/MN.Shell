using System.Collections.Generic;

namespace MN.Shell.Framework.StatusBar
{
    public interface IStatusBarAggregator
    {
        IEnumerable<StatusBarItemViewModel> ComposeStatusBar(IEnumerable<IStatusBarProvider> statusBarProviders);
    }
}
