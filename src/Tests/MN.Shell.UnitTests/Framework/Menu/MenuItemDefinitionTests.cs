using MN.Shell.Framework.Menu;
using MN.Shell.MVVM;

namespace MN.Shell.UnitTests.Framework.Menu
{
    public sealed class MenuItemDefinitionTests
    {
        [Fact]
        public void SetPlacement_SetsCorrectValues()
        {
            var menuItem = new MenuItemDefinition("Sample");

            menuItem.Section.ShouldBe(0);
            menuItem.Order.ShouldBe(0);

            menuItem.SetPlacement(1, 2);

            menuItem.Section.ShouldBe(1);
            menuItem.Order.ShouldBe(2);
        }

        [Fact]
        public void SetCommand_SetsCommand()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var command = new Command(() => { });

            menuItem.SetCommand(command);

            menuItem.Command.ShouldBeSameAs(command);
        }

        [Fact]
        public void SetCommand_Throws_WhenItemContainsSubItems()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var subItem = new MenuItemDefinition("Sample Sub Item");
            menuItem.SubItems.Add(subItem);

            var act = () => menuItem.SetCommand(new Command(() => { }));
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SetCommand_Throws_WhenItemIsCheckbox()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCheckbox(false);

            var act = () => menuItem.SetCommand(new Command(() => { }));
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SetCheckbox_SetsCorrectValues()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCheckbox(false);

            menuItem.IsCheckbox.ShouldBeTrue();
        }

        [Fact]
        public void SetCheckbox_Throws_WhenItemContainsSubItems()
        {
            var menuItem = new MenuItemDefinition("Sample");
            var subItem = new MenuItemDefinition("Sample Sub Item");
            menuItem.SubItems.Add(subItem);

            var act = () => menuItem.SetCheckbox(false);
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void SetCheckbox_Throws_WhenItemIsCommand()
        {
            var menuItem = new MenuItemDefinition("Sample");
            menuItem.SetCommand(new Command(() => { }));

            var act = () => menuItem.SetCheckbox(false);
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}
