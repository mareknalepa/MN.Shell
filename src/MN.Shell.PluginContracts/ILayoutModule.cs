using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    public interface ILayoutModule : IScreen
    {
        ICommand CloseCommand { get; }
    }
}
