using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        protected ServiceProvider? ServiceProvider { get; set; }

        private ILoggerFactory? _loggerFactory;
        private ILogger? _logger;
        private PluginManager? _pluginManager;

        protected override void Configure()
        {
            var services = new ServiceCollection();

            _loggerFactory = ConfigureLogging(services);
            _logger = _loggerFactory.CreateLogger(GetType());

            _logger.LogInformation("Configuring Bootstrapper...");

            services.AddShellCore();
            services.AddShellFramework();

            LoadPlugins(services, _loggerFactory);

            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual ILoggerFactory ConfigureLogging(IServiceCollection services)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder
#if DEBUG
                .SetMinimumLevel(LogLevel.Debug)
                .AddDebug()
#else
                .SetMinimumLevel(LogLevel.Information)
#endif
            );

            services.AddSingleton(loggerFactory);
            services.AddLogging();

            return loggerFactory;
        }

        protected virtual void LoadPlugins(ServiceCollection services, ILoggerFactory loggerFactory)
        {
            string path = Path.GetDirectoryName(Uri.UnescapeDataString(
                new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath))!;

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Cannot scan empty directory path");

            var pluginFinder = new PluginFinder(loggerFactory.CreateLogger<PluginFinder>());
            var plugins = pluginFinder.DiscoverPlugins(path);

            _logger?.LogInformation($"Loading plugins...");

            var pluginContext = new PluginContext(services);
            _pluginManager = new PluginManager(loggerFactory.CreateLogger<PluginManager>());
            services.AddSingleton(_pluginManager);
            _pluginManager.LoadPlugins(plugins, pluginContext);

            _logger?.LogInformation("Plugins loaded.");
        }

        protected override T GetInstance<T>()
        {
            if (ServiceProvider is null)
            {
                throw new InvalidOperationException($"Service provider is uninitialized");
            }

            return ServiceProvider.GetRequiredService<T>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _logger?.LogInformation("Starting application...");

            IApplicationContext applicationContext = ServiceProvider?.GetRequiredService<IApplicationContext>()!;
            ServiceProvider?.GetRequiredService<PluginManager>().OnStartup(e, applicationContext);
            DisplayRootView<ShellViewModel>();

            _logger?.LogInformation("Application started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger?.LogInformation("Exiting application...");

            IApplicationContext applicationContext = ServiceProvider?.GetRequiredService<IApplicationContext>()!;
            ServiceProvider?.GetRequiredService<PluginManager>().OnExit(e, applicationContext);

            _logger?.LogInformation("Application exited.");
        }

        protected override void Dispose(bool disposing)
        {
            _logger?.LogInformation("Disposing resources...");
            _loggerFactory?.Dispose();
            _pluginManager?.Dispose();

            ServiceProvider?.Dispose();
            base.Dispose(disposing);
        }
    }
}
