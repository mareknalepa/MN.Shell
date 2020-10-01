using MN.Shell.Demo.ControlsDemo;
using MN.Shell.Demo.TabbedInterface;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
using MN.Shell.PluginContracts;
using Ninject.Modules;

namespace MN.Shell.Demo
{
    public class DemoModule : NinjectModule
    {
        public override string Name => "MN.Shell.Demo";

        public override void Load()
        {
            Bind<IMenuProvider>().To<DemoMenuProvider>();
            Bind<IStatusBarProvider>().To<DemoStatusBarProvider>();

            Bind<IDocument>().To<ControlsDemoViewModel>();
            Bind<IDocument>().To<TabbedInterfaceViewModel>();
        }
    }
}
