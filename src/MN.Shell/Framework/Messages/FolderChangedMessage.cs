using System.IO;

namespace MN.Shell.Framework.Messages
{
    public class FolderChangedMessage
    {
        public FolderChangedMessage(DirectoryInfo currentFolder, DirectoryInfo previousFolder)
        {
            CurrentFolder = currentFolder;
            PreviousFolder = previousFolder;
        }

        public DirectoryInfo CurrentFolder { get; }

        public DirectoryInfo PreviousFolder { get; }
    }
}
