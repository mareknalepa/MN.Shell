using MN.Shell.Core.Framework;
using MN.Shell.Core.MainWindow;
using MN.Shell.Demo.DemoPanel;
using Ninject;
using Ninject.Modules;

namespace MN.Shell.Demo
{
    public class DemoModule : NinjectModule
    {
        public override string Name => "MN.Shell.Demo";

        public override void Load()
        {
        }

        public override void VerifyRequiredModulesAreLoaded()
        {
            IShell shell = Kernel.Get<IShell>();
            if (shell is MainWindowViewModel mwvm)
                mwvm.Content = new DemoPanelViewModel();
        }
    }
}
