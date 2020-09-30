using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using Ninject;
using Ninject.Modules;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private IKernel _kernel;

        protected override void Configure()
        {
            _logger.Info("Configuring Bootstrapper...");

            _kernel = new StandardKernel();

            string path = Path.GetDirectoryName(Uri.UnescapeDataString(
                new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath));

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Cannot scan empty directory path");

            var pluginLoaderContext = new PluginLoaderContext(_kernel);
            var plugins = PluginLoader.DiscoverAndLoadPlugins(path, pluginLoaderContext);

            _logger.Info($"Directory to scan for modules: {path}");

            IEnumerable<Assembly> assemblies = Directory.GetFiles(path, "*.dll")
                .Union(Directory.GetFiles(path, "*.exe"))
                .Select(f =>
                {
                    try
                    {
                        return Assembly.LoadFrom(f);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        _logger.Error(e);
                        return null;
                    }
                }).Where(assembly =>
                {
                    try
                    {
                        return assembly?.GetExportedTypes()
                            .Any(type => typeof(INinjectModule).IsAssignableFrom(type)) ?? false;
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        _logger.Error(e);
                        return false;
                    }
                });

            foreach (Assembly assembly in assemblies)
                _kernel.Load(assembly);

            foreach (INinjectModule module in _kernel.GetModules())
                _logger.Info($"Loaded module: {module.Name} [{module.GetType().Assembly.FullName}]");
        }

        protected override T GetInstance<T>() => _kernel.Get<T>();

        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.Info("Application startup");
            DisplayRootView<ShellViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger.Info("Application exit");
        }

        protected override void Dispose(bool disposing)
        {
            _kernel.Dispose();
            base.Dispose(disposing);
        }
    }
}
