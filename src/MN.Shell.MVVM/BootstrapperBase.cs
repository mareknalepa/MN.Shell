using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for Bootstrappers. Inheriting classes should specify and configure IoC container to be used.
    /// </summary>
    public abstract class BootstrapperBase : IBootstrapper, IDisposable
    {
        private Application _application;

        /// <summary>
        /// Called at application startup to allow Bootstrapper to attach itself to currently running application
        /// in order to start MVVM framework
        /// </summary>
        /// <param name="application">Currently running application to attach to</param>
        public void Setup(Application application)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));

            application.Startup += (sender, e) =>
            {
                Configure();
                ConfigureInternals();
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
        /// Generic method called internally by MVVM framework to get instance of type T from IoC container
        /// </summary>
        /// <typeparam name="T">Type of instance to create</typeparam>
        /// <returns>Instance created by IoC container</returns>
        protected abstract T GetInstance<T>();

        /// <summary>
        /// Alternate method called internally by MVVM framework to get instance of given type from IoC container
        /// </summary>
        /// <param name="type">Type of instance to create</param>
        /// <returns>Instance created by IoC container</returns>
        protected abstract object GetInstance(Type type);

        /// <summary>
        /// Private method to configure internal components
        /// </summary>
        private void ConfigureInternals()
        {
            var viewManager = GetInstance<IViewManager>();
            if (viewManager is null)
                throw new InvalidOperationException("Cannot create instance of IViewManager");

            viewManager.ViewFactory = GetInstance;

            Binder.ViewManager = viewManager;

            var windowManager = GetInstance<IWindowManager>();
            if (windowManager is null)
                throw new InvalidOperationException("Cannot create instance of IWindowManager");

            windowManager.GetActiveWindow =
                () => _application.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) ?? _application.MainWindow;
        }

        /// <summary>
        /// Displays view for application's root view model. Should be called during application startup.
        /// </summary>
        /// <typeparam name="T">Type of root view model to instantiate</typeparam>
        protected void DisplayRootView<T>()
            where T : class
        {
            var windowManager = GetInstance<IWindowManager>();
            if (windowManager is null)
                throw new InvalidOperationException("Cannot create instance of IWindowManager");

            var rootViewModel = GetInstance<T>();
            if (rootViewModel is null)
                throw new InvalidOperationException($"Cannot create instance of root view model ({typeof(T)})");

            windowManager.ShowWindow(rootViewModel);
        }

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
        /// Should be overriden by inheriting classes to dispose application-wide resources and IoC container
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
