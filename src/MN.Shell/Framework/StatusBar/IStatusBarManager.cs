using System.Collections.ObjectModel;

namespace MN.Shell.Framework.StatusBar
{
    public interface IStatusBarManager
    {
        ObservableCollection<StatusBarItemViewModel> StatusBarItems { get; }

        void CompileStatusBar();
    }
}
