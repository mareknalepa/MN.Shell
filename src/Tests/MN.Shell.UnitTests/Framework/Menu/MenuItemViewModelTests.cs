using MN.Shell.Framework.Menu;

namespace MN.Shell.UnitTests.Framework.Menu
{
    public sealed class MenuItemViewModelTests
    {
        [Fact]
        public void IsChecked_RaisesEvent()
        {
            MenuItemViewModel vm = new()
            {
                IsChecked = true
            };
            vm.IsChecked = false;

            bool isChecked = false;
            vm.OnIsCheckedChanged = value => isChecked = value;

            vm.IsChecked.ShouldBeFalse();
            isChecked.ShouldBeFalse();

            vm.IsChecked = true;
            vm.IsChecked.ShouldBeTrue();
            isChecked.ShouldBeTrue();

            vm.IsChecked = false;
            vm.IsChecked.ShouldBeFalse();
            isChecked.ShouldBeFalse();

            bool handlerFired = false;
            vm.OnIsCheckedChanged = value => handlerFired = true;

            vm.IsChecked = false;
            handlerFired.ShouldBeFalse();

            vm.IsChecked = true;
            handlerFired.ShouldBeTrue();
        }
    }
}
