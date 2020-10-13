using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Basic interface for all view models presented in docking layout
    /// </summary>
    public interface ILayoutModule : IScreen
    {
        /// <summary>
        /// Command used to close given layout module
        /// </summary>
        ICommand CloseCommand { get; }
    }
}
