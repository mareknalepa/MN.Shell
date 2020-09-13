using NUnit.Framework;
using System;
using System.ComponentModel;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class PropertyChangedBaseTests
    {
        private class PropertyChangedBaseTestingMock : PropertyChangedBase
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

        private PropertyChangedBaseTestingMock _model;

        [SetUp]
        public void SetUp()
        {
            _model = new PropertyChangedBaseTestingMock();
        }

        [Test]
        public void NotifyPropertyChangedTest()
        {
            Assert.Throws<ArgumentNullException>(() => _model.CallNotifyPropertyChanged(null));

            bool handlerFired = false;
            void handler(object sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                Assert.AreEqual(_model, sender);
                Assert.AreEqual(nameof(PropertyChangedBaseTestingMock.NotifyPropertyChangedTestProperty), e.PropertyName);
            }

            _model.PropertyChanged += handler;
            _model.NotifyPropertyChangedTestProperty = true;
            Assert.True(handlerFired);
            _model.PropertyChanged -= handler;
        }

        [Test]
        public void SetTest()
        {
            bool handlerFired = false;
            void handler(object sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                Assert.AreEqual(_model, sender);
                Assert.AreEqual(nameof(PropertyChangedBaseTestingMock.SetTestProperty), e.PropertyName);
            }

            _model.PropertyChanged += handler;
            _model.SetTestProperty = true;
            Assert.True(handlerFired);
            _model.PropertyChanged -= handler;
        }

        [Test]
        public void RefreshTest()
        {
            bool handlerFired = false;
            void handler(object sender, PropertyChangedEventArgs e)
            {
                handlerFired = true;
                Assert.AreEqual(_model, sender);
                Assert.AreEqual(string.Empty, e.PropertyName);
            }

            _model.PropertyChanged += handler;
            _model.CallRefresh();
            Assert.True(handlerFired);
            _model.PropertyChanged -= handler;
        }
    }
}
