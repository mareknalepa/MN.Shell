using MN.Shell.Core;
using MN.Shell.Framework.Menu;
using MN.Shell.Modules.MainMenu;
using MN.Shell.Modules.Shell;
using Ninject.Modules;

namespace MN.Shell.Modules
{
    public class MainModule : NinjectModule
    {
        public override string Name => "MN.Shell.MainModule";

        public override void Load()
        {
            Bind<IShell>().To<ShellViewModel>();

            Bind<IMenuProvider>().To<MainMenuProvider>();
        }
    }
}
