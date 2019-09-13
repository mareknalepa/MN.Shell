using MN.Shell.Demo.ControlsDemo;
using MN.Shell.Demo.FolderExplorer;
using MN.Shell.Demo.Output;
using MN.Shell.Demo.ProgressBars;
using MN.Shell.Demo.TabbedInterface;
using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.StatusBar;
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

            Bind<ITool>().To<FolderExplorerViewModel>();
            Bind<ITool>().To<OutputViewModel>();

            Bind<IDocument>().To<ControlsDemoViewModel>();
            Bind<IDocument>().To<ProgressBarsViewModel>();
            Bind<IDocument>().To<TabbedInterfaceViewModel>();
        }
    }
}
