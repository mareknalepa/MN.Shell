using MN.Shell.Core;

namespace MN.Shell.Framework
{
    public abstract class DocumentBase : LayoutModuleBase, IDocument
    {
        private string _description;

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        public DocumentBase()
        {
            CloseCommand = new RelayCommand(o => TryClose(null));
        }
    }
}
