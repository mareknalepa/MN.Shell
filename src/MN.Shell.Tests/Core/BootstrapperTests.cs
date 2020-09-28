using MN.Shell.Core;
using MN.Shell.MVVM;
using MN.Shell.Tests.Mocks;
using NUnit.Framework;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class BootstrapperTests
    {
        [Test]
        public void BootstrapperGetInstanceTest()
        {
            using (var bootstrapper = new MockBootstrapper())
            {
                bootstrapper.Configure();
                var instance = bootstrapper.GetInstance<IWindowManager>();

                Assert.NotNull(instance);
                Assert.AreEqual(typeof(ShellWindowManager), instance.GetType());
            }
        }
    }
}
