using Caliburn.Micro;
using MN.Shell.Framework;
using System.Collections.Generic;

namespace MN.Shell.Dialogs
{
    public abstract class DialogViewModelBase : Screen
    {
        public BindableCollection<DialogButton> Buttons { get; } = new BindableCollection<DialogButton>();

        public DialogButton SelectedButton { get; private set; }

        protected void CreateButtons(IEnumerable<DialogButtonType> dialogButtonTypes)
        {
            foreach (DialogButtonType type in dialogButtonTypes)
            {
                DialogButton button = DialogButton.Create(type);

                if (button.IsCancel)
                    button.Command = new RelayCommand(o => { TryClose(false); SelectedButton = button; });
                else
                    button.Command = new RelayCommand(o => { TryClose(true); SelectedButton = button; });

                Buttons.Add(button);
            }
        }
    }
}
