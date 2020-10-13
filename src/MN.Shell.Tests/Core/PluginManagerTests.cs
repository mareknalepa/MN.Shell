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
        public void LoadPluginsTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;

            var mock1 = new Mock<IPlugin>();
            mock1.Setup(p => p.Load(context)).Verifiable();

            var mock2 = new Mock<IPlugin>();
            mock2.Setup(p => p.Load(context)).Verifiable();

            using (var pluginManager = new PluginManager())
            {
                Assert.NotNull(pluginManager.Plugins);
                Assert.IsEmpty(pluginManager.Plugins);

                pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);

                Assert.That(pluginManager.Plugins, Has.Exactly(2).Items);
                Assert.True(pluginManager.Plugins.Contains(mock1.Object));
                Assert.True(pluginManager.Plugins.Contains(mock2.Object));

                mock1.VerifyAll();
                mock2.VerifyAll();
            }
        }

        [Test]
        public void OnStartupTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;

            // Hack to create instance of StartupEventArgs in tests:
            var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
            var e = constructorInfo.Invoke(null) as StartupEventArgs;

            var mock1 = new Mock<IPlugin>();
            mock1.Setup(p => p.OnStartup(e)).Verifiable();

            var mock2 = new Mock<IPlugin>();
            mock2.Setup(p => p.OnStartup(e)).Verifiable();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                pluginManager.OnStartup(e);
            }

            mock1.VerifyAll();
            mock2.VerifyAll();
        }

        [Test]
        public void OnExitTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;

            // Hack to create instance of ExitEventArgs in tests:
            var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
            var e = constructorInfo.Invoke(new object[] { 0 }) as ExitEventArgs;

            var mock1 = new Mock<IPlugin>();
            mock1.Setup(p => p.OnExit(e)).Verifiable();

            var mock2 = new Mock<IPlugin>();
            mock2.Setup(p => p.OnExit(e)).Verifiable();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                pluginManager.OnExit(e);
            }

            mock1.VerifyAll();
            mock2.VerifyAll();
        }

        [Test]
        public void DisposeTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;

            var mock1 = new Mock<IPlugin>();
            var mock1Disposable = mock1.As<IDisposable>();
            mock1Disposable.Setup(p => p.Dispose()).Verifiable();

            var mock2 = new Mock<IPlugin>();

            using (var pluginManager = new PluginManager())
            {
                pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                pluginManager.Dispose();
            }

            mock1Disposable.VerifyAll();
        }
    }
}
