﻿using MN.Shell.Core;
using System;

namespace MN.Shell.Framework.Dialogs
{
    public static class DialogViewModelExtensions
    {
        public static DialogButton AddButton(this IDialog dialog, DialogButtonType type, Action action = null,
            Func<object, bool> canExecute = null)
        {
            DialogButton button = DialogButton.Create(type);
            ProcessAddButton(dialog, button, action, canExecute);

            return button;
        }

        public static DialogButton AddCustomButton(this IDialog dialog, string caption, Action action = null,
            Func<object, bool> canExecute = null)
        {
            DialogButton button = DialogButton.Create(DialogButtonType.Custom, caption);
            ProcessAddButton(dialog, button, action, canExecute);

            return button;
        }

        private static void ProcessAddButton(IDialog dialog, DialogButton button, Action action,
            Func<object, bool> canExecute)
        {
            button.Command = new RelayCommand(o =>
            {
                dialog.SelectedButton = button;

                if (action != null)
                    action();
                else
                    dialog.TryClose(button.IsDefault);
            }, canExecute);

            dialog.Buttons.Add(button);
        }
    }
}