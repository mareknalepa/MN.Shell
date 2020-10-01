namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Context injected externally by plugin loading infrastructure while loading the plugin,
    /// allowing access to various extension points by a plugin composition root
    /// </summary>
    public interface IPluginLoaderContext
    {
        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        void UseTool<T>() where T : ITool;
    }
}
