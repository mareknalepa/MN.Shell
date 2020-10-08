using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class PluginManagerTests
    {
        [Test]
        public void AddPluginsTest()
        {
            var mock1 = new Mock<IPlugin>();
            var mock2 = new Mock<IPlugin>();

            using (var pluginManager = new PluginManager())
            {
                Assert.NotNull(pluginManager.Plugins);
                Assert.IsEmpty(pluginManager.Plugins);

                pluginManager.AddPlugins(new[] { mock1.Object, mock2.Object });

                Assert.That(pluginManager.Plugins, Has.Exactly(2).Items);
                Assert.True(pluginManager.Plugins.Contains(mock1.Object));
                Assert.True(pluginManager.Plugins.Contains(mock2.Object));
            }
        }

        [Test]
        public void OnStartupTest()
        {
            // Hack to create instance of StartupEventArgs in tests:
            var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
            var e = constructorInfo.Invoke(null) as StartupEventArgs;

            var mock1 = new Mock<IPlugin>();
            mock1.Setup(p => p.OnStartup(It.IsAny<StartupEventArgs>())).Verifiable();

            var mock2 = new Mock<IPlugin>();
            mock2.Setup(p => p.OnStartup(It.IsAny<StartupEventArgs>())).Verifiable();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.AddPlugins(new[] { mock1.Object, mock2.Object });
                pluginManager.OnStartup(e);
            }

            mock1.Verify(p => p.OnStartup(It.IsAny<StartupEventArgs>()));
            mock2.Verify(p => p.OnStartup(It.IsAny<StartupEventArgs>()));
        }

        [Test]
        public void OnExitTest()
        {
            // Hack to create instance of ExitEventArgs in tests:
            var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
            var e = constructorInfo.Invoke(new object[] { 0 }) as ExitEventArgs;

            var mock1 = new Mock<IPlugin>();
            mock1.Setup(p => p.OnExit(It.IsAny<ExitEventArgs>())).Verifiable();

            var mock2 = new Mock<IPlugin>();
            mock2.Setup(p => p.OnExit(It.IsAny<ExitEventArgs>())).Verifiable();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.AddPlugins(new[] { mock1.Object, mock2.Object });
                pluginManager.OnExit(e);
            }

            mock1.Verify(p => p.OnExit(It.IsAny<ExitEventArgs>()));
            mock2.Verify(p => p.OnExit(It.IsAny<ExitEventArgs>()));
        }

        [Test]
        public void DisposeTest()
        {
            var mock1 = new Mock<IPlugin>();
            var mock1Disposable = mock1.As<IDisposable>();
            mock1Disposable.Setup(p => p.Dispose()).Verifiable();

            var mock2 = new Mock<IPlugin>();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.AddPlugins(new[] { mock1.Object, mock2.Object });
                pluginManager.Dispose();
            }

            mock1Disposable.Verify(p => p.Dispose());
        }
    }
}
