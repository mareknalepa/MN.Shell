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
            NewDirectoryCommand = new RelayCommand(o => NewNode(isDirectory: true),
                o => SelectedDirectory != null && CurrentInsertNode == null);
            NewFileCommand = new RelayCommand(o => NewNode(isDirectory: false),
                o => SelectedDirectory != null && CurrentInsertNode == null);
            ConfirmNewNodeCommand = new RelayCommand(o => ConfirmNewNode(), o => CurrentInsertNode != null);
            CancelNewNodeCommand = new RelayCommand(o => CancelNewNode(), o => CurrentInsertNode != null);

            ReloadFolders();
        }

        public override string DisplayName => "Folder Explorer";

        public override ToolPosition InitialPosition => ToolPosition.Left;

        public override double MinWidth => 320;

        public override double AutoHideMinWidth => 320;

        public ObservableCollection<DirectoryViewModel> Folders { get; }
            = new ObservableCollection<DirectoryViewModel>();

        private FileSystemNodeViewModel _selectedNode;

        public FileSystemNodeViewModel SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                NotifyOfPropertyChange();

                if (_selectedNode is DirectoryViewModel directory)
                    SelectedDirectory = directory;
                else if (_selectedNode is FileViewModel file)
                    SelectedDirectory = file.Parent as DirectoryViewModel;
                else
                    SelectedDirectory = null;
            }
        }

        private DirectoryViewModel _selectedDirectory;

        public DirectoryViewModel SelectedDirectory
        {
            get => _selectedDirectory;
            set
            {
                if (_selectedDirectory == value)
                    return;

                var message = new FolderChangedMessage(value?.Directory, _selectedDirectory?.Directory);
                _selectedDirectory = value;
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

        private InsertNodeViewModel _currentInsertNode;

        public InsertNodeViewModel CurrentInsertNode
        {
            get => _currentInsertNode;
            private set => Set(ref _currentInsertNode, value);
        }

        public ICommand ReloadCommand { get; }

        public ICommand CollapseAllCommand { get; }

        public ICommand NewDirectoryCommand { get; }

        public ICommand NewFileCommand { get; }

        public ICommand ConfirmNewNodeCommand { get; }

        public ICommand CancelNewNodeCommand { get; }

        public void ReloadFolders()
        {
            var selectedDirectory = SelectedDirectory;

            Folders.Clear();
            foreach (var drive in DriveInfo.GetDrives())
                Folders.Add(new DriveViewModel(drive));

            if (selectedDirectory != null)
                TrySelectFolder(selectedDirectory.Directory.FullName);
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

        private void NewNode(bool isDirectory)
        {
            if (SelectedDirectory == null)
                return;

            SelectedDirectory.IsExpanded = true;

            CurrentInsertNode = new InsertNodeViewModel(isDirectory);
            SelectedDirectory.AttachChild(CurrentInsertNode, 0);
            CurrentInsertNode.IsSelected = true;
        }

        private void ConfirmNewNode()
        {
            if (CurrentInsertNode == null)
                return;

            var parentDirectory = CurrentInsertNode.Parent as DirectoryViewModel;
            parentDirectory.DetachChild(CurrentInsertNode);

            try
            {
                string newName = CurrentInsertNode.Name;

                if (CurrentInsertNode.IsDirectory)
                    parentDirectory.Directory.CreateSubdirectory(CurrentInsertNode.Name);
                else
                    using (var fs = File.Create(Path.Combine(parentDirectory.Directory.FullName, newName)))
                        fs.Close();

                parentDirectory.ReloadChildren();

                var justCreatedNode = parentDirectory.Children.
                    FirstOrDefault(child => child.Name == newName);
                if (justCreatedNode != null)
                    justCreatedNode.IsSelected = true;
            }
            catch (Exception e)
            {
                parentDirectory.AttachChild(new SpecialNodeViewModel(e), 0);
            }

            CurrentInsertNode = null;
        }

        private void CancelNewNode()
        {
            if (CurrentInsertNode == null || CurrentInsertNode.Parent == null)
                return;

            var parentDirectory = CurrentInsertNode.Parent;
            parentDirectory.DetachChild(CurrentInsertNode);

            CurrentInsertNode = null;
        }

        private void ForEachNode(Action<FileSystemNodeViewModel> action)
        {
            foreach (var node in Folders)
                action.Invoke(node);
        }
    }
}
