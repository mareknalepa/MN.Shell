using MN.Shell.Core;
using Ninject.Modules;

namespace MN.Shell.Tests.Mocks
{
    public class MockModule : NinjectModule
    {
        public override string Name => "MN.Shell.Tests.Mock";

        public override void Load()
        {
            Rebind<IShell>().To<MockShell>();

            Bind<IMockService>().To<MockService1>();
            Bind<IMockService>().To<MockService2>();
        }
    }
}
