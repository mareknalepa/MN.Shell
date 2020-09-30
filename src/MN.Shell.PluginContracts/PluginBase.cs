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
    }
}
