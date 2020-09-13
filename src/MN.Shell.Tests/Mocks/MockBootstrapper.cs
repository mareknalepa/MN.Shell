using MN.Shell.Core;

namespace MN.Shell.Tests.Mocks
{
    public class MockBootstrapper : Bootstrapper
    {
        public new void Configure() => base.Configure();

        public new T GetInstance<T>() => base.GetInstance<T>();
    }
}
