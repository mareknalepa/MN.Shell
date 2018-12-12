using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Core.Dialogs
{
    public abstract class DialogViewModelBase : Screen
    {
        public BindableCollection<DialogButton> Buttons { get; } = new BindableCollection<DialogButton>();

        protected void CreateButtons(IEnumerable<DialogButtonType> dialogButtonTypes)
        {
            foreach (DialogButtonType type in dialogButtonTypes)
            {
                Buttons.Add(DialogButton.Create(type));
            }
        }
    }
}
