using MN.Shell.MVVM.Tests.Mocks;
using System.ComponentModel;
using System.Windows.Controls;

namespace MN.Shell.MVVM.Tests
{
    public sealed class ScreenTests
    {
        private readonly Screen _screen = new ScreenStub();

        [StaFact]
        public void AttachView_AttachesViewToScreen()
        {
            var act = () => _screen.AttachView(null!);
            act.ShouldThrow<ArgumentNullException>();

            var view = new Control() { DataContext = _screen };

            _screen.View.ShouldBeNull();
            _screen.AttachView(view);
            _screen.View.ShouldBeSameAs(view);

            var anotherView = new Control() { DataContext = _screen };

            act = () => _screen.AttachView(anotherView);
            act.ShouldThrow<InvalidOperationException>();

            _screen.View.ShouldBeSameAs(view);
        }

        [StaFact]
        public void TitleSetter_NotifiesPropertyChanged()
        {
            bool handlerFired = false;

            void handler(object? sender, PropertyChangedEventArgs e)
            {
                e.PropertyName.ShouldBe(nameof(Screen.Title));
                handlerFired = true;
            }

            _screen.PropertyChanged += handler;
            _screen.Title = "New Title";
            _screen.PropertyChanged -= handler;

            handlerFired.ShouldBeTrue();
        }

        [StaFact]
        public void ChangingLifecycle_SetsProperties()
        {
            var screen = new ScreenStub();

            screen.State.ShouldBe(LifecycleState.Undefined);
            screen.IsActive.ShouldBeFalse();

            screen.Close();
            screen.State.ShouldBe(LifecycleState.Undefined);
            screen.IsActive.ShouldBeFalse();

            screen.Activate();
            screen.State.ShouldBe(LifecycleState.Active);
            screen.IsActive.ShouldBeTrue();

            screen.Deactivate();
            screen.State.ShouldBe(LifecycleState.Inactive);
            screen.IsActive.ShouldBeFalse();

            screen.Close();
            screen.State.ShouldBe(LifecycleState.Closed);
            screen.IsActive.ShouldBeFalse();

            screen.Activate();
            screen.State.ShouldBe(LifecycleState.Closed);
            screen.IsActive.ShouldBeFalse();

            screen.Deactivate();
            screen.State.ShouldBe(LifecycleState.Closed);
            screen.IsActive.ShouldBeFalse();
        }

        [StaFact]
        public void Activate_CallsOnInitialized_OnlyOnTheFirstTime()
        {
            var screen = new ScreenStub();

            screen.OnInitializedCalledCount.ShouldBe(0);

            screen.Activate();
            screen.OnInitializedCalledCount.ShouldBe(1);

            screen.Deactivate();
            screen.Activate();
            screen.OnInitializedCalledCount.ShouldBe(1);

            screen.Close();
            screen.OnInitializedCalledCount.ShouldBe(1);
        }

        [StaFact]
        public void Activate_CallsOnActivated_OnEveryActivation()
        {
            var screen = new ScreenStub();

            screen.OnActivatedCalledCount.ShouldBe(0);

            screen.Activate();
            screen.OnActivatedCalledCount.ShouldBe(1);

            screen.Activate();
            screen.OnActivatedCalledCount.ShouldBe(1);

            screen.Deactivate();
            screen.Activate();
            screen.OnActivatedCalledCount.ShouldBe(2);

            screen.Activate();
            screen.OnActivatedCalledCount.ShouldBe(2);

            screen.Close();
            screen.Activate();
            screen.OnActivatedCalledCount.ShouldBe(2);
        }

        [StaFact]
        public void Deactivate_CallsOnDeactivated_OnEveryDeactivation()
        {
            var screen = new ScreenStub();

            screen.OnDeactivatedCalledCount.ShouldBe(0);

            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(0);

            screen.Activate();
            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(1);

            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(1);

            screen.Activate();
            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(2);

            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(2);

            screen.Activate();
            screen.Close();
            screen.Deactivate();
            screen.OnDeactivatedCalledCount.ShouldBe(2);
        }

        [StaFact]
        public void Close_CallsOnClosed()
        {
            var screen = new ScreenStub();

            screen.OnClosedCalledCount.ShouldBe(0);

            screen.Close();
            screen.OnClosedCalledCount.ShouldBe(0);

            screen.Deactivate();
            screen.Close();
            screen.OnClosedCalledCount.ShouldBe(0);

            screen.Activate();
            screen.Close();
            screen.OnClosedCalledCount.ShouldBe(1);

            screen.Close();
            screen.OnClosedCalledCount.ShouldBe(1);

            screen.Activate();
            screen.Deactivate();
            screen.Close();
            screen.OnClosedCalledCount.ShouldBe(1);
        }

        [StaTheory]
        [InlineData(false)]
        [InlineData(true)]
        [InlineData(null)]
        public void RequestClose_RaisesEvent(bool? expectedResult)
        {
            bool handlerFired = false;
            void CloseRequestedHandler(object? sender, bool? dialogResult)
            {
                handlerFired = true;
                dialogResult.ShouldBe(expectedResult);
            }

            _screen.CloseRequested += CloseRequestedHandler;
            _screen.RequestClose(expectedResult);
            _screen.CloseRequested -= CloseRequestedHandler;

            handlerFired.ShouldBeTrue();
        }
    }
}
