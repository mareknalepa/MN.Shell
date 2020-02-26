using System;
using System.Windows.Input;

namespace MN.Shell.Core
{
    public class RelayCommand : ICommand
    {
        protected readonly Action<object> _execute;
        protected readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                _execute.Invoke(parameter);
        }
    }
}
