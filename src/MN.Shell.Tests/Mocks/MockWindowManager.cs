using MN.Shell.MVVM;
using System.Windows;

namespace MN.Shell.Tests.Mocks
{
    public class MockWindowManager : Shell.Core.WindowManager
    {
        public MockWindowManager(IViewManager viewManager) : base(viewManager) { }

        public new Window EnsureWindow(FrameworkElement view, bool isDialog) =>
            base.EnsureWindow(view, isDialog);
    }
}
