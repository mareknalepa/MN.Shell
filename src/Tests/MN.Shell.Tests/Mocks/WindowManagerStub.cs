using MN.Shell.Core;
using MN.Shell.MVVM;
using System.Windows;

namespace MN.Shell.Tests.Mocks
{
    public sealed class WindowManagerStub(IViewManager viewManager)
        : ShellWindowManager(viewManager)
    {
        public new Window EnsureWindow(FrameworkElement view, bool isDialog) =>
            base.EnsureWindow(view, isDialog);
    }
}
