using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;
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

            using (var pluginManager = new PluginManager(NullLogger<PluginManager>.Instance))
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
            var applicationContext = new Mock<IApplicationContext>().Object;

            // Hack to create instance of StartupEventArgs in tests:
            var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke(null) is StartupEventArgs startupEventArgs)
            {
                var mock1 = new Mock<IPlugin>();
                mock1.Setup(p => p.OnStartup(startupEventArgs, applicationContext)).Verifiable();

                var mock2 = new Mock<IPlugin>();
                mock2.Setup(p => p.OnStartup(startupEventArgs, applicationContext)).Verifiable();

                using (var pluginManager = new PluginManager(NullLogger<PluginManager>.Instance))
                {
                    pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                    pluginManager.OnStartup(startupEventArgs, applicationContext);
                }

                mock1.VerifyAll();
                mock2.VerifyAll();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void OnExitTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;
            var applicationContext = new Mock<IApplicationContext>().Object;

            // Hack to create instance of ExitEventArgs in tests:
            var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke(new object[] { 0 }) is ExitEventArgs exitEventArgs)
            {
                var mock1 = new Mock<IPlugin>();
                mock1.Setup(p => p.OnExit(exitEventArgs, applicationContext)).Verifiable();

                var mock2 = new Mock<IPlugin>();
                mock2.Setup(p => p.OnExit(exitEventArgs, applicationContext)).Verifiable();

                using (var pluginManager = new PluginManager(NullLogger<PluginManager>.Instance))
                {
                    pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                    pluginManager.OnExit(exitEventArgs, applicationContext);
                }

                mock1.VerifyAll();
                mock2.VerifyAll();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void DisposeTest()
        {
            var context = new Mock<IScopedPluginContext>().Object;

            var mock1 = new Mock<IPlugin>();
            var mock1Disposable = mock1.As<IDisposable>();
            mock1Disposable.Setup(p => p.Dispose()).Verifiable();

            var mock2 = new Mock<IPlugin>();

            using (var pluginManager = new PluginManager(NullLogger<PluginManager>.Instance))
            {
                pluginManager.LoadPlugins(new[] { mock1.Object, mock2.Object }, context);
                pluginManager.Dispose();
            }

            mock1Disposable.VerifyAll();
        }
    }
}
