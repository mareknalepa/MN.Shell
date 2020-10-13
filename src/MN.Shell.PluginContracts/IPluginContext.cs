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
            where T : class, ITool;

        /// <summary>
        /// Registers given interface type as auto-implemented document factory
        /// </summary>
        /// <typeparam name="T">Interface of document factory</typeparam>
        void UseDocumentFactory<T, TDocument>()
            where T : class, IDocumentFactory<TDocument>
            where TDocument : IDocument;

        /// <summary>
        /// Registers menu provider
        /// </summary>
        /// <typeparam name="T">Type of menu provider</typeparam>
        void UseMenuProvider<T>()
            where T : class, IMenuProvider;

        /// <summary>
        /// Registers status bar provider
        /// </summary>
        /// <typeparam name="T">Type of status bar provider</typeparam>
        void UseStatusBarProvider<T>()
            where T : class, IStatusBarProvider;

        /// <summary>
        /// Registers service to be injected into any object requiring it
        /// </summary>
        /// <typeparam name="TInterface">Type of service base interface</typeparam>
        /// <typeparam name="TService">Type of service implementation</typeparam>
        void UseService<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface;
    }
}
