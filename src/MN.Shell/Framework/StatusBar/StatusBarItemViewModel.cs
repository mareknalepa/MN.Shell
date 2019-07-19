using Caliburn.Micro;
using MN.Shell.Core;
using System.Windows.Input;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarItemViewModel : PropertyChangedBase
    {
        public StatusBarSide Side { get; set; } = StatusBarSide.Left;

        public int Priority { get; set; } = 50;

        private string _content;

        public string Content
        {
            get => _content;
            set { _content = value; NotifyOfPropertyChange(); }
        }

        public ICommand Command { get; }

        public System.Action CommandAction { get; set; }

        private bool _canExecuteCommand;

        public bool CanExecuteCommand
        {
            get => _canExecuteCommand;
            set { _canExecuteCommand = value; NotifyOfPropertyChange(); }
        }

        public StatusBarItemViewModel()
        {
            Command = new RelayCommand(o => CommandAction(), o => CanExecuteCommand);
        }
    }
}
