using MN.Shell.MVVM;

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

        public virtual ToolPosition InitialPosition => ToolPosition.Right;

        public virtual double MinWidth => 200;

        public virtual double MinHeight => 200;

        public virtual double AutoHideMinWidth => 200;

        public virtual double AutoHideMinHeight => 200;

        public ToolBase()
        {
            CloseCommand = new RelayCommand(() => IsVisible = false);
        }
    }
}
