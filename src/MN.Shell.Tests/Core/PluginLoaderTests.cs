using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class PluginLoaderTests
    {
        public static bool OnLoadCalled { get; set; }

        [Test]
        public void PluginDiscoveredAndLoadedTest()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var mockPluginLoaderContext = new Mock<IPluginLoaderContext>();

            OnLoadCalled = false;
            PluginLoader.DiscoverAndLoadPlugins(path, mockPluginLoaderContext.Object);
            Assert.True(OnLoadCalled);
        }
    }

    public class PluginLoaderTestsExamplePlugin : PluginBase
    {
        protected override void OnLoad() => PluginLoaderTests.OnLoadCalled = true;
    }
}
