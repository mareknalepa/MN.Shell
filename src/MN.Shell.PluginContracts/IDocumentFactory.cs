namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Basic interface of document factory
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public interface IDocumentFactory<T>
        where T : IDocument
    {
        /// <summary>
        /// Creates document view model using dependency injection container
        /// </summary>
        /// <returns>Created document view model</returns>
        T Create();
    }
}
