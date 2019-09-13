using MN.Shell.Core;
using MN.Shell.Framework.StatusBar;
using System.Collections.Generic;
using System.Windows;

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
                MinWidth = 400,
                Content = "Ready",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Left,
                Priority = 70,
                MinWidth = 200,
                Content = "Demo item 1",
                CommandAction = () =>
                {
                    MessageBox.Show("Demo item 1");
                },
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Left,
                Priority = 50,
                MinWidth = 200,
                Content = "Demo item 2",
                CommandAction = () =>
                {
                    MessageBox.Show("Demo item 2");
                },
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 60,
                MinWidth = 100,
                Content = "MN.Shell",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 70,
                MinWidth = 100,
                Content = "Help",
            };

            yield return new StatusBarItemViewModel()
            {
                Side = StatusBarSide.Right,
                Priority = 90,
                MinWidth = 120,
                Content = "Notifications",
            };
        }
    }
}
