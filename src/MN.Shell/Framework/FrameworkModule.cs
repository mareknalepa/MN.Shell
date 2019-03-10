using MN.Shell.Framework.Menu;
using Ninject.Modules;

namespace MN.Shell.Framework
{
    public class FrameworkModule : NinjectModule
    {
        public override string Name => "MN.Shell.Framework";

        public override void Load()
        {
            Bind<IMenuAggregator>().To<MenuAggregator>();
        }
    }
}
