namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Interface of application context which allows to access application-wide functionalities by plugins
    /// </summary>
    public interface IApplicationContext
    {
        /// <summary>
        /// Application title shown on title bar
        /// </summary>
        string ApplicationTitle { get; set; }

        /// <summary>
        /// Requests application to gracefully shutdown
        /// </summary>
        void RequestApplicationExit();

        /// <summary>
        /// Loads document of given type using its factory
        /// </summary>
        /// <typeparam name="T">Document factory type</typeparam>
        /// <typeparam name="TDocument">Document type</typeparam>
        void LoadDocumentUsingFactory<T, TDocument>()
            where T : IDocumentFactory<TDocument>
            where TDocument : IDocument;
    }
}
