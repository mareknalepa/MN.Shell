using MN.Shell.Framework.Menu;
using MN.Shell.MVVM;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MN.Shell.Tests.Framework.Menu
{
    [TestFixture]
    public class MenuAggregatorTests
    {
        [Test]
        public void MenuAggregatorComposeMenuBasicTest()
        {
            Mock<IMenuProvider> menuProvider1 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider1.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "File", GroupOrder = 10 },
                new MenuItem() { Name = "Edit", GroupOrder = 20 },
                new MenuItem() { Name = "Open...", Path = new []{ "File"} },
                new MenuItem() { Name = "About...", Path = new []{ "Help" } },
            });

            Mock<IMenuProvider> menuProvider2 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider2.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Save As...", Path = new []{ "File"} },
                new MenuItem() { Name = "Help", GroupOrder = 30 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();
            var menuVm = menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider1.Object,
                menuProvider2.Object,
            });

            Assert.NotNull(menuVm);
            Assert.AreEqual(3, menuVm.Count());

            Assert.AreEqual("File", menuVm.First().Name);
            Assert.AreEqual("Edit", menuVm.Skip(1).First().Name);
            Assert.AreEqual("Help", menuVm.Skip(2).First().Name);

            Assert.AreEqual("Open...", menuVm.First().SubItems.First().Name);
            Assert.AreEqual("Save As...", menuVm.First().SubItems.Skip(1).First().Name);
            Assert.AreEqual("About...", menuVm.Skip(2).First().SubItems.First().Name);
        }

        [Test]
        public void MenuAggregatorComposeMenuGroupIdTest()
        {
            Mock<IMenuProvider> menuProvider1 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider1.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item1", GroupOrder = 10 },
                new MenuItem() { Name = "Item2", GroupOrder = 20 },
                new MenuItem() { Name = "Item3", GroupOrder = 30 },
            });

            Mock<IMenuProvider> menuProvider2 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider2.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "SubItem12", Path = new[]{ "Item1" }, GroupId = 20 },
                new MenuItem() { Name = "SubItem11", Path = new[]{ "Item1" }, GroupId = 10 },
                new MenuItem() { Name = "SubItem31", Path = new[]{ "Item3" }, GroupId = 10 },
                new MenuItem() { Name = "SubItem32", Path = new[]{ "Item3" }, GroupId = 10 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();
            var menuVm = menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider1.Object,
                menuProvider2.Object,
            });

            Assert.NotNull(menuVm);
            Assert.AreEqual(3, menuVm.Count());

            Assert.AreEqual("Item1", menuVm.First().Name);
            Assert.AreEqual("Item2", menuVm.Skip(1).First().Name);
            Assert.AreEqual("Item3", menuVm.Skip(2).First().Name);

            Assert.AreEqual("SubItem11", menuVm.First().SubItems.First().Name);
            Assert.True(menuVm.First().SubItems.Skip(1).First().IsSeparator);
            Assert.AreEqual("SubItem12", menuVm.First().SubItems.Skip(2).First().Name);

            Assert.AreEqual("SubItem31", menuVm.Skip(2).First().SubItems.First().Name);
            Assert.AreEqual("SubItem32", menuVm.Skip(2).First().SubItems.Skip(1).First().Name);
        }

        [Test]
        public void MenuAggregatorComposeMenuGroupOrderTest()
        {
            Mock<IMenuProvider> menuProvider1 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider1.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item2", GroupOrder = 20 },
                new MenuItem() { Name = "Item1", GroupOrder = 10 },
                new MenuItem() { Name = "Item4", GroupOrder = 40 },
            });

            Mock<IMenuProvider> menuProvider2 = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider2.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item3", GroupOrder = 30 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();
            var menuVm = menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider1.Object,
                menuProvider2.Object,
            });

            Assert.NotNull(menuVm);
            Assert.AreEqual(4, menuVm.Count());

            Assert.AreEqual("Item1", menuVm.First().Name);
            Assert.AreEqual("Item2", menuVm.Skip(1).First().Name);
            Assert.AreEqual("Item3", menuVm.Skip(2).First().Name);
            Assert.AreEqual("Item4", menuVm.Skip(3).First().Name);
        }

        [Test]
        public void MenuAggregatorComposeMenuItemWithoutParentTest()
        {
            Mock<IMenuProvider> menuProvider = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item1", GroupOrder = 10 },
                new MenuItem() { Name = "SubItem11", Path = new []{ "Item1" }, GroupOrder = 10 },
                new MenuItem() { Name = "SubItem21", Path = new []{ "Item2" }, GroupOrder = 10 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();

            Assert.Throws<InconsistentMenuException>(() => menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider.Object,
            }));
        }

        [Test]
        public void MenuAggregatorComposeMenuItemWithCommandAndCheckableTest()
        {
            Mock<IMenuProvider> menuProvider = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem()
                {
                    Name = "Item1",
                    GroupOrder = 10,
                    Command = new RelayCommand(() => { }),
                    IsCheckable = true,
                },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();

            Assert.Throws<InconsistentMenuException>(() => menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider.Object,
            }));
        }

        [Test]
        public void MenuAggregatorComposeMenuItemWithSubItemsAndCommandTest()
        {
            Mock<IMenuProvider> menuProvider = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item1", GroupOrder = 10, Command = new RelayCommand(() => { }) },
                new MenuItem() { Name = "SubItem11", Path = new []{ "Item1" }, GroupOrder = 10 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();

            Assert.Throws<InconsistentMenuException>(() => menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider.Object,
            }));
        }

        [Test]
        public void MenuAggregatorComposeMenuItemWithSubItemsAndCheckableTest()
        {
            Mock<IMenuProvider> menuProvider = new Mock<IMenuProvider>(MockBehavior.Strict);
            menuProvider.Setup(m => m.GetMenuItems()).Returns(new List<MenuItem>()
            {
                new MenuItem() { Name = "Item1", GroupOrder = 10, IsCheckable = true },
                new MenuItem() { Name = "SubItem11", Path = new []{ "Item1" }, GroupOrder = 10 },
            });

            IMenuAggregator menuAggregator = new MenuAggregator();

            Assert.Throws<InconsistentMenuException>(() => menuAggregator.ComposeMenu(new List<IMenuProvider>()
            {
                menuProvider.Object,
            }));
        }
    }
}
