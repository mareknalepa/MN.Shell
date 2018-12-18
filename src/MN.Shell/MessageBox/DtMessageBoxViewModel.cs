using MN.Shell.Dialogs;

namespace MN.Shell.MessageBox
{
    public class DtMessageBoxViewModel : MessageBoxViewModel
    {
        public DtMessageBoxViewModel()
            : base("Info", "This is an example of MVVM-aware MessageBox dialog.",
                  new[] { DialogButtonType.Ok })
        { }
    }
}
