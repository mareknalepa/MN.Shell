using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarAggregator : IStatusBarAggregator
    {
        public IEnumerable<StatusBarItemViewModel> ComposeStatusBar(IEnumerable<IStatusBarProvider> statusBarProviders)
        {
            return statusBarProviders.SelectMany(s => s.GetStatusBarItems())
                .OrderBy(s => s.Side)
                .ThenByDescending(s => s.Priority)
                .ThenByDescending(s => s.Content)
                .ToList();
        }
    }
}
