using MN.Shell.Framework.StatusBar;
using MN.Shell.MVVM;

namespace MN.Shell.UnitTests.Framework.StatusBar
{
    public sealed class StatusBarItemDefinitionTests
    {
        [Fact]
        public void SetPlacement_SetsCorrectValues()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");

            statusBarItem.IsRightSide.ShouldBeFalse();
            statusBarItem.Order.ShouldBe(0);

            statusBarItem.SetSizeAndPlacement(150, true, 1);

            statusBarItem.MinWidth.ShouldBe(150);
            statusBarItem.IsRightSide.ShouldBeTrue();
            statusBarItem.Order.ShouldBe(1);
        }

        [Fact]
        public void SetContent_SetsContent()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");

            statusBarItem.Content.ShouldBeNullOrEmpty();

            statusBarItem.SetContent("Content");

            statusBarItem.Content.ShouldBe("Content");
        }

        [Fact]
        public void SetCommand_SetsCommand()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");
            var command = new Command(() => { });

            statusBarItem.SetCommand(command);

            statusBarItem.Command.ShouldBeSameAs(command);
        }
    }
}
