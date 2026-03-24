using MN.Shell.Framework.Dialogs;
using MN.Shell.Tests.Mocks;
using NUnit.Framework;
using System.Linq;

namespace MN.Shell.Tests.Framework.Dialogs
{
    [TestFixture]
    public class DialogViewModelExtensionsTests
    {
        [Test]
        public void DialogViewModelExtensionAddButtonTest([Values(
            DialogButtonType.Ok,
            DialogButtonType.Cancel,
            DialogButtonType.Yes,
            DialogButtonType.No,
            DialogButtonType.Custom)] DialogButtonType type)
        {
            var vm = new MockDialogViewModel();
            vm.AddButton(type);

            Assert.NotNull(vm.Buttons);

            var button = vm.Buttons.First();
            Assert.NotNull(button);
            Assert.AreEqual(type, button.Type);

            Assert.Null(vm.SelectedButton);
            button.Command.Execute(null);
            Assert.AreEqual(button, vm.SelectedButton);
        }

        [Test]
        public void DialogViewModelExtensionAddCustomButtonTest()
        {
            var vm = new MockDialogViewModel();

            bool handlerFired = false;
            vm.AddCustomButton("Caption 1", () => handlerFired = true);

            Assert.NotNull(vm.Buttons);

            var button = vm.Buttons.First();
            Assert.NotNull(button);
            Assert.AreEqual(DialogButtonType.Custom, button.Type);
            Assert.AreEqual("Caption 1", button.Caption);
            Assert.False(handlerFired);

            Assert.Null(vm.SelectedButton);
            button.Command.Execute(null);
            Assert.AreEqual(button, vm.SelectedButton);
            Assert.True(handlerFired);
        }
    }
}
