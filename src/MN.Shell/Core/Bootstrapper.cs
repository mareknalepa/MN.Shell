using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MN.Shell.Framework;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using Ninject;
using Ninject.Modules;
using NLog.Extensions.Logging;
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

            ConfigureLogging();

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

        private void ConfigureLogging()
        {
            var entryPointAssembly = Assembly.GetEntryAssembly();

            if (entryPointAssembly == null)
            {
                // This can occur in unit tests
                Kernel.Bind<ILoggerFactory>().To<NullLoggerFactory>().InSingletonScope();
                Kernel.Bind<ILogger>().ToConstant(NullLogger.Instance);
                _logger = NullLogger.Instance;
                return;
            }

            var versionAttribute = entryPointAssembly.GetCustomAttribute<AssemblyVersionAttribute>();
            var infoVersionAttribute = entryPointAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            string version = versionAttribute?.Version ?? infoVersionAttribute?.InformationalVersion ?? "Unknown";

            var appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                entryPointAssembly.GetName().Name,
                version);

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

#pragma warning disable CA2000 // Dispose objects before losing scope (will be disposed by kernel)

            var nlogConfig = new NLog.Config.LoggingConfiguration();

#if DEBUG
            var debugConsoleTarget = new NLog.Targets.DebuggerTarget("debuggerTarget");
            nlogConfig.AddTarget(debugConsoleTarget);
            nlogConfig.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, debugConsoleTarget);
#endif

            var logFileTarget = new NLog.Targets.FileTarget("logfileTarget")
            {
                FileName = Path.Combine(appFolder, "log.txt")
            };
            nlogConfig.AddTarget(logFileTarget);
            nlogConfig.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logFileTarget);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
#if DEBUG
                builder.SetMinimumLevel(LogLevel.Trace);
#else
                builder.SetMinimumLevel(LogLevel.Information);
#endif
                builder
                    .AddDebug()
                    .AddNLog(nlogConfig);
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

            NLog.LogManager.Shutdown();
            Kernel.Dispose();
            base.Dispose(disposing);
        }
    }
}
