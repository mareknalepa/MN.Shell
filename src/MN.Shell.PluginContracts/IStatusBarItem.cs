using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    public interface IStatusBarItem
    {
        StatusBarSide Side { get; }

        int Priority { get; }

        double MinWidth { get; }

        string Content { get; }

        ICommand Command { get; }
    }
}
