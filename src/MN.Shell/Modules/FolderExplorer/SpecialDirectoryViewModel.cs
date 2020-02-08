using MN.Shell.Framework.Tree;

namespace MN.Shell.Modules.FolderExplorer
{
    public class SpecialDirectoryViewModel : TreeNodeBase
    {
        public SpecialDirectoryViewModel(string contents)
        {
            Name = contents;
        }
    }
}
