using Moq;
using NUnit.Framework;
using System;

namespace MN.Shell.PluginContracts.Tests
{
    [TestFixture]
    public class PluginBaseTests
    {
        [Test]
        public void ContextInjectedTest()
        {
            var pluginLoaderContextMock = new Mock<IPluginLoaderContext>();

            var plugin = new ExamplePlugin();
            plugin.Load(pluginLoaderContextMock.Object);

            Assert.AreSame(pluginLoaderContextMock.Object, plugin.Context);
        }

        [Test]
        public void OnLoadCalledTest()
        {
            var pluginLoaderContextMock = new Mock<IPluginLoaderContext>();

            var plugin = new ExamplePlugin();

            bool onLoadCalled = false;
            plugin.OnLoadCalled += (sender, e) => onLoadCalled = true;

            plugin.Load(pluginLoaderContextMock.Object);

            Assert.True(onLoadCalled);
        }

        private class ExamplePlugin : PluginBase
        {
            public event EventHandler OnLoadCalled;
            protected override void OnLoad() => OnLoadCalled?.Invoke(this, EventArgs.Empty);
        }
    }
}
