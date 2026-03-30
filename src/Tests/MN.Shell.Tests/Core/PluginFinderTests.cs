using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.PluginContracts;
using System.IO;
using System.Reflection;

namespace MN.Shell.Tests.Core
{
    public sealed class PluginFinderTests
    {
        private readonly PluginFinder _pluginFinder = new(NullLogger<PluginFinder>.Instance);

        [Fact]
        public void DiscoverPlugins_ReturnsCorrectResult()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var discoveredPlugins = _pluginFinder.DiscoverPlugins(path);

            discoveredPlugins.ShouldNotBeNull();
            discoveredPlugins.ShouldNotBeEmpty();
            discoveredPlugins.ShouldContain(p => p.GetType() == typeof(PluginLoaderTestsExamplePlugin));
        }
    }

    public sealed class PluginLoaderTestsExamplePlugin : PluginBase
    {
        protected override void OnLoad() { }
    }
}
