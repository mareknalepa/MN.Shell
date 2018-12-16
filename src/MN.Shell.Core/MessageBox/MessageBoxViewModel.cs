using MN.Shell.Core.Dialogs;
using System.Collections.Generic;

namespace MN.Shell.Core.MessageBox
{
    public class MessageBoxViewModel : DialogViewModelBase
    {
        private string _message;

        public string Message
        {
            get => _message;
            set { _message = value; NotifyOfPropertyChange(); }
        }

        public MessageBoxViewModel(string title, string message, IEnumerable<DialogButtonType> buttonTypes)
        {
            DisplayName = title;
            Message = message;
            CreateButtons(buttonTypes);
        }
    }
}
