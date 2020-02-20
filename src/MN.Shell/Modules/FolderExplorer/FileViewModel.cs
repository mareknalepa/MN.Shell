using System.IO;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FileViewModel : FileSystemNodeViewModel
    {
        public FileInfo File { get; }

        public FileViewModel(FileInfo fileInfo) : base(fileInfo, false)
        {
            File = fileInfo;
        }
    }
}
