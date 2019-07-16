using Caliburn.Micro;
using System.Windows.Input;

namespace MN.Shell.Framework
{
    public abstract class LayoutModuleBase : Screen, ILayoutModule
    {
        public ICommand CloseCommand { get; protected set; }
    }
}
