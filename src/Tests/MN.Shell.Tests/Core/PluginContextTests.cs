using Microsoft.Extensions.DependencyInjection;
using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Moq;
using NUnit.Framework;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class PluginContextTests
    {
        private PluginContext _context = new PluginContext(new Mock<IServiceCollection>().Object);

        [SetUp]
        public void SetUp()
        {
            _context = new PluginContext(new Mock<IServiceCollection>().Object);
        }

        [Test]
        public void UseToolOutOfScopeTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => _context.UseTool<MockTool>());
        }

        [Test]
        public void UseDocumentFactoryOutOfScopeTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => _context.UseDocumentFactory<MockDocument>());
        }

        [Test]
        public void UseMenuProviderOutOfScopeTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => _context.UseMenuProvider<MockMenuProvider>());
        }

        [Test]
        public void UseStatusBarProviderOutOfScopeTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => _context.UseStatusBarProvider<MockStatusBarProvider>());
        }

        [Test]
        public void UseServiceOutOfScopeTest()
        {
            Assert.Throws<InvalidOperationException>(
                () => _context.UseService<IService, Service>());
        }

#pragma warning disable CA1812
        private class MockTool : ToolBase { }

        private class MockDocument : DocumentBase { }

        private class MockMenuProvider : IMenuProvider
        {
            public void BuildMenu(IMenuBuilder builder) { }
        }

        private class MockStatusBarProvider : IStatusBarProvider
        {
            public void BuildStatusBar(IStatusBarBuilder builder) { }
        }

        private interface IService { }

        private class Service : IService { }
#pragma warning restore CA1812

    }
}
