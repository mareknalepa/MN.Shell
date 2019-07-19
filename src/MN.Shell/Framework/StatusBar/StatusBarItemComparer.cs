using System.Collections.Generic;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarItemComparer : IComparer<StatusBarItemViewModel>
    {
        public int Compare(StatusBarItemViewModel x, StatusBarItemViewModel y)
        {
            if (x.Side != y.Side)
                return x.Side.CompareTo(y.Side);

            if (x.Priority != y.Priority)
            {
                if (x.Side == StatusBarSide.Left)
                    return y.Priority.CompareTo(x.Priority);
                else
                    return x.Priority.CompareTo(y.Priority);
            }

            return x.Content.CompareTo(y.Content);
        }
    }
}
