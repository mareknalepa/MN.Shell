using Caliburn.Micro;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core.Framework
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly ILog _log = new AppLogger(NLog.LogManager.GetLogger(typeof(AppBootstrapper).Name));

        private IKernel _kernel;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
                Assembly.GetExecutingAssembly(),
            };
        }

        protected override void Configure()
        {
            _log.Info("Configuring AppBootstrapper...");

            LogManager.GetLog = type => new AppLogger(NLog.LogManager.GetLogger(type.Name));

            _kernel = new StandardKernel();
            _kernel.Load(SelectAssemblies());

            _log.Info("AppBootstrapper configured.");
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
