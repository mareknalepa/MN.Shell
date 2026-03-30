using MN.Shell.Framework;
using MN.Shell.Framework.Dialogs;
using MN.Shell.MVVM;
using System.Collections.ObjectModel;

namespace MN.Shell.Tests.Mocks
{
    public sealed class MockDialogViewModel : Screen, IDialog
    {
        public ObservableCollection<DialogButton> Buttons { get; } = [];

        public DialogButton? SelectedButton { get; set; }
    }
}
