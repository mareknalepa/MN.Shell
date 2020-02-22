using MN.Shell.Framework.Tree;
using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public abstract class FileSystemNodeViewModel : TreeNodeBase
    {
        public FileSystemInfo ElementInfo { get; }

        public bool IsHidden => Parent != null && ElementInfo.Attributes.HasFlag(FileAttributes.Hidden);

        public bool IsSystem => Parent != null && ElementInfo.Attributes.HasFlag(FileAttributes.System);

        public bool IsFile => Parent != null && !ElementInfo.Attributes.HasFlag(FileAttributes.Directory);

        private bool _isBeingRenamed;

        public bool IsBeingRenamed
        {
            get => _isBeingRenamed;
            set => Set(ref _isBeingRenamed, value);
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        public FileSystemNodeViewModel(FileSystemInfo fileSystemInfo, bool isLazyLoadable = false)
            : base(isLazyLoadable)
        {
            ElementInfo = fileSystemInfo;
            Name = ElementInfo.Name;
        }
    }
}
