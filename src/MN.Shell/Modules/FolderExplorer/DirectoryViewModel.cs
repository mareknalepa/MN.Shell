using System;
using System.IO;
using System.Linq;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DirectoryViewModel : FileSystemNodeViewModel
    {
        public DirectoryInfo Directory { get; }

        public DirectoryViewModel(DirectoryInfo directoryInfo) : base(directoryInfo, isLazyLoadable: true)
        {
            Directory = directoryInfo;
        }

        protected override void LoadChildren()
        {
            try
            {
                foreach (var subdir in Directory.EnumerateDirectories())
                    AttachChild(new DirectoryViewModel(subdir));
                foreach (var file in Directory.EnumerateFiles())
                    AttachChild(new FileViewModel(file));
            }
            catch (SystemException e)
            {
                AttachChild(new SpecialNodeViewModel(e));
            }
        }

        protected void ForEachChildDirectory(Action<DirectoryViewModel> action)
        {
            foreach (var child in Children.OfType<DirectoryViewModel>())
                action.Invoke(child);
        }
    }
}
