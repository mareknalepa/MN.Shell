using MN.Shell.Core;
using MN.Shell.Framework;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FolderExplorerViewModel : ToolBase
    {
        public override string DisplayName => "Folder Explorer";

        public override ToolPosition InitialPosition => ToolPosition.Left;

        public ObservableCollection<DirectoryViewModel> Folders { get; }
            = new ObservableCollection<DirectoryViewModel>();

        private DirectoryViewModel _selectedFolder;

        public DirectoryViewModel SelectedFolder
        {
            get => _selectedFolder;
            set => Set(ref _selectedFolder, value);
        }

        public ICommand ReloadCommand { get; }

        public FolderExplorerViewModel()
        {
            ReloadCommand = new RelayCommand(o => ReloadFolders());
            ReloadFolders();
        }

        public void ReloadFolders()
        {
            Folders.Clear();
            foreach (var drive in DriveInfo.GetDrives())
                Folders.Add(new DirectoryViewModel(drive.RootDirectory));
        }
    }
}
