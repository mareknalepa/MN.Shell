using MN.Shell.Framework.Tree;
using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FileSystemNodeViewModel : TreeNodeBase
    {
        public FileSystemInfo ElementInfo { get; }

        public override string Name => ElementInfo.Name;

        public bool IsHidden => Parent != null && ElementInfo.Attributes.HasFlag(FileAttributes.Hidden);

        public bool IsSystem => Parent != null && ElementInfo.Attributes.HasFlag(FileAttributes.System);

        public FileSystemNodeViewModel(FileSystemInfo fileSystemInfo, bool isLazyLoadable = false)
            : base(isLazyLoadable)
        {
            ElementInfo = fileSystemInfo;
        }
    }
}
