using MN.Shell.Core;
using MN.Shell.Framework.Shell;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Framework
{
    public class FrameworkModule : NinjectModule
    {
        public override string Name => "MN.Shell.Framework";

        public override void Load()
        {
            Bind<IShell>().To<ShellViewModel>();
        }
    }
}
