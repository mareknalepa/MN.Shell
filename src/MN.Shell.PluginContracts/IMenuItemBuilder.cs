using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Menu item builder allowing to configure it
    /// </summary>
    public interface IMenuItemBuilder
    {
        /// <summary>
        /// Set section (items grouped together between separators) and relative order in that section
        /// </summary>
        /// <param name="section">Section number</param>
        /// <param name="order">Relative order in section</param>
        /// <returns>Menu item builder</returns>
        IMenuItemBuilder SetPlacement(int section, int order);

        /// <summary>
        /// Configures menu item as command menu item
        /// </summary>
        /// <param name="command">Command to associated to menu item</param>
        /// <returns>Menu item builder</returns>
        IMenuItemBuilder SetCommand(ICommand command);

        /// <summary>
        /// Configures menu item as a checkbox menu item
        /// </summary>
        /// <param name="isCheckedByDefault">Default value of checkbox</param>
        /// <param name="isThreeState">True if null is accepted</param>
        /// <returns>Menu item builder</returns>
        IMenuItemBuilder SetCheckbox(bool? isCheckedByDefault, bool isThreeState = false);
    }
}
