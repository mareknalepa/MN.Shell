using Caliburn.Micro;
using System;
using System.Windows.Input;

namespace MN.Shell.Framework.Dialogs
{
    public class DialogButton : PropertyChangedBase
    {
        public DialogButtonType Type { get; }

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

        protected DialogButton(DialogButtonType type, string caption, bool isDefault, bool isCancel, ICommand command)
        {
            Type = type;
            Caption = caption;
            IsDefault = isDefault;
            IsCancel = isCancel;
            Command = command;
        }

        public static DialogButton Create(DialogButtonType type, string caption = null)
        {
            switch (type)
            {
                case DialogButtonType.Ok:
                    return new DialogButton(type, "OK", true, false, null);
                case DialogButtonType.Cancel:
                    return new DialogButton(type, "Cancel", false, true, null);
                case DialogButtonType.Yes:
                    return new DialogButton(type, "Yes", true, false, null);
                case DialogButtonType.No:
                    return new DialogButton(type, "No", false, false, null);
                case DialogButtonType.Custom:
                    return new DialogButton(type, caption, true, false, null);
                default:
                    throw new ArgumentException("Unsupported dialog button type");
            }
        }
    }
}
