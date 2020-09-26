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
        /// Executes command asynchronously
        /// </summary>
        /// <param name="parameter">Internal parameter which can be optionally passed to command</param>
        /// <returns>Task representing asynchronous operation</returns>
        Task ExecuteAsync(object parameter);
    }
}
