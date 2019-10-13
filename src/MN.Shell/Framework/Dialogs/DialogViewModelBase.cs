using Caliburn.Micro;
using MN.Shell.Core;
using System;

namespace MN.Shell.Framework.Dialogs
{
    public abstract class DialogViewModelBase : Screen
    {
        public BindableCollection<DialogButton> Buttons { get; } = new BindableCollection<DialogButton>();

        public DialogButton SelectedButton { get; private set; }

        protected void AddButton(DialogButtonType dialogButtonType)
        {
            var dialogButton = DialogButton.Create(dialogButtonType);
            dialogButton.Command = new RelayCommand(o =>
            {
                SelectedButton = dialogButton;
                TryClose(!dialogButton.IsCancel);
            });
            Buttons.Add(dialogButton);
        }

        protected void AddCustomButton(string caption, Action<DialogViewModelBase> action)
        {
            var dialogButton = DialogButton.CreateCustom(caption);
            dialogButton.Command = new RelayCommand(o =>
            {
                SelectedButton = dialogButton;
                action?.Invoke(this);
            });
            Buttons.Add(dialogButton);
        }
    }
}
