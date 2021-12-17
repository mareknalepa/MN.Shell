using System;
using System.IO;

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
    }
}
