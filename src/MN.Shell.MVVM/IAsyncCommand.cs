using System.Threading.Tasks;
using System.Windows.Input;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Defines command which can be executed asynchronously
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// TaskNotifier exposed to allow observing command execution progress via data binding
        /// </summary>
        TaskNotifier Execution { get; }

        /// <summary>
        /// Status of command execution - true if currently command is running, false otherwise
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// Executes command asynchronously
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        /// <returns>Task representing asynchronous operation</returns>
        Task ExecuteAsync(object parameter);
    }
}
