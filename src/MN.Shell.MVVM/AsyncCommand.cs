using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Asynchronous command which allows observing its progress while executing
    /// </summary>
    public class AsyncCommand : PropertyChangedBase, IAsyncCommand
    {
        private readonly Func<object, Task> _executeAsync;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Event raised when CanExecute state should be refreshed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private TaskNotifier _execution;

        /// <summary>
        /// TaskNotifier exposed to allow observing command execution progress via data binding
        /// </summary>
        public TaskNotifier Execution
        {
            get => _execution;
            private set
            {
                _execution = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IsExecuting));
            }
        }

        public bool IsExecuting => Execution?.IsNotCompleted ?? false;

        /// <summary>
        /// Creates new AsyncCommand with given delegates accepting parameter
        /// </summary>
        /// <param name="executeAsync">Asynchronous delegate executed with command</param>
        /// <param name="canExecute">Delegate used to determine if command can be executed (optional)</param>
        public AsyncCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates new AsyncCommand using simpler delegates without additional parameter
        /// </summary>
        /// <param name="executeAsync">Asynchronous delegate executed with command</param>
        /// <param name="canExecute">Delegate used to determine if command can be executed (optional)</param>
        public AsyncCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = o => executeAsync.Invoke();
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
            return (Execution == null || Execution.IsCompleted) && (_canExecute?.Invoke(parameter) ?? true);
        }

        /// <summary>
        /// Method returning task which represents asynchronous execution of command
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        /// <returns>Task representing asynchronous execution of command</returns>
        public async Task ExecuteAsync(object parameter)
        {
            Execution = new TaskNotifier(_executeAsync(parameter));
            NotifyPropertyChanged(nameof(IsExecuting));
            CommandManager.InvalidateRequerySuggested();
            await Execution.TaskCompleted.ConfigureAwait(true);
            NotifyPropertyChanged(nameof(IsExecuting));
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Asynchronous method executed with command
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
                await ExecuteAsync(parameter).ConfigureAwait(false);
        }
    }
}
