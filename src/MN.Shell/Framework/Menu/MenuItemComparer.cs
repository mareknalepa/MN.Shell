using MN.Shell.PluginContracts;
using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Framework.Menu
{
    public class MenuItemComparer : IComparer<IMenuItem>
    {
        public int Compare(IMenuItem x, IMenuItem y)
        {
            int xPathLength = x.Path?.Count() ?? 0;
            int yPathLength = y.Path?.Count() ?? 0;

            if (xPathLength != yPathLength)
                return xPathLength.CompareTo(yPathLength);

            if (x.Path != null && y.Path != null)
            {
                foreach (var pair in x.Path.Zip(y.Path, (first, second) => new { First = first, Second = second }))
                {
                    int compareResult = pair.First.CompareTo(pair.Second);
                    if (compareResult != 0)
                        return compareResult;
                }
            }

            if (x.GroupId != y.GroupId)
                return x.GroupId.CompareTo(y.GroupId);

            if (x.GroupOrder != y.GroupOrder)
                return x.GroupOrder.CompareTo(y.GroupOrder);

            return x.Name.CompareTo(y.Name);
        }
    }
}
