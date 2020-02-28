using MN.Shell.Framework.Tree;
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
                Name = $"Computer [{Environment.MachineName}]";
            }
            catch (InvalidOperationException)
            {
                Name = "Computer";
            }
        }

        protected override void LoadChildren()
        {
            foreach (var drive in DriveInfo.GetDrives())
                AttachChild(new DriveViewModel(drive));
        }
    }
}
