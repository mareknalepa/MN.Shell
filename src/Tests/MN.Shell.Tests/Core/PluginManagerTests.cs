using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.PluginContracts;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Tests.Core
{
    public sealed class PluginManagerTests : IDisposable
    {
        private readonly IScopedPluginContext _context = Substitute.For<IScopedPluginContext>();
        private readonly IApplicationContext _applicationContext = Substitute.For<IApplicationContext>();
        private readonly PluginManager _pluginManager = new(NullLogger<PluginManager>.Instance);

        public void Dispose()
        {
            _pluginManager.Dispose();
        }

        [Fact]
        public void LoadPlugins_CallsLoadOnPlugins()
        {
            var plugin1 = Substitute.For<IPlugin>();
            var plugin2 = Substitute.For<IPlugin>();

            _pluginManager.Plugins.ShouldNotBeNull();
            _pluginManager.Plugins.ShouldBeEmpty();

            _pluginManager.LoadPlugins([plugin1, plugin2], _context);

            _pluginManager.Plugins.ShouldContain(plugin1);
            _pluginManager.Plugins.ShouldContain(plugin2);

            plugin1.Received(1).Load(_context);
            plugin2.Received(1).Load(_context);
        }

        [Fact]
        public void OnStartup_CallsOnStartupOnPlugins()
        {
            // Hack to create instance of StartupEventArgs in tests:
            var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke(null) is not StartupEventArgs startupEventArgs)
            {
                throw new InvalidOperationException($"Cannot create {nameof(StartupEventArgs)} instance");
            }

            var plugin1 = Substitute.For<IPlugin>();
            var plugin2 = Substitute.For<IPlugin>();

            _pluginManager.LoadPlugins([plugin1, plugin2], _context);
            _pluginManager.OnStartup(startupEventArgs, _applicationContext);

            plugin1.Received(1).OnStartup(startupEventArgs, _applicationContext);
            plugin2.Received(1).OnStartup(startupEventArgs, _applicationContext);
        }

        [Fact]
        public void OnExit_CallsOnExitOnPlugins()
        {
            // Hack to create instance of ExitEventArgs in tests:
            var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke([0]) is not ExitEventArgs exitEventArgs)
            {
                throw new InvalidOperationException($"Cannot create {nameof(ExitEventArgs)} instance");
            }

            var plugin1 = Substitute.For<IPlugin>();
            var plugin2 = Substitute.For<IPlugin>();

            _pluginManager.LoadPlugins([plugin1, plugin2], _context);
            _pluginManager.OnExit(exitEventArgs, _applicationContext);

            plugin1.Received(1).OnExit(exitEventArgs, _applicationContext);
            plugin2.Received(1).OnExit(exitEventArgs, _applicationContext);
        }

        [Fact]
        public void Dispose_DisposesPlugins()
        {
            var plugin1 = Substitute.For<IPlugin, IDisposable>();
            var plugin2 = Substitute.For<IPlugin>();

            _pluginManager.LoadPlugins([plugin1, plugin2], _context);
            _pluginManager.Dispose();

            ((IDisposable)plugin1).Received(1).Dispose();
        }
    }
}
