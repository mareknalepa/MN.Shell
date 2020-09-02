using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MN.Shell.MVVM.Sample
{
    public class SampleBootstrapper : BootstrapperBase
    {
        private IServiceProvider _serviceProvider;

        protected override void Configure()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IViewManager, ViewManager>();
            serviceCollection.AddSingleton<IWindowManager, WindowManager>();
            serviceCollection.AddSingleton<IMessageBus, MessageBus>();

            serviceCollection.AddTransient<ShellViewModel>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override T GetInstance<T>() => _serviceProvider.GetService<T>();

        protected override object GetInstance(Type type) => _serviceProvider.GetService(type);

        protected override void OnStartup(StartupEventArgs e) => DisplayRootView<ShellViewModel>();

        protected override void Dispose(bool disposing)
        {
            (_serviceProvider as IDisposable).Dispose();
            base.Dispose(disposing);
        }
    }
}
