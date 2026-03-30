using MN.Shell.Framework.Dialogs;
using MN.Shell.Framework.MessageBox;
using MN.Shell.Modules.MessageBox;
using MN.Shell.MVVM;

namespace MN.Shell.Tests.Framework.MessageBox
{
    public sealed class MessageBoxManagerTests
    {
        private readonly IWindowManager _windowManager;
        private readonly MessageBoxManager _messageBoxManager;

        public MessageBoxManagerTests()
        {
            _windowManager = Substitute.For<IWindowManager>();
            _messageBoxManager = new(_windowManager);

            _windowManager.ShowDialog(Arg.Any<MessageBoxViewModel>()).Returns(true);
        }

        [Theory]
        [InlineData("", "", MessageBoxType.None)]
        [InlineData("Caption 1", "", MessageBoxType.None)]
        [InlineData("", "Message 1", MessageBoxType.Info)]
        [InlineData("Caption 2", "Message 2", MessageBoxType.Warning)]
        [InlineData("Caption 3", "Message 3", MessageBoxType.Error)]
        public void Show_CallsWindowsManager_WithCorrectCaptionMessageType(string caption, string message, MessageBoxType type)
        {
            _messageBoxManager.Show(caption, message, type);
            _windowManager.Received(1).ShowDialog(Arg.Is<MessageBoxViewModel>(
                vm => vm.Title == caption && vm.Message == message && vm.Type == type));
        }

        [Theory]
        [InlineData(MessageBoxButtonSet.Ok)]
        [InlineData(MessageBoxButtonSet.OkCancel)]
        [InlineData(MessageBoxButtonSet.YesNo)]
        [InlineData(MessageBoxButtonSet.YesNoCancel)]
        public void Show_AddsCorrectButtons(MessageBoxButtonSet buttons)
        {
            _messageBoxManager.Show("Caption", "Message", MessageBoxType.None, buttons);
            _windowManager.Received(1).ShowDialog(Arg.Is<MessageBoxViewModel>(vm => IsViewModelCorrect(vm, buttons)));
        }

        private static bool IsViewModelCorrect(MessageBoxViewModel vm, MessageBoxButtonSet buttons)
            => buttons switch
            {
                MessageBoxButtonSet.Ok => vm.Buttons.Count == 1
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Ok),
                MessageBoxButtonSet.OkCancel => vm.Buttons.Count == 2
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Ok)
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Cancel),
                MessageBoxButtonSet.YesNo => vm.Buttons.Count == 2
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Yes)
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.No),
                MessageBoxButtonSet.YesNoCancel => vm.Buttons.Count == 3
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Yes)
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.No)
                    && vm.Buttons.Any(b => b.Type == DialogButtonType.Cancel),
                _ => throw new ArgumentException("Buttons argument out of scope")
            };
    }
}
