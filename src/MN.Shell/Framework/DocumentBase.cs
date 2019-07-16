using MN.Shell.Core;

namespace MN.Shell.Framework
{
    public abstract class DocumentBase : LayoutModuleBase, IDocument
    {
        public DocumentBase()
        {
            CloseCommand = new RelayCommand(o => TryClose(null));
        }
    }
}
