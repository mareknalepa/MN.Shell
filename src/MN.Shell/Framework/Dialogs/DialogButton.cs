using Caliburn.Micro;
using System.Windows.Input;

namespace MN.Shell.Framework.Dialogs
{
    public class DialogButton : PropertyChangedBase
    {
        private string _caption;

        public string Caption
        {
            get => _caption;
            set { _caption = value; NotifyOfPropertyChange(); }
        }

        private bool _isDefault;

        public bool IsDefault
        {
            get => _isDefault;
            set { _isDefault = value; NotifyOfPropertyChange(); }
        }

        private bool _isCancel;

        public bool IsCancel
        {
            get => _isCancel;
            set { _isCancel = value; NotifyOfPropertyChange(); }
        }

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            set { _command = value; NotifyOfPropertyChange(); }
        }

        public DialogButton(string caption, bool isDefault, bool isCancel, ICommand command)
        {
            Caption = caption;
            IsDefault = isDefault;
            IsCancel = isCancel;
            Command = command;
        }

        public static DialogButton Create(DialogButtonType type)
        {
            switch (type)
            {
                case DialogButtonType.Ok:
                    return new DialogButton("OK", true, false, null);
                case DialogButtonType.Cancel:
                    return new DialogButton("Cancel", false, true, null);
                case DialogButtonType.Yes:
                    return new DialogButton("Yes", false, false, null);
                case DialogButtonType.No:
                    return new DialogButton("No", false, false, null);
                default:
                    return new DialogButton(string.Empty, false, false, null);
            }
        }
    }
}
