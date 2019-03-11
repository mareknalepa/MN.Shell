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
        }
    }
}
