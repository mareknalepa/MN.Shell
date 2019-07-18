using System.Collections.Generic;

namespace MN.Shell.Framework.StatusBar
{
    public interface IStatusBarProvider
    {
        IEnumerable<StatusBarItemViewModel> GetStatusBarItems();
    }
}
