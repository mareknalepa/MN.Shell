using MN.Shell.Framework.Dialogs;
using System.Collections.Generic;

namespace MN.Shell.Tests.Mocks
{
    public class MockDialogViewModel : DialogViewModelBase
    {
        public new void CreateButtons(IEnumerable<DialogButtonType> dialogButtonTypes) =>
            base.CreateButtons(dialogButtonTypes);
    }
}
