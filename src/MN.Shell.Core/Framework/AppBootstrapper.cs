using Caliburn.Micro;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MN.Shell.Core.Framework
{
    public class AppBootstrapper : BootstrapperBase
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private CompositionContainer _container;

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

            var catalog = new AggregateCatalog(AssemblySource.Instance
                .Select(x => new AssemblyCatalog(x))
                .OfType<ComposablePartCatalog>());

            _container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new AppWindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);

            _logger.Info("AppBootstrapper configured.");
        }

        protected override object GetInstance(Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _logger.Info("Application startup");

            DisplayRootViewFor<IShell>();
        }
    }
}
