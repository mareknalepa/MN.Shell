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

        protected override void OnStartup(StartupEventArgs e)
        {
            var windowManager = _serviceProvider.GetRequiredService<IWindowManager>();
            var shellViewModel = _serviceProvider.GetRequiredService<ShellViewModel>();
            windowManager.ShowWindow(shellViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            (_serviceProvider as IDisposable).Dispose();
        }
    }
}
