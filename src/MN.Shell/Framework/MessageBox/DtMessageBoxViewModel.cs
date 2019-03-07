using MN.Shell.Framework.Dialogs;

namespace MN.Shell.Framework.MessageBox
{
    public class DtMessageBoxViewModel : MessageBoxViewModel
    {
        public DtMessageBoxViewModel()
            : base("Info", "This is an example of MVVM-aware MessageBox dialog.",
                  new[] { DialogButtonType.Ok })
        { }
    }
}
