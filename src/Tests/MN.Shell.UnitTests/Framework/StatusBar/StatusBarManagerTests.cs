using MN.Shell.Framework.StatusBar;

namespace MN.Shell.UnitTests.Framework.StatusBar
{
    public sealed class StatusBarManagerTests
    {
        [Fact]
        public void AddItem_AddsStatusBarItem()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.StatusBarItemDefinitions.ShouldNotBeNull();
            statusBarManager.StatusBarItemDefinitions.ShouldBeEmpty();

            statusBarManager.AddItem("Sample");

            statusBarManager.StatusBarItemDefinitions.Count.ShouldBe(1);
            statusBarManager.StatusBarItemDefinitions.ShouldContain(d => d.Name == "Sample");
        }

        [Fact]
        public void RemoveItem_RemovesItem()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("Sample");

            statusBarManager.StatusBarItemDefinitions.Count.ShouldBe(1);

            statusBarManager.RemoveItem("Sample");

            statusBarManager.StatusBarItemDefinitions.Count.ShouldBe(0);
        }

        [Fact]
        public void RemoveItem_DoesNothing_ForNotExistingItem()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("Sample");

            statusBarManager.StatusBarItemDefinitions.Count.ShouldBe(1);

            statusBarManager.RemoveItem("Not existing");

            statusBarManager.StatusBarItemDefinitions.Count.ShouldBe(1);
        }

        [Fact]
        public void CompileStatusBar_CreatesStatusBarCorrectly()
        {
            var statusBarManager = new StatusBarManagerWrapper();

            statusBarManager.AddItem("repo").SetSizeAndPlacement(100, true, 50).SetContent("Repository");
            statusBarManager.AddItem("tasks").SetSizeAndPlacement(100, false, 30).SetContent("Background tasks in progress...");
            statusBarManager.AddItem("status").SetSizeAndPlacement(100, false, 10).SetContent("Status");
            statusBarManager.AddItem("user").SetSizeAndPlacement(100, true, 80).SetContent("User");
            statusBarManager.AddItem("charset").SetSizeAndPlacement(100, true, 20).SetContent("Charset");

            statusBarManager.CompileStatusBar();

            statusBarManager.StatusBarItems.Count.ShouldBe(5);

            statusBarManager.StatusBarItems[0].Content.ShouldBe("Status");
            statusBarManager.StatusBarItems[0].IsRightSide.ShouldBeFalse();

            statusBarManager.StatusBarItems[1].Content.ShouldBe("Background tasks in progress...");
            statusBarManager.StatusBarItems[1].IsRightSide.ShouldBeFalse();

            statusBarManager.StatusBarItems[2].Content.ShouldBe("Charset");
            statusBarManager.StatusBarItems[2].IsRightSide.ShouldBeTrue();

            statusBarManager.StatusBarItems[3].Content.ShouldBe("Repository");
            statusBarManager.StatusBarItems[3].IsRightSide.ShouldBeTrue();

            statusBarManager.StatusBarItems[4].Content.ShouldBe("User");
            statusBarManager.StatusBarItems[4].IsRightSide.ShouldBeTrue();
        }

        private class StatusBarManagerWrapper : StatusBarManager
        {
            public StatusBarManagerWrapper() : base([]) { }

            public new List<StatusBarItemDefinition> StatusBarItemDefinitions => base.StatusBarItemDefinitions;
        }
    }
}
