using MN.Shell.Core;
using MN.Shell.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FolderExplorerViewModel : ToolBase
    {
        public override string DisplayName => "Folder Explorer";

        public override ToolPosition InitialPosition => ToolPosition.Left;

        public override double MinWidth => 320;

        public override double AutoHideMinWidth => 320;

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
            var selectedFolder = SelectedFolder;

            Folders.Clear();
            foreach (var drive in DriveInfo.GetDrives())
                Folders.Add(new DirectoryViewModel(drive.RootDirectory));

            if (selectedFolder != null)
                TrySelectFolder(selectedFolder.FullPath);
        }

        public void TrySelectFolder(string path)
        {
            var components = path.Split(Path.DirectorySeparatorChar);
            if (components.Length <= 1)
                return;

            components[0] += Path.DirectorySeparatorChar;
            IEnumerable<DirectoryViewModel> searchIn = Folders;

            foreach (var component in components)
            {
                var foundNode = searchIn.FirstOrDefault(d => d.Name == component);
                if (foundNode == null)
                    break;

                foundNode.IsExpanded = true;
                foundNode.IsSelected = true;
                searchIn = foundNode.Children.OfType<DirectoryViewModel>();
            }
        }
    }
}
