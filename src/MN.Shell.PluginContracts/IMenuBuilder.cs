namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Menu builder injected into menu providers to allow menu manipulating
    /// </summary>
    public interface IMenuBuilder
    {
        /// <summary>
        /// Adds new menu item specified by path in format of: "Menu/Submenu1/Submenu2/Item"
        /// </summary>
        /// <param name="path">Path to given menu item in string (separated by slashes)</param>
        /// <param name="localizedName">Localized name of menu item</param>
        /// <returns>Menu item builder</returns>
        IMenuItemBuilder AddItem(string path, string localizedName);

        /// <summary>
        /// Removes menu item specified by path in format of: "Menu/Submenu1/Submenu2/Item"
        /// </summary>
        /// <param name="path">Path to given menu item in string (separated by slashes)</param>
        /// <param name="forceRemoveIfNonEmpty">True if item should be removed even if it has sub items</param>
        void RemoveItem(string path, bool forceRemoveIfNonEmpty = false);
    }
}
