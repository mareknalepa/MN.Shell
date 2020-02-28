using Caliburn.Micro;
using MN.Shell.Framework;
using MN.Shell.Framework.Dialogs;
using MN.Shell.Framework.MessageBox;
using System.Collections.ObjectModel;

namespace MN.Shell.Modules.MessageBox
{
    public class MessageBoxViewModel : Screen, IDialog
    {
        private string _message;

        public string Message
        {
            get => _message;
            set { _message = value; NotifyOfPropertyChange(); }
        }

        private MessageBoxType _type;

        public MessageBoxType Type
        {
            get => _type;
            set { _type = value; NotifyOfPropertyChange(); }
        }

        public ObservableCollection<DialogButton> Buttons { get; } = new ObservableCollection<DialogButton>();

        public DialogButton SelectedButton { get; set; }

        public MessageBoxViewModel(string title, string message, MessageBoxType type)
        {
            DisplayName = title;
            Message = message;
            Type = type;
        }
    }
}
