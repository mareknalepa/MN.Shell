using MN.Shell.Core.Dialogs;

namespace MN.Shell.Core.MessageBox
{
    public class DtMessageBoxViewModel : MessageBoxViewModel
    {
        public DtMessageBoxViewModel()
            : base("Info", "This is an example of MVVM-aware MessageBox dialog.",
                  new[] { DialogButtonType.Ok })
        { }
    }
}
