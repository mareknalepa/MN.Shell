namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Basic interface which should be implemented by plugin composition root
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Internal name of the plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Method called by application's plugin loading infrastructure while loading the plugin
        /// </summary>
        /// <param name="context">Plugin loader context, allowing access to various extension points
        /// by a plugin composition root</param>
        void Load(IPluginLoaderContext context);
    }
}
