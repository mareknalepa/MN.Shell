using MN.Shell.Framework.StatusBar;
using MN.Shell.MVVM;
using NUnit.Framework;

namespace MN.Shell.Tests.Framework.StatusBar
{
    [TestFixture]
    public class StatusBarItemDefinitionTests
    {
        [Test]
        public void SetPlacementTest()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");

            Assert.False(statusBarItem.IsRightSide);
            Assert.AreEqual(0, statusBarItem.Order);

            statusBarItem.SetSizeAndPlacement(150, true, 1);

            Assert.AreEqual(150, statusBarItem.MinWidth);
            Assert.True(statusBarItem.IsRightSide);
            Assert.AreEqual(1, statusBarItem.Order);
        }

        [Test]
        public void SetContentTest()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");

            Assert.True(string.IsNullOrEmpty(statusBarItem.Content));

            statusBarItem.SetContent("Content");

            Assert.AreEqual("Content", statusBarItem.Content);
        }

        [Test]
        public void SetCommandTest()
        {
            var statusBarItem = new StatusBarItemDefinition("Sample");
            var command = new Command(() => { });

            statusBarItem.SetCommand(command);

            Assert.AreSame(command, statusBarItem.Command);
        }
    }
}
