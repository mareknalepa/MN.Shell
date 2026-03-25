using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;
using System.Reflection;
using System.Windows;

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
                var instance = bootstrapper.GetInstance<IExampleService>();

                Assert.NotNull(instance);
                Assert.AreEqual(typeof(ExampleService), instance.GetType());

                var anotherInstance = bootstrapper.GetInstance<IExampleService>();

                Assert.NotNull(anotherInstance);
                Assert.AreSame(instance, anotherInstance);
            }
        }

        public static bool PluginLoadCalled { get; set; }

        [Test]
        public void PluginLoadTest()
        {
            using (var bootstrapper = new MockBootstrapper())
            {
                PluginLoadCalled = false;
                bootstrapper.Configure();

                Assert.True(PluginLoadCalled);
            }
        }

        public static bool PluginOnStartupCalled { get; set; }

        [Test]
        public void PluginOnStartupTest()
        {
            using (var bootstrapper = new MockBootstrapper())
            {
                PluginOnStartupCalled = false;
                bootstrapper.Configure();

                // Hack to create instance of StartupEventArgs in tests:
                var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
                if (constructorInfo.Invoke(null) is StartupEventArgs startupEventArgs)
                {
                    try
                    {
                        bootstrapper.OnStartup(startupEventArgs);
                    }
                    catch (InvalidOperationException) { }

                    Assert.True(PluginOnStartupCalled);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        public static bool PluginOnExitCalled { get; set; }

        [Test]
        public void PluginOnExitTest()
        {
            using (var bootstrapper = new MockBootstrapper())
            {
                PluginOnExitCalled = false;
                bootstrapper.Configure();

                // Hack to create instance of ExitEventArgs in tests:
                var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
                if (constructorInfo.Invoke(new object[] { 0 }) is ExitEventArgs exitEventArgs)
                {
                    bootstrapper.OnExit(exitEventArgs);

                    Assert.True(PluginOnExitCalled);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        public static bool PluginDisposeCalled { get; set; }

        [Test]
        public void PluginDisposeCalledTest()
        {
            using (var bootstrapper = new MockBootstrapper())
            {
                PluginDisposeCalled = false;
                bootstrapper.Configure();

                bootstrapper.Dispose();

                Assert.True(PluginDisposeCalled);
            }
        }
    }

    public class MockBootstrapper : Bootstrapper
    {
        public new void Configure()
        {
            var services = new ServiceCollection();

            var loggerFactory = ConfigureLogging(services);
            services.AddShellCore();
            services.AddShellFramework();

            // Hack to suppress creating real WindowManager
            var windowManagerMock = new Mock<IWindowManager>();

            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IWindowManager));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton<IWindowManager>(windowManagerMock.Object);

            // Hack to suppress creating real ShellViewModel
            descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ShellViewModel));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IExampleService, ExampleService>();

            LoadPlugins(services, loggerFactory);

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override ILoggerFactory ConfigureLogging(IServiceCollection services)
        {
            services.AddTransient(typeof(ILogger<>), typeof(NullLogger<>));
            return NullLoggerFactory.Instance;
        }

        public new T GetInstance<T>()
            where T : notnull
            => base.GetInstance<T>();

        public new void OnStartup(StartupEventArgs e) => base.OnStartup(e);

        public new void OnExit(ExitEventArgs e) => base.OnExit(e);
    }

    public sealed class BootstrapperTestsExamplePlugin : PluginBase, IDisposable
    {
        protected override void OnLoad() => BootstrapperTests.PluginLoadCalled = true;

        public override void OnStartup(StartupEventArgs e, IApplicationContext applicationContext) => BootstrapperTests.PluginOnStartupCalled = true;

        public override void OnExit(ExitEventArgs e, IApplicationContext applicationContext) => BootstrapperTests.PluginOnExitCalled = true;

        public void Dispose() => BootstrapperTests.PluginDisposeCalled = true;
    }

    internal interface IExampleService { }

    public class ExampleService : IExampleService { }
}
