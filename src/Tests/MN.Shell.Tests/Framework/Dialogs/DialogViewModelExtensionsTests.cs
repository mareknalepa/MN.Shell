using MN.Shell.Framework.Dialogs;
using MN.Shell.Tests.Mocks;

namespace MN.Shell.Tests.Framework.Dialogs
{
    public sealed class DialogViewModelExtensionsTests
    {
        [Theory]
        [InlineData(DialogButtonType.Ok)]
        [InlineData(DialogButtonType.Cancel)]
        [InlineData(DialogButtonType.Yes)]
        [InlineData(DialogButtonType.No)]
        [InlineData(DialogButtonType.Custom)]
        public void AddButton_AddsButtonCorrectly(DialogButtonType type)
        {
            var vm = new MockDialogViewModel();
            vm.AddButton(type);

            vm.Buttons.ShouldNotBeNull();

            var button = vm.Buttons.First();
            button.ShouldNotBeNull();
            button.Type.ShouldBe(type);

            vm.SelectedButton.ShouldBeNull();
            button.Command?.Execute(null);
            vm.SelectedButton.ShouldBe(button);
        }

        [Fact]
        public void AddCustomButton_AddsButtonCorrectly()
        {
            var vm = new MockDialogViewModel();

            bool handlerFired = false;
            vm.AddCustomButton("Caption 1", () => handlerFired = true);

            vm.Buttons.ShouldNotBeNull();

            var button = vm.Buttons.First();
            button.ShouldNotBeNull();
            button.Type.ShouldBe(DialogButtonType.Custom);
            button.Caption.ShouldBe("Caption 1");
            handlerFired.ShouldBeFalse();

            vm.SelectedButton.ShouldBeNull();
            button.Command?.Execute(null);
            vm.SelectedButton.ShouldBe(button);
            handlerFired.ShouldBeTrue();
        }
    }
}
