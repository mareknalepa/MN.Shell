using MN.Shell.Core;

namespace MN.Shell.Framework
{
    public abstract class DocumentBase : LayoutModuleBase, IDocument
    {
        private string _description;

        public virtual string Description
        {
            get => _description;
            set { _description = value; NotifyOfPropertyChange(); }
        }

        public DocumentBase()
        {
            CloseCommand = new RelayCommand(o => TryClose(null));
        }
    }
}
