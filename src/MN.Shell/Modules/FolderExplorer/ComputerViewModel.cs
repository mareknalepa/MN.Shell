using MN.Shell.Framework.Tree;
using MN.Shell.Properties;
using System;
using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public class ComputerViewModel : TreeNodeBase
    {
        public ComputerViewModel() : base(true)
        {
            try
            {
                Name = $"{Resources.FolderExplorerComputer} [{Environment.MachineName}]";
            }
            catch (InvalidOperationException)
            {
                Name = Resources.FolderExplorerComputer;
            }
        }

        protected override void LoadChildren()
        {
            foreach (var drive in DriveInfo.GetDrives())
                AttachChild(new DriveViewModel(drive));
        }
    }
}
