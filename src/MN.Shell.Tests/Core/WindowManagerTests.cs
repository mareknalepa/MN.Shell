using MN.Shell.Tests.Mocks;
using NUnit.Framework;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class WindowManagerTests
    {
        [Test]
        public void WindowManagerEnsureWindowForWindowTest()
        {
            MockWindowManager windowManager = new MockWindowManager();

            var vm = new MockWindowViewModel();
            Window window = new MockWindowView();
            var view = windowManager.EnsureWindow(vm, window, false);

            Assert.AreSame(window, view);
        }

        [Test]
        public void WindowManagerEnsureWindowForUserControlTest()
        {
            MockWindowManager windowManager = new MockWindowManager();

            var vm = new MockUserControlViewModel();
            UserControl userControl = new MockUserControlView();
            var view = windowManager.EnsureWindow(vm, userControl, false);

            Assert.AreSame(userControl, view.Content);
        }
    }
}
