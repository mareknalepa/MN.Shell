using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using Moq;
using Ninject;
using NUnit.Framework;
using System;
using System.Linq;
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
                var e = constructorInfo.Invoke(null) as StartupEventArgs;

                try
                {
                    bootstrapper.OnStartup(e);
                }
                catch (ActivationException) { }

                Assert.True(PluginOnStartupCalled);
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
                var e = constructorInfo.Invoke(new object[] { 0 }) as ExitEventArgs;

                bootstrapper.OnExit(e);

                Assert.True(PluginOnExitCalled);
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
            base.Configure();

            // Hack to suppress creating real WindowManager
            var windowManagerMock = new Mock<IWindowManager>();
            Kernel.Rebind<IWindowManager>().ToConstant(windowManagerMock.Object);

            // Hack to suppress creating real ShellViewModel
            Kernel.Rebind<ShellViewModel>().ToConstant(null as ShellViewModel);

            Kernel.Bind<IExampleService, ExampleService>().To<ExampleService>().InSingletonScope();
        }

        protected override ILogger ConfigureLogging()
        {
            Kernel.Bind<ILogger>().ToConstant(NullLogger.Instance).InSingletonScope();
            return NullLogger.Instance;
        }

        public new T GetInstance<T>() => base.GetInstance<T>();

        public new void OnStartup(StartupEventArgs e) => base.OnStartup(e);

        public new void OnExit(ExitEventArgs e) => base.OnExit(e);
    }

    public sealed class BootstrapperTestsExamplePlugin : PluginBase, IDisposable
    {
        protected override void OnLoad() => BootstrapperTests.PluginLoadCalled = true;

        public override void OnStartup(StartupEventArgs e) => BootstrapperTests.PluginOnStartupCalled = true;

        public override void OnExit(ExitEventArgs e) => BootstrapperTests.PluginOnExitCalled = true;

        public void Dispose() => BootstrapperTests.PluginDisposeCalled = true;
    }

    internal interface IExampleService { }

    public class ExampleService : IExampleService { }
}
