using MN.Shell.Framework.StatusBar;
using System.Collections.Generic;

namespace MN.Shell.Demo
{
    public class DemoStatusBarProvider : IStatusBarProvider
    {
        public IEnumerable<StatusBarItemViewModel> GetStatusBarItems()
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
