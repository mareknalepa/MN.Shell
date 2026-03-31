using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.IntegrationTests
{
    public sealed class ViewManagerTests
    {
        private readonly ViewManager _viewManager = new();

        [StaFact]
        public void GetViewFor_ReturnsCorrectResult()
        {
            var viewModel = new Example1.Example1ViewModel();

            var view = _viewManager.GetViewFor(viewModel);
            view.ShouldNotBeNull();
            view.ShouldBeOfType<Example1.Example1View>();
            view.DataContext.ShouldBeSameAs(viewModel);
        }

        [StaFact]
        public void GetViewFor_UsesViewStaFactory()
        {
            var viewModel = new Example1.Example1ViewModel();
            var expectedView = new Example1.Example1View();

            bool StaFactoryCalled = false;

            object viewStaFactory(Type type)
            {
                StaFactoryCalled = true;
                type.ShouldBe(typeof(Example1.Example1View));
                return expectedView;
            }

            var view1 = _viewManager.GetViewFor(viewModel);
            view1.ShouldNotBeNull();
            StaFactoryCalled.ShouldBeFalse();
            view1.ShouldBeOfType<Example1.Example1View>();

            _viewManager.ViewFactory = viewStaFactory;

            var view2 = _viewManager.GetViewFor(viewModel);
            view2.ShouldNotBeNull();
            StaFactoryCalled.ShouldBeTrue();
            view2.ShouldBeSameAs(expectedView);
        }

        [StaFact]
        public void GetViewFor_ThrowsForInvalidInput()
        {
            var act = () => _viewManager.GetViewFor(null!);
            act.ShouldThrow<ArgumentNullException>();

            act = () => _viewManager.GetViewFor(new Example2.Example2ViewModel());
            act.ShouldThrow<InvalidOperationException>();

            act = () => _viewManager.GetViewFor(new Example3.Example3InvalidName());
            act.ShouldThrow<InvalidOperationException>();

            act = () => _viewManager.GetViewFor(new Example4.Example4InvalidView());
            act.ShouldThrow<InvalidOperationException>();

            act = () => _viewManager.GetViewFor(new Example5.Example5AbstractViewModel());
            act.ShouldThrow<InvalidOperationException>();
        }

        [StaFact]
        public void GetViewFor_ReturnsCorrectResult_ForNotViewAware()
        {
            var viewModel1 = new Example1.Example1ViewModel();
            var viewModel2 = new Example1.Example1ViewModel();

            viewModel2.ShouldNotBeSameAs(viewModel1);

            var view1 = _viewManager.GetViewFor(viewModel1);
            view1.ShouldNotBeNull();
            view1.ShouldBeOfType<Example1.Example1View>();
            view1.DataContext.ShouldBeSameAs(viewModel1);

            var view1a = _viewManager.GetViewFor(viewModel1);
            view1a.ShouldNotBeNull();
            view1a.ShouldBeOfType<Example1.Example1View>();
            view1a.DataContext.ShouldBeSameAs(viewModel1);

            view1a.ShouldNotBeSameAs(view1);

            var view2 = _viewManager.GetViewFor(viewModel2);
            view2.ShouldNotBeNull();
            view2.ShouldBeOfType<Example1.Example1View>();
            view2.DataContext.ShouldBeSameAs(viewModel2);

            view2.ShouldNotBeSameAs(view1);
            view2.ShouldNotBeSameAs(view1a);
        }

        [StaFact]
        public void GetViewFor_ReturnsCorrectResult_ForViewAware()
        {
            var viewModel1 = new Example6.Example6ViewModel();
            var viewModel2 = new Example6.Example6ViewModel();

            viewModel2.ShouldNotBeSameAs(viewModel1);

            var view1 = _viewManager.GetViewFor(viewModel1);
            view1.ShouldNotBeNull();
            view1.ShouldBeOfType<Example6.Example6View>();
            view1.DataContext.ShouldBeSameAs(viewModel1);

            var view1a = _viewManager.GetViewFor(viewModel1);
            view1a.ShouldNotBeNull();
            view1a.ShouldBeOfType<Example6.Example6View>();
            view1a.DataContext.ShouldBeSameAs(viewModel1);

            view1a.ShouldBeSameAs(view1);

            var view2 = _viewManager.GetViewFor(viewModel2);
            view2.ShouldNotBeNull();
            view2.ShouldBeOfType<Example6.Example6View>();
            view2.DataContext.ShouldBeSameAs(viewModel2);

            view2.ShouldNotBeSameAs(view1);
            view2.ShouldNotBeSameAs(view1a);
        }
    }

    namespace Example1
    {
        public class Example1ViewModel { }
        public class Example1View : Control { }
    }

    namespace Example2
    {
        public class Example2ViewModel { }
    }

    namespace Example3
    {
        public class Example3InvalidName { }
    }

    namespace Example4
    {
        public class Example4InvalidViewModel { }
        public class Example4InvalidView { }
    }

    namespace Example5
    {
        public class Example5AbstractViewModel { }
        public abstract class Example5AbstractView { }
    }

    namespace Example6
    {
        public class Example6ViewModel : IViewAware
        {
            public FrameworkElement? View { get; private set; }
            public void AttachView(FrameworkElement view) => View = view;
        }
        public class Example6View : Control { }
    }
}
