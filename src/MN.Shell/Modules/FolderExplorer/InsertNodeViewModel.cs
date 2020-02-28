using MN.Shell.Framework.Tree;

namespace MN.Shell.Modules.FolderExplorer
{
    public class InsertNodeViewModel : TreeNodeBase
    {
        public bool IsDirectory { get; }

        public InsertNodeViewModel(bool isDirectory)
        {
            IsDirectory = isDirectory;
        }
    }
}
