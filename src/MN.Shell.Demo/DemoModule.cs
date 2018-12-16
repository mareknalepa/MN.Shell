using MN.Shell.Core.Framework;
using MN.Shell.Demo.DemoPanel;
using Ninject.Modules;

namespace MN.Shell.Demo
{
    public class DemoModule : NinjectModule
    {
        public override string Name => "MN.Shell.Demo";

        public override void Load()
        {
            Bind<IShellContent>().To<DemoPanelViewModel>();
        }
    }
}
