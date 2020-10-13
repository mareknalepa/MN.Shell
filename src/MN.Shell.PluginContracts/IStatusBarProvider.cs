namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Status bar provider interface which needs to be implemented in plugins in order to manipulate status bar
    /// </summary>
    public interface IStatusBarProvider
    {
        /// <summary>
        /// Called by status bar compiling infrastructure while building status bar
        /// </summary>
        /// <param name="builder">Status bar builder passed to allow status bar manipulation</param>
        void BuildStatusBar(IStatusBarBuilder builder);
    }
}
