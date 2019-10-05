using MN.Shell.Framework.ColorSchemes;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using Ninject.Modules;

namespace MN.Shell.Framework
{
    public class FrameworkModule : NinjectModule
    {
        public override string Name => "MN.Shell.Framework";

        public override void Load()
        {
            Bind<IColorSchemeLoader>().To<ColorSchemeLoader>();
            Bind<IMenuAggregator>().To<MenuAggregator>();
            Bind<IStatusBarAggregator>().To<StatusBarAggregator>();
        }
    }
}
