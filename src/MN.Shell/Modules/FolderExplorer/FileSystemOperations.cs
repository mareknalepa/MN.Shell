using System;
using System.Runtime.InteropServices;

namespace MN.Shell.Modules.FolderExplorer
{
    public static class FileSystemOperations
    {
        [Flags]
        private enum OpFlags : ushort
        {
            // Do not show a dialog during the process
            FOF_SILENT = 0x0004,
            // Do not ask the user to confirm selection
            FOF_NOCONFIRMATION = 0x0010,
            // Delete the file to the recycle bin
            FOF_ALLOWUNDO = 0x0040,
            // Do not show the names of the files or folders that are being recycled
            FOF_SIMPLEPROGRESS = 0x0100,
            // Surpress errors, if any occur during the process
            FOF_NOERRORUI = 0x0400,
            // Warn if files are too big to fit in the recycle bin and will need to be deleted completely
            FOF_WANTNUKEWARNING = 0x4000,
        }

        private enum OpType : uint
        {
            // Move the objects
            FO_MOVE = 0x0001,
            // Copy the objects
            FO_COPY = 0x0002,
            // Delete (or recycle) the objects
            FO_DELETE = 0x0003,
            // Rename the object(s)
            FO_RENAME = 0x0004,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public OpType wFunc;
            public string pFrom;
            public string pTo;
            public OpFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        public static void SendToRecycleBin(string path)
        {
            var fs = new SHFILEOPSTRUCT
            {
                wFunc = OpType.FO_DELETE,
                pFrom = path + '\0' + '\0',
                fFlags = OpFlags.FOF_ALLOWUNDO | OpFlags.FOF_NOCONFIRMATION | OpFlags.FOF_WANTNUKEWARNING,
            };
            SHFileOperation(ref fs);
        }
    }
}
