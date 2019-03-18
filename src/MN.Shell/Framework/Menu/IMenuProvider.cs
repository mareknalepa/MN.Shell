using System.Collections.Generic;

namespace MN.Shell.Framework.Menu
{
    public interface IMenuProvider
    {
        IEnumerable<MenuItem> GetMenuItems();
    }
}
