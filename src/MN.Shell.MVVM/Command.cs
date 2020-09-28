using System;
using System.Windows.Input;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Basic implementation of ICommand to be used in bindings
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Event raised when CanExecute state should be refreshed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Creates new Command with given delegates accepting parameter
        /// </summary>
        /// <param name="execute">Delegate executed with command</param>
        /// <param name="canExecute">Delegate used to determine if command can be executed (optional)</param>
        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates new Command using simpler delegates without additional parameter
        /// </summary>
        /// <param name="execute">Delegate executed with command</param>
        /// <param name="canExecute">Delegate used to determine if command can be executed (optional)</param>
        public Command(Action execute, Func<bool> canExecute = null)
        {
            _execute = o => execute.Invoke();
            if (canExecute != null)
                _canExecute = o => canExecute.Invoke();
        }

        /// <summary>
        /// Method used to determine if command can be executed
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        /// <returns>True if command can be executed, false otherwise</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Method executed with command
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                _execute.Invoke(parameter);
        }
    }
}
