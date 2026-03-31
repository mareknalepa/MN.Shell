using MN.Shell.MVVM;
using MN.Shell.UnitTests.Mocks;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.UnitTests.Core
{
    public sealed class WindowManagerTests
    {
        private readonly IViewManager _viewManager;
        private readonly WindowManagerStub _windowManager;

        public WindowManagerTests()
        {
            _viewManager = Substitute.For<IViewManager>();
            _windowManager = new(_viewManager);
        }

        [StaFact]
        public void EnsureWindow_ReturnsInput_ForWindows()
        {
            Window window = new MockWindowView();
            var view = _windowManager.EnsureWindow(window, false);

            view.ShouldBeSameAs(window);
        }

        [StaFact]
        public void EnsureWindow_ReturnsControlWrappedInWindow_ForUserControl()
        {
            UserControl userControl = new MockUserControlView();
            var view = _windowManager.EnsureWindow(userControl, false);

            view.Content.ShouldBeSameAs(userControl);
        }
    }
}
