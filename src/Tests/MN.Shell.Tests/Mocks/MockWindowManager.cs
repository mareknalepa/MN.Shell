using MN.Shell.Core;
using MN.Shell.MVVM;
using System.Windows;

namespace MN.Shell.Tests.Mocks
{
    public class MockWindowManager : ShellWindowManager
    {
        public MockWindowManager(IViewManager viewManager) : base(viewManager) { }

        public new Window EnsureWindow(FrameworkElement view, bool isDialog) =>
            base.EnsureWindow(view, isDialog);
    }
}
