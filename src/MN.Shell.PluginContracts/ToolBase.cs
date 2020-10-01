using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    public abstract class ToolBase : Screen, ITool
    {
        public ICommand CloseCommand { get; }

        private bool _isVisible = true;

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }

        public virtual ToolPosition InitialPosition => ToolPosition.Right;

        public virtual double MinWidth => 200;

        public virtual double MinHeight => 200;

        public virtual double AutoHideMinWidth => 200;

        public virtual double AutoHideMinHeight => 200;

        public ToolBase()
        {
            CloseCommand = new Command(() => IsVisible = false);
        }
    }
}
