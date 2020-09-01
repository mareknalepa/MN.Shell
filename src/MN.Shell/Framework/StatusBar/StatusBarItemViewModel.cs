using System.Windows.Input;
using MN.Shell.MVVM;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarItemViewModel : PropertyChangedBase
    {
        public StatusBarSide Side { get; set; } = StatusBarSide.Left;

        public int Priority { get; set; } = 50;

        public double MinWidth { get; set; } = 100;

        private string _content;

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        public ICommand Command { get; }

        public System.Action CommandAction { get; set; }

        private bool _canExecuteCommand = true;

        public bool CanExecuteCommand
        {
            get => _canExecuteCommand;
            set => Set(ref _canExecuteCommand, value);
        }

        public StatusBarItemViewModel()
        {
            Command = new RelayCommand(() => CommandAction?.Invoke(), () => CanExecuteCommand && CommandAction != null);
        }
    }
}
