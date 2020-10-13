using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Status bar item builder allowing to configure it
    /// </summary>
    public interface IStatusBarItemBuilder
    {
        /// <summary>
        /// Set section (items grouped together between separators) and relative order in that section
        /// </summary>
        /// <param name="minWidth">Minimal width of status bar item</param>
        /// <param name="isRightSide">True if item should be added to the right side of status bar</param>
        /// <param name="order">Relative order</param>
        /// <returns>Status bar item builder</returns>
        IStatusBarItemBuilder SetSizeAndPlacement(int minWidth, bool isRightSide, int order);

        /// <summary>
        /// Set text content of status bar item
        /// </summary>
        /// <param name="content">Text content</param>
        /// <returns>Status bar item builder</returns>
        IStatusBarItemBuilder SetContent(string content);

        /// <summary>
        /// Configures status bar item as command status bar item
        /// </summary>
        /// <param name="command">Command to associate to status bar item</param>
        /// <returns>Status bar item builder</returns>
        IStatusBarItemBuilder SetCommand(ICommand command);
    }
}
