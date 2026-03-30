using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Tests.Core
{
    public sealed class BootstrapperTests
    {
        [Fact]
        public void GetInstance_UsesInternalServiceProvider()

        {
            using var bootstrapper = new MockBootstrapper();
            bootstrapper.Configure();
            var instance = bootstrapper.GetInstance<IExampleService>();

            instance.ShouldNotBeNull();
            instance.GetType().ShouldBe(typeof(ExampleService));

            var anotherInstance = bootstrapper.GetInstance<IExampleService>();

            anotherInstance.ShouldNotBeNull();
            anotherInstance.ShouldBeSameAs(instance);
        }

        [Fact]
        public void Configure_LoadsPlugins()
        {
            using var bootstrapper = new MockBootstrapper();
            BootstrapperTestsExamplePlugin.PluginLoadCalled = false;
            bootstrapper.Configure();

            BootstrapperTestsExamplePlugin.PluginLoadCalled.ShouldBeTrue();
        }

        [Fact]
        public void OnStartup_CallsPluginOnStartup()
        {
            using var bootstrapper = new MockBootstrapper();
            BootstrapperTestsExamplePlugin.PluginOnStartupCalled = false;
            bootstrapper.Configure();

            // Hack to create instance of StartupEventArgs in tests:
            var constructorInfo = typeof(StartupEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke(null) is not StartupEventArgs startupEventArgs)
            {
                throw new InvalidOperationException($"Cannot create {nameof(StartupEventArgs)} instance");
            }

            try
            {
                bootstrapper.OnStartup(startupEventArgs);
            }
            catch (InvalidOperationException) { }

            BootstrapperTestsExamplePlugin.PluginOnStartupCalled.ShouldBeTrue();
        }

        [Fact]
        public void OnExit_CallsPluginOnExit()
        {
            using var bootstrapper = new MockBootstrapper();
            BootstrapperTestsExamplePlugin.PluginOnExitCalled = false;
            bootstrapper.Configure();

            // Hack to create instance of ExitEventArgs in tests:
            var constructorInfo = typeof(ExitEventArgs).GetTypeInfo().DeclaredConstructors.First();
            if (constructorInfo.Invoke([0]) is not ExitEventArgs exitEventArgs)
            {
                throw new InvalidOperationException($"Cannot create {nameof(ExitEventArgs)} instance");
            }

            bootstrapper.OnExit(exitEventArgs);

            BootstrapperTestsExamplePlugin.PluginOnExitCalled.ShouldBeTrue();
        }

        [Fact]
        public void Dispose_DisposesPlugins()
        {
            using var bootstrapper = new MockBootstrapper();
            BootstrapperTestsExamplePlugin.PluginDisposeCalled = false;
            bootstrapper.Configure();

            bootstrapper.Dispose();

            BootstrapperTestsExamplePlugin.PluginDisposeCalled.ShouldBeTrue();
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
            var windowManager = Substitute.For<IWindowManager>();

            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IWindowManager));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(windowManager);

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
        public static bool PluginLoadCalled { get; set; }
        public static bool PluginOnStartupCalled { get; set; }
        public static bool PluginOnExitCalled { get; set; }
        public static bool PluginDisposeCalled { get; set; }

        protected override void OnLoad() => PluginLoadCalled = true;

        public override void OnStartup(StartupEventArgs e, IApplicationContext applicationContext) => PluginOnStartupCalled = true;

        public override void OnExit(ExitEventArgs e, IApplicationContext applicationContext) => PluginOnExitCalled = true;

        public void Dispose() => PluginDisposeCalled = true;
    }

    internal interface IExampleService { }

    public class ExampleService : IExampleService { }
}
