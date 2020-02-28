using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DriveViewModel : DirectoryViewModel
    {
        public DriveInfo Drive { get; }

        public override string Name { get; set; }

        public DriveViewModel(DriveInfo driveInfo) : base(driveInfo.RootDirectory)
        {
            Drive = driveInfo;
            Name = Drive.Name;
            if (!string.IsNullOrEmpty(Drive.VolumeLabel))
                Name += $" [{Drive.VolumeLabel}]";
        }
    }
}
