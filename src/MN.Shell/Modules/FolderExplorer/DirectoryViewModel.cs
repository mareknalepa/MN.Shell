using MN.Shell.Framework.Tree;
using System;
using System.IO;
using System.Linq;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DirectoryViewModel : TreeNodeBase
    {
        public DirectoryInfo Directory { get; }

        public string FullPath => Directory.FullName;

        public override string Name => Directory.Name;

        public bool IsHidden => Parent != null && Directory.Attributes.HasFlag(FileAttributes.Hidden);

        public bool IsSystem => Parent != null && Directory.Attributes.HasFlag(FileAttributes.System);

        private bool _showHidden = true;

        public bool ShowHidden
        {
            get => _showHidden;
            set
            {
                _showHidden = value;
                NotifyOfPropertyChange();
                ForEachChild(child => child.ShowHidden = value);
            }
        }

        private bool _showSystem = true;

        public bool ShowSystem
        {
            get => _showSystem;
            set
            {
                _showSystem = value;
                NotifyOfPropertyChange();
                ForEachChild(child => child.ShowSystem = value);
            }
        }

        public DirectoryViewModel(DirectoryInfo directoryInfo, bool showHidden, bool showSystem)
            : base(isLazyLoadable: true)
        {
            Directory = directoryInfo;
            Name = directoryInfo.Name;
            ShowHidden = showHidden;
            ShowSystem = showSystem;
        }

        protected override void LoadChildren()
        {
            var dirInfo = new DirectoryInfo(FullPath);
            try
            {
                foreach (var subdir in dirInfo.EnumerateDirectories())
                    AttachChild(new DirectoryViewModel(subdir, ShowHidden, ShowSystem));
            }
            catch (SystemException e)
            {
                AttachChild(new SpecialDirectoryViewModel(e.Message));
            }
        }

        protected void ForEachChild(Action<DirectoryViewModel> action)
        {
            foreach (var child in Children.OfType<DirectoryViewModel>())
                action.Invoke(child);
        }
    }
}
