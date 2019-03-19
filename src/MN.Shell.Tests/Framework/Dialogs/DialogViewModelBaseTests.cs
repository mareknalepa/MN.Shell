using MN.Shell.Framework.Dialogs;
using MN.Shell.Tests.Mocks;
using NUnit.Framework;
using System.Linq;

namespace MN.Shell.Tests.Framework.Dialogs
{
    [TestFixture]
    public class DialogViewModelBaseTests
    {
        [Test]
        public void DialogViewModelBaseCreateButtonsTest()
        {
            MockDialogViewModel vm = new MockDialogViewModel();
            vm.Buttons.IsNotifying = false;
            Assert.IsEmpty(vm.Buttons);

            vm.CreateButtons(new[] { DialogButtonType.Ok });
            Assert.AreEqual(1, vm.Buttons.Count);
            Assert.AreEqual("OK", vm.Buttons.First().Caption);
            Assert.True(vm.Buttons.First().IsDefault);
            Assert.False(vm.Buttons.First().IsCancel);

            vm.Buttons.Clear();
            vm.CreateButtons(new[] { DialogButtonType.Cancel });
            Assert.AreEqual(1, vm.Buttons.Count);
            Assert.AreEqual("Cancel", vm.Buttons.First().Caption);
            Assert.False(vm.Buttons.First().IsDefault);
            Assert.True(vm.Buttons.First().IsCancel);

            vm.Buttons.Clear();
            vm.CreateButtons(new[] { DialogButtonType.Yes });
            Assert.AreEqual(1, vm.Buttons.Count);
            Assert.AreEqual("Yes", vm.Buttons.First().Caption);
            Assert.False(vm.Buttons.First().IsDefault);
            Assert.False(vm.Buttons.First().IsCancel);

            vm.Buttons.Clear();
            vm.CreateButtons(new[] { DialogButtonType.No });
            Assert.AreEqual(1, vm.Buttons.Count);
            Assert.AreEqual("No", vm.Buttons.First().Caption);
            Assert.False(vm.Buttons.First().IsDefault);
            Assert.False(vm.Buttons.First().IsCancel);
        }
    }
}
