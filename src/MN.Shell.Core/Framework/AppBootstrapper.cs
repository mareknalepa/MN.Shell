using Caliburn.Micro;
using MN.Shell.Core.Modules.MainWindow;
using Ninject;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MN.Shell.Core.Framework
{
    public class AppBootstrapper : BootstrapperBase
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

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
            _logger.Info("Configuring AppBootstrapper...");

            Caliburn.Micro.LogManager.GetLog = type => new CaliburnMicroLogger(type);

            _kernel = new StandardKernel();
            _kernel.Bind<IWindowManager>().To<AppWindowManager>().InSingletonScope();
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            _kernel.Bind<IShell>().To<MainWindowViewModel>().InSingletonScope();

            _logger.Info("AppBootstrapper configured.");
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
            _logger.Info("Application startup");

            DisplayRootViewFor<IShell>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _kernel.Dispose();

            _logger.Info("Application exit");
        }
    }
}
