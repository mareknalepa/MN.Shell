using MN.Shell.Framework;
using MN.Shell.Framework.Dialogs;
using MN.Shell.Framework.MessageBox;
using MN.Shell.MVVM;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.MessageBox
{
    public class MessageBoxViewModel : Screen, IDialog
    {
        private string _message;

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        private MessageBoxType _type;

        public MessageBoxType Type
        {
            get => _type;
            set => Set(ref _type, value);
        }

        public ObservableCollection<DialogButton> Buttons { get; } = new ObservableCollection<DialogButton>();

        public DialogButton SelectedButton { get; set; }

        public MessageBoxViewModel(string title, string message, MessageBoxType type)
        {
            Title = title;
            Message = message;
            Type = type;
        }
    }
}
