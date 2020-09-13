using MN.Shell.MVVM.Tests.Mocks;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ScreenTests
    {
        private Mock<Screen> _screenMock;
        private Screen _screen;

        [SetUp]
        public void SetUp()
        {
            _screenMock = new Mock<Screen>(MockBehavior.Loose) { CallBase = true };
            _screen = _screenMock.Object;
        }

        [Test]
        public void AttachViewTest()
        {
            Assert.Throws<ArgumentNullException>(() => (_screen as IViewAware).AttachView(null));

            var view = new Control() { DataContext = _screen };

            Assert.Null(_screen.View);
            (_screen as IViewAware).AttachView(view);
            Assert.NotNull(_screen.View);
            Assert.AreSame(view, _screen.View);

            var anotherView = new Control() { DataContext = _screen };

            Assert.Throws<InvalidOperationException>(() => (_screen as IViewAware).AttachView(anotherView));
            Assert.AreSame(view, _screen.View);
        }

        [Test]
        public void TitlePropertyChangedTest()
        {
            bool handler1Fired = false;

            void handler1(object sender, PropertyChangedEventArgs e)
            {
                Assert.AreEqual(nameof(Screen.Title), e.PropertyName);
                handler1Fired = true;
            }

            _screen.PropertyChanged += handler1;
            _screen.Title = "New Title";
            _screen.PropertyChanged -= handler1;

            Assert.True(handler1Fired);
        }

        [Test]
        public void StateTest()
        {
            var screen = new MockScreen();

            Assert.AreEqual(LifecycleState.Undefined, screen.State);
            Assert.False(screen.IsActive);

            screen.Close();
            Assert.AreEqual(LifecycleState.Undefined, screen.State);
            Assert.False(screen.IsActive);

            screen.Activate();
            Assert.AreEqual(LifecycleState.Active, screen.State);
            Assert.True(screen.IsActive);

            screen.Deactivate();
            Assert.AreEqual(LifecycleState.Inactive, screen.State);
            Assert.False(screen.IsActive);

            screen.Close();
            Assert.AreEqual(LifecycleState.Closed, screen.State);
            Assert.False(screen.IsActive);

            screen.Activate();
            Assert.AreEqual(LifecycleState.Closed, screen.State);
            Assert.False(screen.IsActive);

            screen.Deactivate();
            Assert.AreEqual(LifecycleState.Closed, screen.State);
            Assert.False(screen.IsActive);
        }

        [Test]
        public void OnInitializedTest()
        {
            var screen = new MockScreen();

            Assert.AreEqual(0, screen.OnInitializedCalledCount);

            screen.Activate();
            Assert.AreEqual(1, screen.OnInitializedCalledCount);

            screen.Deactivate();
            screen.Activate();
            Assert.AreEqual(1, screen.OnInitializedCalledCount);

            screen.Close();
            Assert.AreEqual(1, screen.OnInitializedCalledCount);
        }

        [Test]
        public void OnActivatedTest()
        {
            var screen = new MockScreen();

            Assert.AreEqual(0, screen.OnActivatedCalledCount);

            screen.Activate();
            Assert.AreEqual(1, screen.OnActivatedCalledCount);

            screen.Activate();
            Assert.AreEqual(1, screen.OnActivatedCalledCount);

            screen.Deactivate();
            screen.Activate();
            Assert.AreEqual(2, screen.OnActivatedCalledCount);

            screen.Activate();
            Assert.AreEqual(2, screen.OnActivatedCalledCount);

            screen.Close();
            screen.Activate();
            Assert.AreEqual(2, screen.OnActivatedCalledCount);
        }

        [Test]
        public void OnDeactivatedTest()
        {
            var screen = new MockScreen();

            Assert.AreEqual(0, screen.OnDeactivatedCalledCount);

            screen.Deactivate();
            Assert.AreEqual(0, screen.OnDeactivatedCalledCount);

            screen.Activate();
            screen.Deactivate();
            Assert.AreEqual(1, screen.OnDeactivatedCalledCount);

            screen.Deactivate();
            Assert.AreEqual(1, screen.OnDeactivatedCalledCount);

            screen.Activate();
            screen.Deactivate();
            Assert.AreEqual(2, screen.OnDeactivatedCalledCount);

            screen.Deactivate();
            Assert.AreEqual(2, screen.OnDeactivatedCalledCount);

            screen.Activate();
            screen.Close();
            screen.Deactivate();
            Assert.AreEqual(2, screen.OnDeactivatedCalledCount);
        }

        [Test]
        public void OnClosedTest()
        {
            var screen = new MockScreen();

            Assert.AreEqual(0, screen.OnClosedCalledCount);

            screen.Close();
            Assert.AreEqual(0, screen.OnClosedCalledCount);

            screen.Deactivate();
            screen.Close();
            Assert.AreEqual(0, screen.OnClosedCalledCount);

            screen.Activate();
            screen.Close();
            Assert.AreEqual(1, screen.OnClosedCalledCount);

            screen.Close();
            Assert.AreEqual(1, screen.OnClosedCalledCount);

            screen.Activate();
            screen.Deactivate();
            screen.Close();
            Assert.AreEqual(1, screen.OnClosedCalledCount);
        }

        [Test]
        public void RequestCloseTest([Values] bool? result)
        {
            bool handlerFired = false;
            void CloseRequestedHandler(object sender, bool? dialogResult)
            {
                handlerFired = true;
                Assert.AreEqual(result, dialogResult);
            }

            _screen.CloseRequested += CloseRequestedHandler;
            _screen.RequestClose(result);
            _screen.CloseRequested -= CloseRequestedHandler;

            Assert.True(handlerFired);
        }
    }
}
