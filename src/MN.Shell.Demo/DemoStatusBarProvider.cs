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
                Priority = 90,
                Content = "Ready",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Left,
                Priority = 70,
                Content = "Demo item 1",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Left,
                Priority = 50,
                Content = "Demo item 2",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 60,
                Content = "MN.Shell",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 70,
                Content = "Help",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 90,
                Content = "Notifications",
            };
        }
    }
}
