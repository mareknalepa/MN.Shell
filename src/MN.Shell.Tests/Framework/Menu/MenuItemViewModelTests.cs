using MN.Shell.Framework.Menu;
using NUnit.Framework;

namespace MN.Shell.Tests.Framework.Menu
{
    [TestFixture]
    public class MenuItemViewModelTests
    {
        [Test]
        public void MenuItemViewModelCheckedChangedTest()
        {
            MenuItemViewModel vm = new MenuItemViewModel();

            vm.IsChecked = true;
            vm.IsChecked = false;

            bool isChecked = false;
            vm.OnIsCheckedChanged = value => isChecked = value;

            Assert.False(vm.IsChecked);
            Assert.False(isChecked);

            vm.IsChecked = true;
            Assert.True(vm.IsChecked);
            Assert.True(isChecked);

            vm.IsChecked = false;
            Assert.False(vm.IsChecked);
            Assert.False(isChecked);

            bool handlerFired = false;
            vm.OnIsCheckedChanged = value => handlerFired = true;

            vm.IsChecked = false;
            Assert.False(handlerFired);

            vm.IsChecked = true;
            Assert.True(handlerFired);
        }
    }
}
