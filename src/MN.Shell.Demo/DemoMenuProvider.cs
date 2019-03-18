using MN.Shell.Framework.Menu;
using System.Collections.Generic;

namespace MN.Shell.Demo
{
    public class DemoMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            yield return new MenuItem()
            {
                Name = "Select All",
                Path = new[] { "Edit" },
                GroupId = 20,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "New Demo Project...",
                Path = new[] { "File" },
                GroupId = 10,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Demo",
                GroupId = 0,
                GroupOrder = 35,
            };

            yield return new MenuItem()
            {
                Name = "Cut",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Copy",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 20,
            };

            yield return new MenuItem()
            {
                Name = "Paste",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 30,
            };

            yield return new MenuItem()
            {
                Name = "Run Demo...",
                Path = new[] { "Demo" },
                GroupId = 10,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Preferences...",
                Path = new[] { "Edit" },
                GroupId = 30,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Demo mode",
                Path = new[] { "Demo" },
                GroupId = 10,
                GroupOrder = 5,
                IsCheckable = true,
                OnIsCheckedChanged = isChecked => System.Windows.MessageBox.Show($"New value: {isChecked}", "Info"),
            };
        }
    }
}
