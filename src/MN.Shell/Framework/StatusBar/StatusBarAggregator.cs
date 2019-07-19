using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarAggregator : IStatusBarAggregator
    {
        private static readonly IComparer<StatusBarItemViewModel> _statusBarItemComparer = new StatusBarItemComparer();

        public IEnumerable<StatusBarItemViewModel> ComposeStatusBar(IEnumerable<IStatusBarProvider> statusBarProviders)
        {
            return statusBarProviders.SelectMany(s => s.GetStatusBarItems())
                .OrderBy(s => s, _statusBarItemComparer)
                .ToList();
        }
    }
}
