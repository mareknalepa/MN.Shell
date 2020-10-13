namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Basic interface for document view model
    /// </summary>
    public interface IDocument : ILayoutModule
    {
        /// <summary>
        /// Description of document
        /// </summary>
        string Description { get; }
    }
}
