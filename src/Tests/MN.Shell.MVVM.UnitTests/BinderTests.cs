using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.UnitTests
{
    public sealed class BinderTests
    {
        [StaFact]
        public void SetViewModel_BindsViewModelAndView()
        {
            var exampleViewModel = new object();
            var exampleView = Substitute.For<Control>();

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
}
