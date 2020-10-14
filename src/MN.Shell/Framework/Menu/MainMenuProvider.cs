using MN.Shell.Framework.ColorSchemes;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using MN.Shell.Properties;
using System;

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

        public void BuildMenu(IMenuBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder
                .AddItem("File", Resources.MenuFile)
                .SetPlacement(0, 10);

            builder
                .AddItem("Edit", Resources.MenuEdit)
                .SetPlacement(0, 20);

            builder
                .AddItem("View", Resources.MenuView)
                .SetPlacement(0, 30);

            builder
                .AddItem("Help", Resources.MenuHelp)
                .SetPlacement(0, 40);

            builder
                .AddItem("File/Exit", Resources.MenuExit)
                .SetPlacement(100, 100)
                .SetCommand(new Command(() => _applicationContext.RequestApplicationExit()));

            builder
                .AddItem("View/Theme", Resources.MenuTheme)
                .SetPlacement(30, 10);

            foreach (var baseColors in _colorSchemeLoader.AvailableBaseColors)
            {
                int groupOrder = 0;

                builder
                    .AddItem($"View/Theme/{baseColors.Name}", baseColors.LocalizedName)
                    .SetPlacement(10, groupOrder++)
                    .SetCommand(new Command(() => _colorSchemeLoader.LoadBaseColors(baseColors)));
            }

            builder
                .AddItem("View/Accent", Resources.MenuAccent)
                .SetPlacement(30, 20);

            foreach (var accentColors in _colorSchemeLoader.AvailableAccentColors)
            {
                int groupOrder = 0;

                builder
                    .AddItem($"View/Accent/{accentColors.Name}", accentColors.LocalizedName)
                    .SetPlacement(10, groupOrder++)
                    .SetCommand(new Command(() => _colorSchemeLoader.LoadAccentColors(accentColors)));
            }
        }
    }
}
