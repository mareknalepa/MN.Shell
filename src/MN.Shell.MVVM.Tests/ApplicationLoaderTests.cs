using System.Windows;
using Moq;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class ApplicationLoaderTests

    {
        [Test]
        public void ApplicationLoaderDictionaryBootstrapperSetupTest()
        {
            var application = new Application();

            var bootstrapperMock = new Mock<IBootstrapper>();
            bootstrapperMock.Setup(b => b.Setup(application)).Verifiable();

            var appLoader = new ApplicationLoader
            {
                Bootstrapper = bootstrapperMock.Object
            };

            bootstrapperMock.VerifyAll();
        }
    }
}
