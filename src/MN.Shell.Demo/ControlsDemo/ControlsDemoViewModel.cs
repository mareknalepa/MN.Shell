using MN.Shell.Modules.Shell;
using MN.Shell.PluginContracts;

namespace MN.Shell.Demo.ControlsDemo
{
    public class ControlsDemoViewModel : DocumentBase
    {
        public ControlsDemoViewModel(ShellContext shellContext)
        {
            shellContext.ApplicationTitle = "MN.Shell Demo Application";
            Title = "Controls Demo";
        }
    }
}
