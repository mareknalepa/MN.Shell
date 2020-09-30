using System.Windows;

namespace MN.Shell.PluginContracts
{
    public abstract class PluginBase : IPlugin
    {
        /// <summary>
        /// Internal name of the plugin (composition root class full name)
        /// </summary>
        public virtual string Name => GetType().FullName;

        /// <summary>
        /// Plugin loader context, allowing access to various extension points by a plugin composition root
        /// </summary>
        public IPluginLoaderContext Context { get; private set; }

        /// <summary>
        /// Method called by application infrastructure while loading the plugin
        /// </summary>
        /// <param name="context">Plugin loader context, allowing access to various extension points
        /// by a plugin composition root</param>
        public void Load(IPluginLoaderContext context)
        {
            Context = context;
            OnLoad();
        }

        /// <summary>
        /// Composition root of the plugin, method called while loading a plugin after context has been already set up
        /// </summary>
        protected abstract void OnLoad();

        /// <summary>
        /// Method called by application's plugin infrastructure upon application's startup
        /// </summary>
        /// <param name="e">StartupEventArgs containing command-line arguments</param>
        public virtual void OnStartup(StartupEventArgs e) { }

        /// <summary>
        /// Method called by application's plugin infrastructure just before application's exit
        /// </summary>
        /// <param name="e">ExitEventArgs allowing to set exit code</param>
        public virtual void OnExit(ExitEventArgs e) { }
    }
}
