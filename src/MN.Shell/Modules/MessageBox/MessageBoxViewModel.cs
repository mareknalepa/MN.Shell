using Caliburn.Micro;
using MN.Shell.Framework;
using MN.Shell.Framework.Dialogs;
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

        public ObservableCollection<DialogButton> Buttons { get; } = new ObservableCollection<DialogButton>();

        public DialogButton SelectedButton { get; set; }

        public MessageBoxViewModel(string title, string message)
        {
            DisplayName = title;
            Message = message;
        }
    }
}
