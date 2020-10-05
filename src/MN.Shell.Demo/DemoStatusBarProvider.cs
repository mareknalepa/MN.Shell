using MN.Shell.Framework.StatusBar;
using MN.Shell.PluginContracts;
using System.Collections.Generic;

namespace MN.Shell.Demo
{
    public class DemoStatusBarProvider : IStatusBarProvider
    {
        public IEnumerable<IStatusBarItem> GetStatusBarItems()
        {
            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Left,
                Priority = 100,
                MinWidth = 200,
                Content = "Ready",
            };
        }
    }
}
