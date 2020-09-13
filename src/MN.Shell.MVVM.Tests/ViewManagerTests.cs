using MN.Shell.MVVM.Tests.Example1;
using NUnit.Framework;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ViewManagerTests
    {
        private ViewManager _viewManager;

        [SetUp]
        public void SetUp()
        {
            _viewManager = new ViewManager();
        }

        [Test]
        public void GetViewForTest()
        {
            var viewModel = new Example1ViewModel();
            var expectedViewType = new Example1View();

            var view = _viewManager.GetViewFor(viewModel);
            Assert.NotNull(view);
            Assert.AreEqual(expectedViewType.GetType(), view.GetType());
            Assert.AreSame(viewModel, view.DataContext);
        }

        [Test]
        public void GetViewForUsingViewFactoryTest()
        {
            var viewModel1 = new Example1ViewModel();
            var expectedView = new Example1View();

            bool factoryCalled = false;

            object viewFactory(Type type)
            {
                factoryCalled = true;
                Assert.AreEqual(typeof(Example1View), type);
                return expectedView;
            }

            var view1 = _viewManager.GetViewFor(viewModel1);
            Assert.NotNull(view1);
            Assert.False(factoryCalled);
            Assert.AreEqual(expectedView.GetType(), view1.GetType());

            _viewManager.ViewFactory = viewFactory;

            var viewModel2 = new Example1ViewModel();
            var view2 = _viewManager.GetViewFor(viewModel1);
            Assert.AreSame(expectedView, view2);
            Assert.True(factoryCalled);
        }

        [Test]
        public void GetViewForInvalidTest()
        {
            Assert.Throws<ArgumentNullException>(() => _viewManager.GetViewFor(null));

            Assert.Throws<InvalidOperationException>(() => _viewManager.GetViewFor(
                new Example2.Example2ViewModel()));

            Assert.Throws<InvalidOperationException>(() => _viewManager.GetViewFor(
                new Example3.Example3InvalidName()));

            Assert.Throws<InvalidOperationException>(() => _viewManager.GetViewFor(
                new Example4.Example4InvalidViewModel()));

            Assert.Throws<InvalidOperationException>(() => _viewManager.GetViewFor(
                new Example5.Example5AbstractViewModel()));
        }

        [Test]
        public void GetViewForNotViewAwareTest()
        {
            var viewModel1 = new Example1ViewModel();
            var viewModel2 = new Example1ViewModel();

            Assert.AreNotSame(viewModel1, viewModel2);

            var view1 = _viewManager.GetViewFor(viewModel1);
            Assert.NotNull(view1);
            Assert.AreEqual(typeof(Example1View), view1.GetType());
            Assert.AreSame(viewModel1, view1.DataContext);

            var view1a = _viewManager.GetViewFor(viewModel1);
            Assert.NotNull(view1a);
            Assert.AreEqual(typeof(Example1View), view1a.GetType());
            Assert.AreSame(viewModel1, view1a.DataContext);

            Assert.AreNotSame(view1, view1a);

            var view2 = _viewManager.GetViewFor(viewModel2);
            Assert.NotNull(view2);
            Assert.AreEqual(typeof(Example1View), view2.GetType());
            Assert.AreSame(viewModel2, view2.DataContext);

            Assert.AreNotSame(view1, view2);
            Assert.AreNotSame(view1a, view2);
        }

        [Test]
        public void GetViewForViewAwareTest()
        {
            var viewModel1 = new Example6.Example6ViewModel();
            var viewModel2 = new Example6.Example6ViewModel();

            Assert.AreNotSame(viewModel1, viewModel2);

            var view1 = _viewManager.GetViewFor(viewModel1);
            Assert.NotNull(view1);
            Assert.AreEqual(typeof(Example6.Example6View), view1.GetType());
            Assert.AreSame(viewModel1, view1.DataContext);

            var view1a = _viewManager.GetViewFor(viewModel1);
            Assert.NotNull(view1a);
            Assert.AreEqual(typeof(Example6.Example6View), view1a.GetType());
            Assert.AreSame(viewModel1, view1a.DataContext);

            Assert.AreSame(view1, view1a);

            var view2 = _viewManager.GetViewFor(viewModel2);
            Assert.NotNull(view2);
            Assert.AreEqual(typeof(Example6.Example6View), view2.GetType());
            Assert.AreSame(viewModel2, view2.DataContext);

            Assert.AreNotSame(view1, view2);
            Assert.AreNotSame(view1a, view2);
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
            public FrameworkElement View { get; private set; }
            public void AttachView(FrameworkElement view) => View = view;
        }
        public class Example6View : Control { }
    }
}
