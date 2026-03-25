using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MN.Shell.MVVM.Sample
{
    public class SampleBootstrapper : BootstrapperBase
    {
        private ServiceProvider? _serviceProvider;

        protected override void Configure()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IViewManager, ViewManager>();
            serviceCollection.AddSingleton<IWindowManager, WindowManager>();
            serviceCollection.AddSingleton<IMessageBus, MessageBus>();

            serviceCollection.AddTransient<ShellViewModel>();
            serviceCollection.AddTransient<ShellView>();
            serviceCollection.AddTransient<CommandsSampleView>();
            serviceCollection.AddTransient<SampleDocumentView>();
            serviceCollection.AddTransient<SampleToolView>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var viewManager = _serviceProvider.GetRequiredService<IViewManager>();
            viewManager.ViewFactory = type => _serviceProvider.GetService(type);
        }

        protected override T GetInstance<T>()
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException($"Service provider is uninitialized");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        protected override void OnStartup(StartupEventArgs e) => DisplayRootView<ShellViewModel>();

        protected override void Dispose(bool disposing)
        {
            _serviceProvider?.Dispose();
            base.Dispose(disposing);
        }
    }
}
