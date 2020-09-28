using MN.Shell.Framework;
using MN.Shell.Modules.Shell;

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
