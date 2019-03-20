using MN.Shell.Core;
using System.Windows;

namespace MN.Shell.Tests.Mocks
{
    public class MockWindowManager : WindowManager
    {
        public new Window EnsureWindow(object model, object view, bool isDialog) =>
            base.EnsureWindow(model, view, isDialog);
    }
}
