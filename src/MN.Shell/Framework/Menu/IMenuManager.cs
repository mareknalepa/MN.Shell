using System.Collections.ObjectModel;

namespace MN.Shell.Framework.Menu
{
    public interface IMenuManager
    {
        ObservableCollection<MenuItemViewModel> MenuItems { get; }

        void CompileMenu();
    }
}
