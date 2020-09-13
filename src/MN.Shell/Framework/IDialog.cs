using MN.Shell.Framework.Dialogs;
using MN.Shell.MVVM;
using System.Collections.ObjectModel;

namespace MN.Shell.Framework
{
    public interface IDialog : IScreen
    {
        ObservableCollection<DialogButton> Buttons { get; }
        DialogButton SelectedButton { get; set; }
    }
}
