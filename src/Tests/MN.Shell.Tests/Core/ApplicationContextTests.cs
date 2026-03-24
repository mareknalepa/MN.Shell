using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using Ninject;
using Ninject.Activation;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class ApplicationContextTests
    {
        private Mock<IKernel> _kernelMock;
        private ApplicationContext _applicationContext;

        [SetUp]
        public void SetUp()
        {
            _kernelMock = new Mock<IKernel>();
            _applicationContext = new ApplicationContext(_kernelMock.Object);
        }

        [Test]
        public void ApplicationTitleSetterRaisesEventTest()
        {
            bool handlerCalled = false;

            void OnApplicationTitleChanged(object sender, string newTitle)
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

            void OnApplicationExitRequested(object sender, EventArgs e) => handlerCalled = true;

            _applicationContext.ApplicationExitRequested += OnApplicationExitRequested;
            _applicationContext.RequestApplicationExit();
            _applicationContext.ApplicationExitRequested -= OnApplicationExitRequested;

            Assert.True(handlerCalled);
        }

        public static bool CreateCalled { get; set; }

        [Test]
        public void LoadDocumentUsingFactoryTest()
        {
            _kernelMock
                .Setup(k => k.Resolve(It.IsAny<IRequest>()))
                .Returns(() => new List<object>() { new ExampleDocumentFactory() });

            Assert.AreEqual(0, _applicationContext.DocumentsToLoad.Count);
            _applicationContext.LoadDocumentUsingFactory<IExampleDocumentFactory, ExampleDocument>();
            Assert.AreEqual(1, _applicationContext.DocumentsToLoad.Count);

            Assert.True(_applicationContext.DocumentsToLoad.Peek() is ExampleDocument);
        }

        private class ExampleDocument : DocumentBase { }

        private interface IExampleDocumentFactory : IDocumentFactory<ExampleDocument> { }

        private class ExampleDocumentFactory : IExampleDocumentFactory
        {
            public ExampleDocument Create()
            {
                CreateCalled = true;
                return new ExampleDocument();
            }
        }
    }
}
