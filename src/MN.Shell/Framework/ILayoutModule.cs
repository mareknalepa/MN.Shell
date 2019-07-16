using Caliburn.Micro;
using System.Windows.Input;

namespace MN.Shell.Framework
{
    public interface ILayoutModule : IScreen
    {
        string ContentId { get; }
        ICommand CloseCommand { get; }
    }
}
