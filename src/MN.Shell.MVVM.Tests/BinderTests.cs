using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MN.Shell.MVVM.Tests.BinderExample;
using Moq;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class BinderTests
    {
        [Test]
        public void ViewModelPropertyTest()
        {
            var exampleViewModel = new ExampleViewModel();
            var exampleView = new ExampleView();

            var viewManagerMock = new Mock<IViewManager>();
            viewManagerMock.Setup(vm => vm.GetViewFor(exampleViewModel)).Returns(exampleView).Verifiable();
            Binder.ViewManager = viewManagerMock.Object;

            var viewContainer = new ContentControl();
            Binder.SetViewModel(viewContainer, exampleViewModel);

            Assert.AreSame(exampleView, viewContainer.Content);
            viewManagerMock.VerifyAll();
        }

        [Test]
        public void SetContentTest()
        {
            var element = new ContentControl();
            var view = new FrameworkElement();

            Assert.Null(element.Content);

            Binder.SetContentView(element, view);

            Assert.AreSame(view, element.Content);
        }
    }

    namespace BinderExample
    {
        public class ExampleViewModel { }

        public class ExampleView : Control { }
    }
}
