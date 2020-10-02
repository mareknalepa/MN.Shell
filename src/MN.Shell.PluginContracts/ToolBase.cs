using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Base class for document view models
    /// </summary>
    public abstract class ToolBase : Screen, ITool
    {
        /// <summary>
        /// Command used to close given layout module
        /// </summary>
        public ICommand CloseCommand { get; }

        private bool _isVisible = true;

        /// <summary>
        /// Determines if a tool is visible in docking layout
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        /// <summary>
        /// Specifies initial position of a tool in docking layout
        /// </summary>
        public virtual ToolPosition InitialPosition => ToolPosition.Right;

        /// <summary>
        /// Minimal width
        /// </summary>
        public virtual double MinWidth => 200;

        /// <summary>
        /// Minimal height
        /// </summary>
        public virtual double MinHeight => 200;

        /// <summary>
        /// Minimal width while automatically showing collapsed tool
        /// </summary>
        public virtual double AutoHideMinWidth => 200;

        /// <summary>
        /// Minimal height while automatically showing collapsed tool
        /// </summary>
        public virtual double AutoHideMinHeight => 200;

        /// <summary>
        /// Creates new tool view model
        /// </summary>
        public ToolBase()
        {
            CloseCommand = new Command(() => IsVisible = false);
        }
    }
}
