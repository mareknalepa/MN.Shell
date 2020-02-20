using System;
using System.IO;
using System.Linq;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DirectoryViewModel : FileSystemNodeViewModel
    {
        public DirectoryInfo Directory { get; }

        private bool _showHidden = true;

        public bool ShowHidden
        {
            get => _showHidden;
            set
            {
                _showHidden = value;
                NotifyOfPropertyChange();
                ForEachChildDirectory(child => child.ShowHidden = value);
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
                ForEachChildDirectory(child => child.ShowSystem = value);
            }
        }

        public DirectoryViewModel(DirectoryInfo directoryInfo, bool showHidden, bool showSystem)
            : base(directoryInfo, isLazyLoadable: true)
        {
            Directory = directoryInfo;
            Name = directoryInfo.Name;
            ShowHidden = showHidden;
            ShowSystem = showSystem;
        }

        protected override void LoadChildren()
        {
            try
            {
                foreach (var subdir in Directory.EnumerateDirectories())
                    AttachChild(new DirectoryViewModel(subdir, ShowHidden, ShowSystem));
            }
            catch (SystemException e)
            {
                AttachChild(new SpecialDirectoryViewModel(e.Message));
            }
        }

        protected void ForEachChildDirectory(Action<DirectoryViewModel> action)
        {
            foreach (var child in Children.OfType<DirectoryViewModel>())
                action.Invoke(child);
        }
    }
}
