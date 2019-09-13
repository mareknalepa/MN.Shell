using MN.Shell.Framework.StatusBar;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Tests.Framework.StatusBar
{
    [TestFixture]
    public class StatusBarAggregatorTests
    {
        [Test]
        public void StatusBarAggregatorComposeStatusBarBasicTest()
        {
            Mock<IStatusBarProvider> statusBarProvider1 = new Mock<IStatusBarProvider>(MockBehavior.Strict);
            statusBarProvider1.Setup(s => s.GetStatusBarItems()).Returns(new List<StatusBarItemViewModel>()
            {
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Left,
                    Priority = 30,
                    Content = "LeftItem2",
                },
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Right,
                    Priority = 20,
                    Content = "RightItem1",
                },
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Left,
                    Priority = 70,
                    Content = "LeftItem1",
                },
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Right,
                    Priority = 40,
                    Content = "RightItem3",
                },
            });

            Mock<IStatusBarProvider> statusBarProvider2 = new Mock<IStatusBarProvider>(MockBehavior.Strict);
            statusBarProvider2.Setup(s => s.GetStatusBarItems()).Returns(new List<StatusBarItemViewModel>()
            {
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Right,
                    Priority = 30,
                    Content = "RightItem2",
                },
                new StatusBarItemViewModel()
                {
                    Side = StatusBarSide.Left,
                    Priority = 10,
                    Content = "LeftItem3",
                },
            });

            IStatusBarAggregator statusBarAggregator = new StatusBarAggregator();
            var statusBarItems = statusBarAggregator.ComposeStatusBar(new List<IStatusBarProvider>()
            {
                statusBarProvider1.Object,
                statusBarProvider2.Object,
            });

            Assert.NotNull(statusBarItems);
            Assert.AreEqual(6, statusBarItems.Count());

            Assert.AreEqual("LeftItem1", statusBarItems.First().Content);
            Assert.AreEqual("LeftItem2", statusBarItems.Skip(1).First().Content);
            Assert.AreEqual("LeftItem3", statusBarItems.Skip(2).First().Content);

            Assert.AreEqual("RightItem3", statusBarItems.Skip(3).First().Content);
            Assert.AreEqual("RightItem2", statusBarItems.Skip(4).First().Content);
            Assert.AreEqual("RightItem1", statusBarItems.Skip(5).First().Content);
        }
    }
}
