using MN.Shell.Framework.Menu;
using MN.Shell.MVVM;
using NUnit.Framework;
using System;

namespace MN.Shell.Tests.Framework.Menu
{
    [TestFixture]
    public class MenuItemDefinitionTests
    {
        [Test]
        public void SetPlacementTest()
        {
            var menuItem = new MenuItemDefinition("Sample");

            Assert.AreEqual(0, menuItem.Section);
            Assert.AreEqual(0, menuItem.Order);

            menuItem.SetPlacement(1, 2);

            Assert.AreEqual(1, menuItem.Section);
            Assert.AreEqual(2, menuItem.Order);
        }

        [Test]
        public void SetCommandTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var command = new Command(() => { });

            menuItem.SetCommand(command);

            Assert.AreSame(command, menuItem.Command);
        }

        [Test]
        public void SetCommandWhenContainsSubItemsTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var subItem = new MenuItemDefinition("Sample Sub Item");
            menuItem.SubItems.Add(subItem);

            Assert.Throws<InvalidOperationException>(() => menuItem.SetCommand(new Command(() => { })));
        }

        [Test]
        public void SetCommandWhenIsCheckboxTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCheckbox(false);

            Assert.Throws<InvalidOperationException>(() => menuItem.SetCommand(new Command(() => { })));
        }

        [Test]
        public void SetCheckboxTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCheckbox(false);

            Assert.True(menuItem.IsCheckbox);
        }

        [Test]
        public void SetCheckboxWhenContainsSubItemsTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var subItem = new MenuItemDefinition("Sample Sub Item");
            menuItem.SubItems.Add(subItem);

            Assert.Throws<InvalidOperationException>(() => menuItem.SetCheckbox(false));
        }

        [Test]
        public void SetCheckboxWhenIsCommandTest()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCommand(new Command(() => { }));

            Assert.Throws<InvalidOperationException>(() => menuItem.SetCheckbox(false));
        }
    }
}
