namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Menu provider interface which needs to be implemented in plugins in order to manipulate menu
    /// </summary>
    public interface IMenuProvider
    {
        /// <summary>
        /// Called by menu compiling infrastructure while building menu
        /// </summary>
        /// <param name="builder">Menu builder passed to allow menu manipulation</param>
        void BuildMenu(IMenuBuilder builder);
    }
}
