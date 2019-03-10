using MN.Shell.Core;
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
        }
    }
}
