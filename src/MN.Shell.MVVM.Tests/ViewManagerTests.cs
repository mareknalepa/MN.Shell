using System;
using System.Threading;
using System.Windows.Controls;
using NUnit.Framework;

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
        public void LocateViewForTest()
        {
            var viewModel = new Example1.Example1ViewModel();
            var viewType = _viewManager.LocateViewFor(viewModel);

            Assert.NotNull(viewType);
            Assert.AreEqual(typeof(Example1.Example1View), viewType);

            Assert.Throws<ArgumentNullException>(() => _viewManager.LocateViewFor(null));

            Assert.Throws<ViewManagerException>(() => _viewManager.LocateViewFor(
                new Example2.Example2ViewModel()));

            Assert.Throws<ViewManagerException>(() => _viewManager.LocateViewFor(
                new Example3.Example3InvalidName()));
        }

        [Test]
        public void CreateViewForTest()
        {
            var viewModel = new Example1.Example1ViewModel();
            var view = _viewManager.CreateViewFor(typeof(Example1.Example1View));

            Assert.NotNull(view);
            Assert.AreEqual(typeof(Example1.Example1View), view.GetType());

            Assert.Throws<ArgumentNullException>(() => _viewManager.CreateViewFor(null));

            Assert.Throws<ViewManagerException>(() => _viewManager.CreateViewFor(
                new Example4.Example4InvalidView().GetType()));

            Assert.Throws<ViewManagerException>(() => _viewManager.CreateViewFor(
                typeof(Example5.Example5AbstractView)));
        }

        [Test]
        public void BindViewToViewModelTest()
        {
            var viewModel = new Example1.Example1ViewModel();
            var view = new Example1.Example1View();

            Assert.Null(view.DataContext);

            _viewManager.BindViewToViewModel(view, viewModel);

            Assert.AreSame(viewModel, view.DataContext);

            Assert.Throws<ArgumentNullException>(() => _viewManager.BindViewToViewModel(null, viewModel));
            Assert.Throws<ArgumentNullException>(() => _viewManager.BindViewToViewModel(view, null));
            Assert.Throws<ArgumentNullException>(() => _viewManager.BindViewToViewModel(null, null));
        }
    }

    namespace Example1
    {
        class Example1ViewModel { }

        class Example1View : Control { }
    }

    namespace Example2
    {
        class Example2ViewModel { }
    }

    namespace Example3
    {
        class Example3InvalidName { }
    }

    namespace Example4
    {
        class Example4InvalidView { }
    }

    namespace Example5
    {
#pragma warning disable 0169
        abstract class Example5AbstractView { }
#pragma warning restore 0169
    }
}
