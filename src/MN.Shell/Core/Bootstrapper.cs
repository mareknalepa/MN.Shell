using Caliburn.Micro;
using Ninject;
using Ninject.Modules;
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
        private readonly ILog _log = new Logger(NLog.LogManager.GetLogger(typeof(Bootstrapper).Name));

        private IKernel _kernel;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override IEnumerable<Assembly> SelectAssemblies() => Enumerable.Empty<Assembly>();

        protected override void Configure()
        {
            _log.Info("Configuring AppBootstrapper...");

            LogManager.GetLog = type => new Logger(NLog.LogManager.GetLogger(type.Name));

            _kernel = new StandardKernel();

            string path = Path.GetDirectoryName(Uri.UnescapeDataString(
                new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath));

            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Cannot scan empty directory path");

            IEnumerable<Assembly> assemblies = Directory.GetFiles(path, "*.dll")
                .Union(Directory.GetFiles(path, "*.exe"))
                .Select(f =>
                {
                    try
                    {
                        return Assembly.LoadFrom(f);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                        return null;
                    }
                }).Where(assembly =>
                {
                    try
                    {
                        return assembly?.GetExportedTypes()
                            .Any(type => typeof(INinjectModule).IsAssignableFrom(type)) ?? false;
                    }
                    catch (Exception e)
                    {
                        _log.Error(e);
                        return false;
                    }
                });

            foreach (Assembly assembly in assemblies)
                _kernel.Load(assembly);

            AssemblySource.Instance.AddRange(_kernel.GetModules().Select(m => m.GetType().Assembly).Distinct());

            foreach (INinjectModule module in _kernel.GetModules())
                _log.Info($"Loaded module: {module.Name} [{module.GetType().Assembly.FullName}]");
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return _kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            return _kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _log.Info("Application startup");

            DisplayRootViewFor<IShell>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();

            _log.Info("Application exit");
        }
    }
}
