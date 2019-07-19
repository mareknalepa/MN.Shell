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
            Bind<IMenuAggregator>().To<MenuAggregator>();
            Bind<IStatusBarAggregator>().To<StatusBarAggregator>();
        }
    }
}
