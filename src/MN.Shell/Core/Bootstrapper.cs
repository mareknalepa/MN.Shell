using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using Ninject;
using Ninject.Modules;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        public IKernel Kernel { get; private set; }

        private ILogger _logger;

        protected override void Configure()
        {
            Kernel = new StandardKernel();

#pragma warning disable CA2000 // Dispose objects before losing scope (loggerFactory will be disposed by kernel)
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });
#pragma warning restore CA2000 // Dispose objects before losing scope

            Kernel.Bind<ILoggerFactory>().ToConstant(loggerFactory).InSingletonScope();
            Kernel.Bind<ILogger>().ToMethod(context =>
            {
                var factory = context.Kernel.Get<ILoggerFactory>();
                var categoryName = context.Request?.ParentRequest?.Service.FullName ?? "Uncategorized";
                return factory.CreateLogger(categoryName);
            });

            _logger = loggerFactory.CreateLogger(GetType().FullName);

            _logger.LogInformation("Configuring Bootstrapper...");

            Kernel.Load(new CoreModule());
            Kernel.Load(new FrameworkModule());

            foreach (INinjectModule module in Kernel.GetModules())
                _logger.LogInformation($"Loaded kernel module: {module.Name} [{module.GetType().Assembly.FullName}]");

            string path = Path.GetDirectoryName(Uri.UnescapeDataString(
                new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath));

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Cannot scan empty directory path");

            var pluginFinder = Kernel.Get<PluginFinder>();
            var plugins = pluginFinder.DiscoverPlugins(path);

            _logger.LogInformation($"Loading plugins...");

            var pluginContext = new PluginContext(Kernel);
            var pluginManager = Kernel.Get<PluginManager>();
            pluginManager.LoadPlugins(plugins, pluginContext);

            _logger.LogInformation("Plugins loaded.");
        }

        protected override T GetInstance<T>() => Kernel.Get<T>();

        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.LogInformation("Starting application...");

            Kernel.Get<PluginManager>().OnStartup(e);
            DisplayRootView<ShellViewModel>();

            _logger.LogInformation("Application started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger.LogInformation("Exiting application...");

            Kernel.Get<PluginManager>().OnExit(e);

            _logger.LogInformation("Application exited.");
        }

        protected override void Dispose(bool disposing)
        {
            _logger.LogInformation("Disposing resources...");

            Kernel.Dispose();
            base.Dispose(disposing);
        }
    }
}
