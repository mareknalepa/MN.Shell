using Caliburn.Micro;
using MN.Shell.Framework;
using MN.Shell.Framework.Dialogs;
using System.Collections.ObjectModel;

namespace MN.Shell.Tests.Mocks
{
    public class MockDialogViewModel : Screen, IDialog
    {
        public ObservableCollection<DialogButton> Buttons { get; } = new ObservableCollection<DialogButton>();

        public DialogButton SelectedButton { get; set; }
    }
}
