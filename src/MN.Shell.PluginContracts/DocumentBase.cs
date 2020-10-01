using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    public class DocumentBase : Screen, IDocument
    {
        public ICommand CloseCommand { get; }

        private string _description;

        public virtual string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        public DocumentBase()
        {
            CloseCommand = new Command(() => RequestClose(null));
        }
    }
}
