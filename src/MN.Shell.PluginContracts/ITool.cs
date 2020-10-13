namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Basic interface for tool view model
    /// </summary>
    public interface ITool : ILayoutModule
    {
        /// <summary>
        /// Determines if a tool is visible in docking layout
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Specifies initial position of a tool in docking layout
        /// </summary>
        ToolPosition InitialPosition { get; }

        /// <summary>
        /// Minimal width
        /// </summary>
        double MinWidth { get; }

        /// <summary>
        /// Minimal height
        /// </summary>
        double MinHeight { get; }

        /// <summary>
        /// Minimal width while automatically showing collapsed tool
        /// </summary>
        double AutoHideMinWidth { get; }

        /// <summary>
        /// Minimal height while automatically showing collapsed tool
        /// </summary>
        double AutoHideMinHeight { get; }
    }
}
