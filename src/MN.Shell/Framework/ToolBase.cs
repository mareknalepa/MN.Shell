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

        public ToolPosition InitialPosition { get; protected set; }

        public ToolBase()
        {
            CloseCommand = new RelayCommand(o => IsVisible = false);
        }
    }
}
