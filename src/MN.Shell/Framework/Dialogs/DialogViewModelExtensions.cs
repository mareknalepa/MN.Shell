using MN.Shell.MVVM;
using System;

namespace MN.Shell.Framework.Dialogs
{
    public static class DialogViewModelExtensions
    {
        public static DialogButton AddButton(this IDialog dialog, DialogButtonType type, Action action = null,
            Func<bool> canExecute = null)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

            DialogButton button = DialogButton.Create(type);
            ProcessAddButton(dialog, button, action, canExecute);

            return button;
        }

        public static DialogButton AddCustomButton(this IDialog dialog, string caption, Action action = null,
            Func<bool> canExecute = null)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

            DialogButton button = DialogButton.Create(DialogButtonType.Custom, caption);
            ProcessAddButton(dialog, button, action, canExecute);

            return button;
        }

        private static void ProcessAddButton(IDialog dialog, DialogButton button, Action action, Func<bool> canExecute)
        {
            button.Command = new Command(() =>
            {
                dialog.SelectedButton = button;

                if (action != null)
                    action();
                else
                    dialog.RequestClose(button.IsDefault);
            }, canExecute);

            dialog.Buttons.Add(button);
        }
    }
}
