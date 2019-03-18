using MN.Shell.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Modules.MainMenu
{
    public class MainMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            yield return new MenuItem()
            {
                Name = "File",
                GroupId = 0,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Edit",
                GroupId = 0,
                GroupOrder = 20,
            };

            yield return new MenuItem()
            {
                Name = "View",
                GroupId = 0,
                GroupOrder = 30,
            };

            yield return new MenuItem()
            {
                Name = "Help",
                GroupId = 0,
                GroupOrder = 40,
            };

            yield return new MenuItem()
            {
                Name = "Exit",
                Path = new[] { "File" },
                GroupId = 100,
                GroupOrder = 100,
            };
        }
    }
}
