using MN.Shell.Framework.Tree;
using System;
using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DirectoryViewModel : TreeNodeBase
    {
        public DirectoryInfo Directory { get; }

        public string FullPath => Directory.FullName;

        public override string Name => Directory.Name;

        public DirectoryViewModel(DirectoryInfo directoryInfo) : base(isLazyLoadable: true)
        {
            Directory = directoryInfo;
            Name = directoryInfo.Name;
        }

        protected override void LoadChildren()
        {
            var dirInfo = new DirectoryInfo(FullPath);
            try
            {
                foreach (var subdir in dirInfo.EnumerateDirectories())
                    AttachChild(new DirectoryViewModel(subdir));
            }
            catch (SystemException e)
            {
                AttachChild(new SpecialDirectoryViewModel(e.Message));
            }
        }
    }
}
