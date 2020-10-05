using MN.Shell.Framework.StatusBar;
using Ninject.Modules;

namespace MN.Shell.Demo
{
    public class DemoModule : NinjectModule
    {
        public override string Name => "MN.Shell.Demo";

        public override void Load()
        {
            Bind<IStatusBarProvider>().To<DemoStatusBarProvider>();
        }
    }
}
