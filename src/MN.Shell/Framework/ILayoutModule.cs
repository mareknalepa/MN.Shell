using Caliburn.Micro;
using System.Windows.Input;

namespace MN.Shell.Framework
{
    public interface ILayoutModule : IScreen
    {
        ICommand CloseCommand { get; }
    }
}
