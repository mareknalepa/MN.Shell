using System.Threading;
using MN.Shell.MVVM.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class WindowManagerTests
    {
        [Test]
        public void ShowWindowTest()
        {
            var viewModel = new MockWindowViewModel();

            var viewManagerMock = new Mock<IViewManager>(MockBehavior.Strict);
            viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockWindowView() { DataContext = viewModel });

            var windowManager = new WindowManager(viewManagerMock.Object);

            Assert.False(viewModel.WindowShown);

            windowManager.ShowWindow(viewModel);

            viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowWindowForUserControlViewTest()
        {
            var viewModel = new MockUserControlViewModel();

            var viewManagerMock = new Mock<IViewManager>(MockBehavior.Strict);
            viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockUserControlView() { DataContext = viewModel });

            var windowManager = new WindowManager(viewManagerMock.Object);

            Assert.False(viewModel.WindowShown);

            windowManager.ShowWindow(viewModel);

            viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowDialogResultTest()
        {
            var viewModel = new MockWindowViewModel();

            var viewManagerMock = new Mock<IViewManager>(MockBehavior.Strict);
            viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockWindowView() { DataContext = viewModel });

            var windowManager = new WindowManager(viewManagerMock.Object);

            Assert.False(viewModel.WindowShown);

            windowManager.ShowDialog(viewModel);

            viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }

        [Test]
        public void ShowDialogForUserControlViewTest()
        {
            var viewModel = new MockUserControlViewModel();

            var viewManagerMock = new Mock<IViewManager>(MockBehavior.Strict);
            viewManagerMock
                .Setup(x => x.GetViewFor(viewModel))
                .Returns(new MockUserControlView() { DataContext = viewModel });

            var windowManager = new WindowManager(viewManagerMock.Object);

            Assert.False(viewModel.WindowShown);

            windowManager.ShowDialog(viewModel);

            viewManagerMock.Verify(x => x.GetViewFor(viewModel));
            Assert.True(viewModel.WindowShown);
        }
    }
}
