using MN.Shell.Core;
using MN.Shell.PluginContracts;

namespace MN.Shell.Tests.Core
{
    public sealed class ApplicationContextTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationContext _applicationContext;

        public ApplicationContextTests()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            _applicationContext = new(_serviceProvider);
        }

        [Fact]
        public void ApplicationTitle_RaisesEvent()
        {
            bool handlerCalled = false;

            void OnApplicationTitleChanged(object? sender, string newTitle)
            {
                handlerCalled = true;
                newTitle.ShouldBe("New Title");
            }

            _applicationContext.ApplicationTitleChanged += OnApplicationTitleChanged;
            _applicationContext.ApplicationTitle = "New Title";
            _applicationContext.ApplicationTitleChanged -= OnApplicationTitleChanged;

            handlerCalled.ShouldBeTrue();
        }

        [Fact]
        public void RequestApplicationExit_RaisesEvent()
        {
            bool handlerCalled = false;

            void OnApplicationExitRequested(object? sender, EventArgs e) => handlerCalled = true;

            _applicationContext.ApplicationExitRequested += OnApplicationExitRequested;
            _applicationContext.RequestApplicationExit();
            _applicationContext.ApplicationExitRequested -= OnApplicationExitRequested;

            handlerCalled.ShouldBeTrue();
        }

        [Fact]
        public void LoadDocumentUsingFactory_UsesServiceProvider()
        {
            _serviceProvider.GetService(typeof(Func<ExampleDocument>)).Returns(new Func<ExampleDocument>(() => new ExampleDocument()));

            _applicationContext.DocumentsToLoad.Count.ShouldBe(0);
            _applicationContext.LoadDocumentUsingFactory<ExampleDocument>();
            _applicationContext.DocumentsToLoad.Count.ShouldBe(1);

            _applicationContext.DocumentsToLoad.ShouldHaveSingleItem();
            _applicationContext.DocumentsToLoad.First().ShouldBeOfType<ExampleDocument>();

            _serviceProvider.Received(1).GetService(typeof(Func<ExampleDocument>));
        }

        private class ExampleDocument : DocumentBase { }
    }
}
