using MN.Shell.MVVM;
using MN.Shell.Tests.Mocks;
using Moq;
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
        private IViewManager _viewManager;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _viewManager = new Mock<IViewManager>(MockBehavior.Loose).Object;
        }

        [Test]
        public void WindowManagerEnsureWindowForWindowTest()
        {
            var windowManager = new MockWindowManager(_viewManager);

            Window window = new MockWindowView();
            var view = windowManager.EnsureWindow(window, false);

            Assert.AreSame(window, view);
        }

        [Test]
        public void WindowManagerEnsureWindowForUserControlTest()
        {
            var windowManager = new MockWindowManager(_viewManager);

            UserControl userControl = new MockUserControlView();
            var view = windowManager.EnsureWindow(userControl, false);

            Assert.AreSame(userControl, view.Content);
        }
    }
}
