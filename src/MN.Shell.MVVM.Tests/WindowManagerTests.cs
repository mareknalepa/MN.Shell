using System;
using System.Threading;
using System.Windows;
using MN.Shell.MVVM.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class WindowManagerTests
    {
        private Mock<IViewManager> _viewManagerMock;
        private WindowManager _windowManager;

        [SetUp]
        public void SetUp()
        {
            _viewManagerMock = new Mock<IViewManager>(MockBehavior.Strict);
            _windowManager = new WindowManager(_viewManagerMock.Object);
        }

        [Test]
        public void ShowWindowTest([Values] bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockWindowView() { DataContext = viewModel };

            Window ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow;

            bool windowShown = false;
            view.OnLoadedAction = window =>
            {
                windowShown = true;
                Assert.AreSame(viewModel, window.DataContext);
                Assert.Null(window.Owner);
            };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            Assert.False(windowShown);
            _windowManager.ShowWindow(viewModel);
            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(windowShown);
        }

        [Test]
        public void ShowWindowForUserControlTest([Values] bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockUserControlView() { DataContext = viewModel };

            Window ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow;

            bool windowShown = false;
            view.OnLoadedAction = userControl =>
            {
                windowShown = true;
                Assert.AreSame(viewModel, userControl.DataContext);
                var parentWindow = userControl.Parent as Window;
                Assert.NotNull(parentWindow);
                Assert.AreSame(viewModel, parentWindow.DataContext);
                Assert.Null(parentWindow.Owner);
            };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            Assert.False(windowShown);
            _windowManager.ShowWindow(viewModel);
            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(windowShown);
        }

        [Test]
        public void ShowDialogTest([Values] bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockWindowView() { DataContext = viewModel };

            Window ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow;

            bool windowShown = false;
            view.OnLoadedAction = window =>
            {
                windowShown = true;
                Assert.AreSame(viewModel, window.DataContext);
                if (isAnotherActiveWindow)
                    Assert.AreSame(ownerWindow, window.Owner);
                else
                    Assert.Null(window.Owner);
            };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            Assert.False(windowShown);
            _windowManager.ShowDialog(viewModel);
            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(windowShown);
        }

        [Test]
        public void ShowDialogForUserControlTest([Values] bool isAnotherActiveWindow)
        {
            var viewModel = new object();
            var view = new MockUserControlView() { DataContext = viewModel };

            Window ownerWindow = null;
            if (isAnotherActiveWindow)
            {
                ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
                ownerWindow.Show();
            }
            _windowManager.GetActiveWindow = () => ownerWindow;

            bool windowShown = false;
            view.OnLoadedAction = userControl =>
            {
                windowShown = true;
                Assert.AreSame(viewModel, userControl.DataContext);
                var parentWindow = userControl.Parent as Window;
                Assert.NotNull(parentWindow);
                Assert.AreSame(viewModel, parentWindow.DataContext);
                if (isAnotherActiveWindow)
                    Assert.AreSame(ownerWindow, parentWindow.Owner);
                else
                    Assert.Null(parentWindow.Owner);
            };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            Assert.False(windowShown);
            _windowManager.ShowDialog(viewModel);
            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(windowShown);
        }

        [Test]
        public void BindWindowTitleTest(
            [Values] bool isDialog,
            [Values] bool isUserControl)
        {
            const string title = "Window Title";

            var viewModelMock = new Mock<IHaveTitle>(MockBehavior.Strict);
            viewModelMock.Setup(x => x.Title).Returns(title);
            var viewModel = viewModelMock.Object;

            string actualTitle = string.Empty;

            FrameworkElement view = null;
            if (isUserControl)
            {
                var userControlView = new MockUserControlView() { DataContext = viewModel };
                userControlView.OnLoadedAction = userControl => actualTitle = (userControl.Parent as Window)?.Title;
                view = userControlView;
            }
            else
            {
                var windowView = new MockWindowView() { DataContext = viewModel };
                windowView.OnLoadedAction = window => actualTitle = window.Title;
                Assert.True(string.IsNullOrEmpty(windowView.Title));
                view = windowView;
            }

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            if (isDialog)
                _windowManager.ShowDialog(viewModel);
            else
                _windowManager.ShowWindow(viewModel);

            Assert.AreEqual(title, actualTitle);
        }

        [Test]
        public void BindWindowTitleExistingTest(
            [Values] bool isDialog,
            [Values] bool isUserControl)
        {
            const string title = "Window Title";
            const string existingTitle = "Existing Window Title";

            var viewModelMock = new Mock<IHaveTitle>(MockBehavior.Strict);
            viewModelMock.Setup(x => x.Title).Returns(title);
            var viewModel = viewModelMock.Object;

            string actualTitle = string.Empty;

            FrameworkElement view = null;
            if (isUserControl)
            {
                var userControlView = new MockUserControlView() { DataContext = viewModel };
                userControlView.OnLoadedAction = userControl => actualTitle = (userControl.Parent as Window)?.Title;
                view = userControlView;
            }
            else
            {
                var windowView = new MockWindowView() { DataContext = viewModel, Title = existingTitle };
                windowView.OnLoadedAction = window => actualTitle = window.Title;
                Assert.AreEqual(existingTitle, windowView.Title);
                view = windowView;
            }

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            if (isDialog)
                _windowManager.ShowDialog(viewModel);
            else
                _windowManager.ShowWindow(viewModel);

            if (isUserControl)
                Assert.AreEqual(title, actualTitle);
            else
                Assert.AreEqual(existingTitle, actualTitle);
        }

        [Test]
        public void ShowWindowViewManagerExceptionTest([Values] bool isDialog)
        {
            var viewModel = new object();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<ArgumentNullException>();

            if (isDialog)
                Assert.Throws<ArgumentNullException>(() => _windowManager.ShowDialog(viewModel));
            else
                Assert.Throws<ArgumentNullException>(() => _windowManager.ShowWindow(viewModel));

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<InvalidOperationException>();

            if (isDialog)
                Assert.Throws<InvalidOperationException>(() => _windowManager.ShowDialog(viewModel));
            else
                Assert.Throws<InvalidOperationException>(() => _windowManager.ShowWindow(viewModel));
        }
    }
}
