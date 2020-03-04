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
        public void ShowWindowTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockWindowView() { DataContext = viewModel });

            Assert.False(viewModel.WindowShown);

            _windowManager.ShowWindow(viewModel);

            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowWindowForUserControlTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockUserControlView() { DataContext = viewModel });

            Assert.False(viewModel.WindowShown);

            _windowManager.ShowWindow(viewModel);

            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowDialogTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockWindowView() { DataContext = viewModel });

            Assert.False(viewModel.WindowShown);

            _windowManager.ShowDialog(viewModel);

            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowDialogForUserControlTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockUserControlView() { DataContext = viewModel });

            Assert.False(viewModel.WindowShown);

            _windowManager.ShowDialog(viewModel);

            _viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowWindowOwnerTest()
        {
            var ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
            ownerWindow.Show();

            var viewModel = new MockWindowViewModel();
            var view = new MockWindowView() { DataContext = viewModel };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            _windowManager.GetActiveWindow = () => ownerWindow;
            _windowManager.ShowWindow(viewModel);

            Assert.Null(view.Owner);
            Assert.False(viewModel.HasOwnerViewInOwnedWindows);
        }

        [Test]
        public void ShowWindowForUserControlOwnerTest()
        {
            var ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
            ownerWindow.Show();

            var viewModel = new MockWindowViewModel();
            var view = new MockUserControlView() { DataContext = viewModel };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            _windowManager.GetActiveWindow = () => ownerWindow;
            _windowManager.ShowWindow(viewModel);

            Assert.True(view.Parent is Window);
            Assert.Null((view.Parent as Window).Owner);
            Assert.False(viewModel.HasOwnerViewInOwnedWindows);
        }

        [Test]
        public void ShowDialogOwnerTest()
        {
            var ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
            ownerWindow.Show();

            var viewModel = new MockWindowViewModel();
            var view = new MockWindowView() { DataContext = viewModel };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            _windowManager.GetActiveWindow = () => ownerWindow;
            _windowManager.ShowDialog(viewModel);

            Assert.NotNull(view.Owner);
            Assert.AreEqual(ownerWindow, view.Owner);
            Assert.True(viewModel.HasOwnerViewInOwnedWindows);
        }

        [Test]
        public void ShowDialogForUserControlOwnerTest()
        {
            var ownerWindow = new Window() { Height = 1, Width = 1, WindowState = WindowState.Minimized };
            ownerWindow.Show();

            var viewModel = new MockWindowViewModel();
            var view = new MockUserControlView() { DataContext = viewModel };

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(view);

            _windowManager.GetActiveWindow = () => ownerWindow;
            _windowManager.ShowDialog(viewModel);

            Assert.True(view.Parent is Window);
            Assert.NotNull((view.Parent as Window).Owner);
            Assert.AreEqual(ownerWindow, (view.Parent as Window).Owner);
            Assert.True(viewModel.HasOwnerViewInOwnedWindows);
        }

        [Test]
        public void ShowWindowViewManagerExceptionTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<ArgumentNullException>();

            Assert.Throws<ArgumentNullException>(() => _windowManager.ShowWindow(viewModel));

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<InvalidOperationException>();

            Assert.Throws<InvalidOperationException>(() => _windowManager.ShowWindow(viewModel));
        }

        [Test]
        public void ShowDialogViewManagerExceptionTest()
        {
            var viewModel = new MockWindowViewModel();

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<ArgumentNullException>();

            Assert.Throws<ArgumentNullException>(() => _windowManager.ShowDialog(viewModel));

            _viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Throws<InvalidOperationException>();

            Assert.Throws<InvalidOperationException>(() => _windowManager.ShowDialog(viewModel));
        }
    }
}
