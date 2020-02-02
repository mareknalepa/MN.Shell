using Caliburn.Micro;
using MN.Shell.Framework.Dialogs;
using MN.Shell.Modules.MessageBox;
using System;

namespace MN.Shell.Framework.MessageBox
{
    public class MessageBoxManager : IMessageBoxManager
    {
        private readonly IWindowManager _windowManager;

        public MessageBoxManager(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public bool? Show(string title, string msg, MessageBoxType type = MessageBoxType.None,
            MessageBoxButtons buttons = MessageBoxButtons.Ok)
        {
            var vm = new MessageBoxViewModel(title, msg, type);

            switch (buttons)
            {
                case MessageBoxButtons.Ok:
                    vm.AddButton(DialogButtonType.Ok);
                    break;
                case MessageBoxButtons.OkCancel:
                    vm.AddButton(DialogButtonType.Ok);
                    vm.AddButton(DialogButtonType.Cancel);
                    break;
                case MessageBoxButtons.YesNo:
                    vm.AddButton(DialogButtonType.Yes);
                    vm.AddButton(DialogButtonType.No);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    vm.AddButton(DialogButtonType.Yes);
                    vm.AddButton(DialogButtonType.No);
                    vm.AddButton(DialogButtonType.Cancel);
                    break;
                default:
                    throw new ArgumentException("Unsupported buttons specified for Message Box");
            }

            return _windowManager.ShowDialog(vm);
        }
    }
}
