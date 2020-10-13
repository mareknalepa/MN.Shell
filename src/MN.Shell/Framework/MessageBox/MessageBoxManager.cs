using MN.Shell.Framework.Dialogs;
using MN.Shell.Modules.MessageBox;
using MN.Shell.MVVM;
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
            MessageBoxButtonSet buttons = MessageBoxButtonSet.Ok)
        {
            var vm = new MessageBoxViewModel(title, msg, type);

            switch (buttons)
            {
                case MessageBoxButtonSet.Ok:
                    vm.AddButton(DialogButtonType.Ok);
                    break;
                case MessageBoxButtonSet.OkCancel:
                    vm.AddButton(DialogButtonType.Ok);
                    vm.AddButton(DialogButtonType.Cancel);
                    break;
                case MessageBoxButtonSet.YesNo:
                    vm.AddButton(DialogButtonType.Yes);
                    vm.AddButton(DialogButtonType.No);
                    break;
                case MessageBoxButtonSet.YesNoCancel:
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
