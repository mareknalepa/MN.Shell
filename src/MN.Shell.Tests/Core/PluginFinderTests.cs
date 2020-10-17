using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.PluginContracts;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class PluginFinderTests
    {
        private PluginFinder _pluginFinder;

        [SetUp]
        public void SetUp()
        {
            _pluginFinder = new PluginFinder(NullLogger.Instance);
        }

        [Test]
        public void PluginDiscoveredTest()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var discoveredPlugins = _pluginFinder.DiscoverPlugins(path);

            Assert.NotNull(discoveredPlugins);
            Assert.IsNotEmpty(discoveredPlugins);
            Assert.True(discoveredPlugins.Any(p => p.GetType() == typeof(PluginLoaderTestsExamplePlugin)));
        }
    }

    public class PluginLoaderTestsExamplePlugin : PluginBase
    {
        protected override void OnLoad() { }
    }
}
