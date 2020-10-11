namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Status bar builder injected into status bar providers to allow status bar manipulating
    /// </summary>
    public interface IStatusBarBuilder
    {
        /// <summary>
        /// Adds new status bar item specified by name
        /// </summary>
        /// <param name="name">Name of the status bar item to add</param>
        /// <returns>Status bar item builder</returns>
        IStatusBarItemBuilder AddItem(string name);

        /// <summary>
        /// Removes status bar item specified by name
        /// </summary>
        /// <param name="name">Name of the status bar item to remove</param>
        void RemoveItem(string name);
    }
}
