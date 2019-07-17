using MN.Shell.Core;

namespace MN.Shell.Framework
{
    public abstract class ToolBase : LayoutModuleBase, ITool
    {
        private bool _isVisible = true;

        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; NotifyOfPropertyChange(); }
        }

        public ToolPosition InitialPosition { get; protected set; } = ToolPosition.Right;

        public double InitialMinWidth { get; protected set; } = 200;

        public double InitialMinHeight { get; protected set; } = 200;

        public double AutoHideMinWidth { get; protected set; } = 200;

        public double AutoHideMinHeight { get; protected set; } = 200;

        public ToolBase()
        {
            CloseCommand = new RelayCommand(o => IsVisible = false);
        }
    }
}
