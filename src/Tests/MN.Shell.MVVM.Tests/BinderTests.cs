using MN.Shell.MVVM.Tests.BinderExample;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.Tests
{
    public sealed class BinderTests
    {
        [StaFact]
        public void SetViewModel_BindsViewModelAndView()
        {
            var exampleViewModel = new ExampleViewModel();
            var exampleView = new ExampleView();

            var viewManager = Substitute.For<IViewManager>();
            viewManager.GetViewFor(exampleViewModel).Returns(exampleView);

            Binder.ViewManager = viewManager;

            var viewContainer = new ContentControl();
            Binder.SetViewModel(viewContainer, exampleViewModel);

            viewContainer.Content.ShouldBeSameAs(exampleView);
            viewManager.Received(1).GetViewFor(exampleViewModel);
        }

        [StaFact]
        public void SetContent_SetsViewAsContent()
        {
            var element = new ContentControl();
            var view = new FrameworkElement();

            element.Content.ShouldBeNull();

            Binder.SetContentView(element, view);

            element.Content.ShouldBeSameAs(view);
        }
    }

    namespace BinderExample
    {
        public sealed class ExampleViewModel { }

        public sealed class ExampleView : Control { }
    }
}
