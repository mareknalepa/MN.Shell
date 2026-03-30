using Microsoft.Extensions.DependencyInjection;
using MN.Shell.Core;
using MN.Shell.PluginContracts;

namespace MN.Shell.Tests.Core
{
    public sealed class PluginContextTests
    {
        private readonly PluginContext _context = new(Substitute.For<IServiceCollection>());

        [Fact]
        public void UseTool_Throws_WhenOutOfScope()
        {
            var act = _context.UseTool<MockTool>;
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void UseDocumentFactory_Throws_WhenOutOfScope()
        {
            var act = _context.UseDocumentFactory<MockDocument>;
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void UseMenuProvider_Throws_WhenOutOfScope()
        {
            var act = _context.UseMenuProvider<MockMenuProvider>;
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void UseStatusBarProvider_Throws_WhenOutOfScope()
        {
            var act = _context.UseStatusBarProvider<MockStatusBarProvider>;
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void UseService_Throws_WhenOutOfScope()
        {
            var act = _context.UseService<IService, Service>;
            act.ShouldThrow<InvalidOperationException>();
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
