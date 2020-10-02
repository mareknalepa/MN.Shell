namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Context injected externally by plugin loading infrastructure while loading the plugin,
    /// allowing access to various extension points and application-wide features by a plugin composition root
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Application context allowing access to application wide-features
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        void UseTool<T>()
            where T : ITool;

        /// <summary>
        /// Registers given interface type as auto-implemented document factory
        /// </summary>
        /// <typeparam name="T">Interface of document factory</typeparam>
        void UseDocumentFactory<T, TDocument>()
            where T : class, IDocumentFactory<TDocument>
            where TDocument : IDocument;
    }
}
