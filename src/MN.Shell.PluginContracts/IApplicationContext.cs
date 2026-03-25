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

        event EventHandler<string>? ApplicationTitleChanged;

        /// <summary>
        /// Requests application to gracefully shutdown
        /// </summary>
        void RequestApplicationExit();

        event EventHandler? ApplicationExitRequested;

        /// <summary>
        /// Loads document of given type using its factory
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        void LoadDocumentUsingFactory<T>()
            where T : IDocument;

        event EventHandler? DocumentLoadRequested;

        Queue<IDocument> DocumentsToLoad { get; }
    }
}
