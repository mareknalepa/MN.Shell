using MN.Shell.Framework.StatusBar;
using MN.Shell.PluginContracts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Tests.Framework.StatusBar
{
    [TestFixture]
    public class StatusBarManagerTests
    {
        [Test]
        public void AddItemTest()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            Assert.NotNull(statusBarManager.StatusBarItemDefinitions);
            Assert.IsEmpty(statusBarManager.StatusBarItemDefinitions);

            statusBarManager.AddItem("Sample");

            Assert.AreEqual(1, statusBarManager.StatusBarItemDefinitions.Count);
            Assert.True(statusBarManager.StatusBarItemDefinitions.Any(d => d.Name == "Sample"));
        }

        [Test]
        public void RemoveItemBasicTest()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("Sample");

            Assert.AreEqual(1, statusBarManager.StatusBarItemDefinitions.Count);

            statusBarManager.RemoveItem("Sample");

            Assert.AreEqual(0, statusBarManager.StatusBarItemDefinitions.Count);
        }

        [Test]
        public void RemoveItemNotExistingTest()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("Sample");

            Assert.AreEqual(1, statusBarManager.StatusBarItemDefinitions.Count);

            statusBarManager.RemoveItem("Not existing");

            Assert.AreEqual(1, statusBarManager.StatusBarItemDefinitions.Count);
        }

        [Test]
        public void CompileStatusBarSimpleTest()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("repo").SetSizeAndPlacement(100, true, 50).SetContent("Repository");
            statusBarManager.AddItem("tasks").SetSizeAndPlacement(100, false, 30).SetContent("Background tasks in progress...");
            statusBarManager.AddItem("status").SetSizeAndPlacement(100, false, 10).SetContent("Status");
            statusBarManager.AddItem("user").SetSizeAndPlacement(100, true, 80).SetContent("User");
            statusBarManager.AddItem("charset").SetSizeAndPlacement(100, true, 20).SetContent("Charset");

            statusBarManager.CompileStatusBar();

            Assert.AreEqual(5, statusBarManager.StatusBarItems.Count);

            Assert.AreEqual("Status", statusBarManager.StatusBarItems[0].Content);
            Assert.False(statusBarManager.StatusBarItems[0].IsRightSide);

            Assert.AreEqual("Background tasks in progress...", statusBarManager.StatusBarItems[1].Content);
            Assert.False(statusBarManager.StatusBarItems[1].IsRightSide);

            Assert.AreEqual("Charset", statusBarManager.StatusBarItems[2].Content);
            Assert.True(statusBarManager.StatusBarItems[2].IsRightSide);

            Assert.AreEqual("Repository", statusBarManager.StatusBarItems[3].Content);
            Assert.True(statusBarManager.StatusBarItems[3].IsRightSide);

            Assert.AreEqual("User", statusBarManager.StatusBarItems[4].Content);
            Assert.True(statusBarManager.StatusBarItems[4].IsRightSide);
        }

        private class StatusBarManagerWrapper : StatusBarManager
        {
            public StatusBarManagerWrapper() : base(Enumerable.Empty<IStatusBarProvider>()) { }

            public new List<StatusBarItemDefinition> StatusBarItemDefinitions => base.StatusBarItemDefinitions;
        }
    }
}
