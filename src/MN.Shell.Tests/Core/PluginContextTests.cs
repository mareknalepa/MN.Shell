﻿using MN.Shell.Core;
using MN.Shell.PluginContracts;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class PluginContextTests
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private IKernel _kernel;
        private PluginContext _context;

        [SetUp]
        public void SetUp()
        {
            _kernel = new StandardKernel();
            _context = new PluginContext(_kernel);
        }

        [TearDown]
        public void TearDown()
        {
            _kernel.Dispose();
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
                () => _context.UseDocumentFactory<IMockDocumentFactory, MockDocument>());
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

        private class MockTool : ToolBase { }

        private class MockDocument : DocumentBase { }

        private class IMockDocumentFactory : IDocumentFactory<MockDocument>
        {
            public MockDocument Create() => throw new NotImplementedException();
        }

        private class MockMenuProvider : IMenuProvider
        {
            public IEnumerable<IMenuItem> GetMenuItems() => throw new NotImplementedException();
        }

        private class MockStatusBarProvider : IStatusBarProvider
        {
            public IEnumerable<IStatusBarItem> GetStatusBarItems() => throw new NotImplementedException();
        }
    }
}
