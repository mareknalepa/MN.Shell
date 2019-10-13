using MN.Shell.Framework.Dialogs;
using System;

namespace MN.Shell.Tests.Mocks
{
    public class MockDialogViewModel : DialogViewModelBase
    {
        public new void AddButton(DialogButtonType dialogButtonType) =>
            base.AddButton(dialogButtonType);

        public new void AddCustomButton(string caption, Action<DialogViewModelBase> action) =>
            base.AddCustomButton(caption, action);
    }
}
