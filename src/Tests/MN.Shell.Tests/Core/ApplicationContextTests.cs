using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class ApplicationContextTests
    {
        private Mock<IServiceProvider> _serviceProviderMock = new Mock<IServiceProvider>();
        private ApplicationContext _applicationContext = new ApplicationContext(new Mock<IServiceProvider>().Object);

        [SetUp]
        public void SetUp()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _applicationContext = new ApplicationContext(_serviceProviderMock.Object);
        }

        [Test]
        public void ApplicationTitleSetterRaisesEventTest()
        {
            bool handlerCalled = false;

            void OnApplicationTitleChanged(object? sender, string newTitle)
            {
                handlerCalled = true;
                Assert.AreEqual("New Title", newTitle);
            }

            _applicationContext.ApplicationTitleChanged += OnApplicationTitleChanged;
            _applicationContext.ApplicationTitle = "New Title";
            _applicationContext.ApplicationTitleChanged -= OnApplicationTitleChanged;

            Assert.True(handlerCalled);
        }

        [Test]
        public void RequestApplicationExitTest()
        {
            bool handlerCalled = false;

            void OnApplicationExitRequested(object? sender, EventArgs e) => handlerCalled = true;

            _applicationContext.ApplicationExitRequested += OnApplicationExitRequested;
            _applicationContext.RequestApplicationExit();
            _applicationContext.ApplicationExitRequested -= OnApplicationExitRequested;

            Assert.True(handlerCalled);
        }

        public static bool CreateCalled { get; set; }

        [Test]
        public void LoadDocumentUsingFactoryTest()
        {
            _serviceProviderMock
                .Setup(sp => sp.GetService(It.IsAny<Type>()))
                .Returns(() => () => new ExampleDocument());

            Assert.AreEqual(0, _applicationContext.DocumentsToLoad.Count);
            _applicationContext.LoadDocumentUsingFactory<ExampleDocument>();
            Assert.AreEqual(1, _applicationContext.DocumentsToLoad.Count);

            Assert.True(_applicationContext.DocumentsToLoad.Peek() is ExampleDocument);
        }

        private class ExampleDocument : DocumentBase { }
    }
}
