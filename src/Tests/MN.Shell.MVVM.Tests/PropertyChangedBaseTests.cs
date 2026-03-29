using System.ComponentModel;

namespace MN.Shell.MVVM.Tests
{
    public sealed class PropertyChangedBaseTests
    {
        private sealed class PropertyChangedBaseTestingMock : PropertyChangedBase
        {
            public void CallNotifyPropertyChanged(string propertyName) => NotifyPropertyChanged(propertyName);

            public void CallRefresh() => Refresh();

            public bool NotifyPropertyChangedTestProperty
            {
                get => true;
                set { NotifyPropertyChanged(); }
            }

            private bool _setTestProperty;

            public bool SetTestProperty
            {
                get => _setTestProperty;
                set => Set(ref _setTestProperty, value);
            }
        }

        private readonly PropertyChangedBaseTestingMock _model = new();

        [Fact]
        public void NotifyPropertyChanged_RaisesEvent()
        {
            Assert.Throws<ArgumentNullException>(() => _model.CallNotifyPropertyChanged(null!));

            bool handlerFired = false;
            void handler(object? sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                sender.ShouldBe(_model);
                e.PropertyName.ShouldBe(nameof(PropertyChangedBaseTestingMock.NotifyPropertyChangedTestProperty));
            }

            _model.PropertyChanged += handler;
            _model.NotifyPropertyChangedTestProperty = true;
            handlerFired.ShouldBeTrue();
            _model.PropertyChanged -= handler;
        }

        [Fact]
        public void SetTest()
        {
            bool handlerFired = false;
            void handler(object? sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                sender.ShouldBe(_model);
                e.PropertyName.ShouldBe(nameof(PropertyChangedBaseTestingMock.SetTestProperty));
            }

            _model.PropertyChanged += handler;
            _model.SetTestProperty = true;
            handlerFired.ShouldBeTrue();
            _model.PropertyChanged -= handler;
        }

        [Fact]
        public void RefreshTest()
        {
            bool handlerFired = false;
            void handler(object? sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                sender.ShouldBe(_model);
                e.PropertyName.ShouldBeEmpty();
            }

            _model.PropertyChanged += handler;
            _model.CallRefresh();
            handlerFired.ShouldBeTrue();
            _model.PropertyChanged -= handler;
        }
    }
}
