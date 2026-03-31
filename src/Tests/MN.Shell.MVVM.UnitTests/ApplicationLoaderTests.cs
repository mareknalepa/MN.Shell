using System.Windows;

namespace MN.Shell.MVVM.UnitTests
{
    public sealed class ApplicationLoaderTests
    {
        [Fact]
        public void BootstrapperSetter_CallsSetup()
        {
            var application = new Application();

            var bootstrapper = Substitute.For<IBootstrapper>();

            _ = new ApplicationLoader
            {
                Bootstrapper = bootstrapper
            };

            bootstrapper.Received(1).Setup(application);
        }
    }
}
