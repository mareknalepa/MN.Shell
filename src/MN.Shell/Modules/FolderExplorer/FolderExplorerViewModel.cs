using Caliburn.Micro;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Framework.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FolderExplorerViewModel : ToolBase
    {
        private readonly IEventAggregator _eventAggregator;

        public FolderExplorerViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ReloadCommand = new RelayCommand(o => ReloadFolders());
            CollapseAllCommand = new RelayCommand(o => CollapseAll());

            ReloadFolders();
        }

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
            set
            {
                var message = new FolderChangedMessage(value?.Directory, _selectedFolder?.Directory);
                _selectedFolder = value;
                NotifyOfPropertyChange();
                _eventAggregator.PublishOnUIThread(message);
            }
        }

        private bool _showHidden = true;

        public bool ShowHidden
        {
            get => _showHidden;
            set => Set(ref _showHidden, value);
        }

        private bool _showSystem = true;

        public bool ShowSystem
        {
            get => _showSystem;
            set => Set(ref _showSystem, value);
        }

        private bool _showFiles = true;

        public bool ShowFiles
        {
            get => _showFiles;
            set => Set(ref _showFiles, value);
        }

        public ICommand ReloadCommand { get; }

        public ICommand CollapseAllCommand { get; }

        public void ReloadFolders()
        {
            var selectedFolder = SelectedFolder;

            Folders.Clear();
            foreach (var drive in DriveInfo.GetDrives())
                Folders.Add(new DriveViewModel(drive));

            if (selectedFolder != null)
                TrySelectFolder(selectedFolder.Directory.FullName);
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

        public void CollapseAll()
        {
            ForEachNode(node => node.CollapseAllCommand.Execute(null));
        }

        private void ForEachNode(Action<FileSystemNodeViewModel> action)
        {
            foreach (var node in Folders)
                action.Invoke(node);
        }
    }
}
