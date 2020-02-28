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
            set { _isBeingRenamed = value; NotifyOfPropertyChange(); }
        }

        private string _newName = string.Empty;

        public string NewName
        {
            get => _newName;
            set { _newName = value; NotifyOfPropertyChange(); }
        }

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; NotifyOfPropertyChange(); }
        }

        public FileSystemNodeViewModel(FileSystemInfo fileSystemInfo, bool isLazyLoadable = false)
            : base(isLazyLoadable)
        {
            ElementInfo = fileSystemInfo;
            Name = ElementInfo.Name;
            NewName = Name;
        }

        protected override void OnIsSelectedChanged(bool isSelected)
        {
            if (!isSelected)
                ErrorMessage = string.Empty;
        }
    }
}
