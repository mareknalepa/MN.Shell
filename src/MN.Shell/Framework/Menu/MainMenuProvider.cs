using MN.Shell.Framework.ColorSchemes;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System.Collections.Generic;

namespace MN.Shell.Framework.Menu
{
    public class MainMenuProvider : IMenuProvider
    {
        private readonly IApplicationContext _applicationContext;
        private readonly IColorSchemeLoader _colorSchemeLoader;

        public MainMenuProvider(IApplicationContext applicationContext, IColorSchemeLoader colorSchemeLoader)
        {
            _applicationContext = applicationContext;
            _colorSchemeLoader = colorSchemeLoader;
        }

        public IEnumerable<IMenuItem> GetMenuItems()
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
                Name = "Settings",
                GroupId = 0,
                GroupOrder = 40,
            };

            yield return new MenuItem()
            {
                Name = "Help",
                GroupId = 0,
                GroupOrder = 50,
            };

            yield return new MenuItem()
            {
                Name = "Exit",
                Path = new[] { "File" },
                GroupId = 100,
                GroupOrder = 100,
                Command = new Command(() => _applicationContext.RequestApplicationExit()),
            };

            yield return new MenuItem()
            {
                Name = "Base Colors",
                Path = new[] { "Settings" },
                GroupId = 10,
                GroupOrder = 10,
            };

            foreach (var baseColors in _colorSchemeLoader.AvailableBaseColors)
            {
                int groupOrder = 0;

                yield return new MenuItem()
                {
                    Name = baseColors.Name,
                    Path = new[] { "Settings", "Base Colors" },
                    GroupId = 10,
                    GroupOrder = groupOrder++,
                    Command = new Command(() => _colorSchemeLoader.LoadBaseColors(baseColors)),
                };
            }

            yield return new MenuItem()
            {
                Name = "Accent Color",
                Path = new[] { "Settings" },
                GroupId = 10,
                GroupOrder = 20,
            };

            foreach (var accentColors in _colorSchemeLoader.AvailableAccentColors)
            {
                int groupOrder = 0;

                yield return new MenuItem()
                {
                    Name = accentColors.Name,
                    Path = new[] { "Settings", "Accent Color" },
                    GroupId = 10,
                    GroupOrder = groupOrder++,
                    Command = new Command(() => _colorSchemeLoader.LoadAccentColors(accentColors)),
                };
            }
        }
    }
}
