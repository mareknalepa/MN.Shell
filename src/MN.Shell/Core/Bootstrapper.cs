using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using Ninject;
using Ninject.Modules;
using NLog;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public IKernel Kernel { get; private set; }

        protected override void Configure()
        {
            _logger.Info("Configuring Bootstrapper...");

            Kernel = new StandardKernel();
            Kernel.Load(new CoreModule());
            Kernel.Load(new FrameworkModule());

            foreach (INinjectModule module in Kernel.GetModules())
                _logger.Info($"Loaded kernel module: {module.Name} [{module.GetType().Assembly.FullName}]");

            string path = Path.GetDirectoryName(Uri.UnescapeDataString(
                new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath));

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Cannot scan empty directory path");

            _logger.Info($"Directory to scan for plugins: {path}");

            var pluginLoaderContext = new PluginContext(Kernel);
            var pluginManager = Kernel.Get<PluginManager>();

            pluginManager.AddPlugins(PluginLoader.DiscoverAndLoadPlugins(path, pluginLoaderContext));

            foreach (IPlugin plugin in pluginManager.Plugins)
                _logger.Info($"Loaded plugin: {plugin.Name} [{plugin.GetType().Assembly.FullName}]");
        }

        protected override T GetInstance<T>() => Kernel.Get<T>();

        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.Info("Starting application...");

            Kernel.Get<PluginManager>().OnStartup(e);
            DisplayRootView<ShellViewModel>();

            _logger.Info("Application started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger.Info("Exiting application...");

            Kernel.Get<PluginManager>().OnExit(e);

            _logger.Info("Application exited.");
        }

        protected override void Dispose(bool disposing)
        {
            _logger.Info("Disposing resources...");

            Kernel.Dispose();
            base.Dispose(disposing);

            _logger.Info("Resources disposed.");
        }
    }
}
