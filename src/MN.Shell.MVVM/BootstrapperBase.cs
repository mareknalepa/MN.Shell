using System;
using System.Windows;
using System.Windows.Threading;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for Bootstrappers. Inheriting classes should specify and configure IoC container to be used.
    /// </summary>
    public abstract class BootstrapperBase : IBootstrapper, IDisposable
    {
        /// <summary>
        /// Called at application startup to allow Bootstrapper to attach itself to currently running application
        /// in order to start MVVM framework
        /// </summary>
        /// <param name="application">Currently running application to attach to</param>
        public void Setup(Application application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            application.Startup += (sender, e) =>
            {
                Configure();
                OnStartup(e);
            };

            application.Exit += (sender, e) =>
            {
                OnExit(e);
                Dispose();
            };

            application.DispatcherUnhandledException += (sender, e) =>
            {
                OnUnhandledException(e);
            };
        }

        /// <summary>
        /// Method called on startup to configure IoC container and register basic services in it
        /// </summary>
        protected abstract void Configure();

        /// <summary>
        /// Method called on startup to run application. It should display view for application's main view model
        /// </summary>
        /// <param name="e">StartupEventArgs including parameters passed on command line</param>
        protected virtual void OnStartup(StartupEventArgs e) { }

        /// <summary>
        /// Method called just before application shutdown. Can be used to perform final cleanup and set exit code.
        /// </summary>
        /// <param name="e">ExitEventArgs allowing to get/set application exit code</param>
        protected virtual void OnExit(ExitEventArgs e) { }

        /// <summary>
        /// Handler for application-wide unhandled exceptions
        /// </summary>
        /// <param name="e">EventArgs containing exception</param>
        protected virtual void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e) { }

        /// <summary>
        /// Called to dispose application-wide resources, should be overriden by inheriting classes.
        /// </summary>
        protected virtual void Dispose(bool disposing) { }

        /// <summary>
        /// Called to dispose application-wide resources (e.g. IoC container)
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
