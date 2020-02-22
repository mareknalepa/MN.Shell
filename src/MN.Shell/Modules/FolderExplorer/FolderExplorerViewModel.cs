using Caliburn.Micro;
using MN.Shell.Core;
using MN.Shell.Framework;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.MessageBox;
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
        private readonly IMessageBoxManager _messageBoxManager;

        public FolderExplorerViewModel(IEventAggregator eventAggregator, IMessageBoxManager messageBoxManager)
        {
            _eventAggregator = eventAggregator;
            _messageBoxManager = messageBoxManager;

            ReloadCommand = new RelayCommand(o => ReloadFolders());
            CollapseAllCommand = new RelayCommand(o => CollapseAll());

            NewDirectoryCommand = new RelayCommand(o => NewNode(isDirectory: true),
                o => SelectedDirectory != null && CurrentInsertNode == null && CurrentRenameNode == null);
            NewFileCommand = new RelayCommand(o => NewNode(isDirectory: false),
                o => SelectedDirectory != null && CurrentInsertNode == null && CurrentRenameNode == null);

            ConfirmNewNodeCommand = new RelayCommand(o => ConfirmNewNode(), o => CurrentInsertNode != null);
            CancelNewNodeCommand = new RelayCommand(o => CancelNewNode(), o => CurrentInsertNode != null);

            RenameNodeCommand = new RelayCommand(o => RenameNode(),
                o => SelectedDirectory != null && CurrentInsertNode == null && CurrentRenameNode == null);

            ConfirmRenameNodeCommand = new RelayCommand(o => ConfirmRenameNode(), o => CurrentRenameNode != null);
            CancelRenameNodeCommand = new RelayCommand(o => CancelRenameNode(), o => CurrentRenameNode != null);

            DeleteNodeCommand = new RelayCommand(o => DeleteNode(), o => SelectedNode != null);

            ContextMenuItems.Add(new MenuItemViewModel()
            {
                Name = "Rename",
                Command = RenameNodeCommand,
            });

            ContextMenuItems.Add(new MenuItemViewModel()
            {
                Name = "Delete",
                Command = DeleteNodeCommand,
            });

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
            set { _showHidden = value; NotifyOfPropertyChange(); }
        }

        private bool _showSystem = true;

        public bool ShowSystem
        {
            get => _showSystem;
            set { _showSystem = value; NotifyOfPropertyChange(); }
        }

        private bool _showFiles = true;

        public bool ShowFiles
        {
            get => _showFiles;
            set { _showFiles = value; NotifyOfPropertyChange(); }
        }

        private InsertNodeViewModel _currentInsertNode;

        public InsertNodeViewModel CurrentInsertNode
        {
            get => _currentInsertNode;
            private set { _currentInsertNode = value; NotifyOfPropertyChange(); }
        }

        private FileSystemNodeViewModel _currentRenameNode;

        public FileSystemNodeViewModel CurrentRenameNode
        {
            get => _currentRenameNode;
            private set { _currentRenameNode = value; NotifyOfPropertyChange(); }
        }

        public ICommand ReloadCommand { get; }
        public ICommand CollapseAllCommand { get; }
        public ICommand NewDirectoryCommand { get; }
        public ICommand NewFileCommand { get; }
        public ICommand ConfirmNewNodeCommand { get; }
        public ICommand CancelNewNodeCommand { get; }
        public ICommand RenameNodeCommand { get; }
        public ICommand ConfirmRenameNodeCommand { get; }
        public ICommand CancelRenameNodeCommand { get; }
        public ICommand DeleteNodeCommand { get; }

        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; }
            = new ObservableCollection<MenuItemViewModel>();

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
            if (CurrentInsertNode == null || CurrentInsertNode.Parent == null)
                return;

            var parentDirectory = CurrentInsertNode.Parent as DirectoryViewModel;
            parentDirectory.DetachChild(CurrentInsertNode);
            string newName = CurrentInsertNode.Name;

            try
            {
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
                var specialNode = new SpecialNodeViewModel(e);
                parentDirectory.AttachChild(specialNode, 0);
                specialNode.IsSelected = true;
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

        private void RenameNode()
        {
            if (SelectedNode == null || !(SelectedNode is DirectoryViewModel || SelectedNode is FileViewModel))
                return;

            CurrentRenameNode = SelectedNode;
            CurrentRenameNode.NewName = CurrentRenameNode.Name;
            CurrentRenameNode.IsSelected = true;
            CurrentRenameNode.IsBeingRenamed = true;
        }

        private void ConfirmRenameNode()
        {
            if (CurrentRenameNode == null || CurrentRenameNode.Parent == null)
                return;

            var parentDirectory = CurrentRenameNode.Parent as DirectoryViewModel;
            string newName = CurrentRenameNode.NewName;
            CurrentRenameNode.IsBeingRenamed = false;

            try
            {
                if (CurrentRenameNode is DirectoryViewModel dir)
                    dir.Directory.MoveTo(Path.Combine(parentDirectory.Directory.FullName, newName));
                else if (CurrentRenameNode is FileViewModel file)
                    file.File.MoveTo(Path.Combine(parentDirectory.Directory.FullName, newName));

                parentDirectory.ReloadChildren();

                var justRenamedNode = parentDirectory.Children.
                    FirstOrDefault(child => child.Name == newName);
                if (justRenamedNode != null)
                    justRenamedNode.IsSelected = true;
            }
            catch (Exception e)
            {
                CurrentRenameNode.ErrorMessage = e.Message;
            }

            CurrentRenameNode = null;
        }

        private void CancelRenameNode()
        {
            if (CurrentRenameNode == null)
                return;

            CurrentRenameNode.IsBeingRenamed = false;
            CurrentRenameNode = null;
        }

        private void DeleteNode()
        {
            if (SelectedNode == null || SelectedNode.Parent == null)
                return;

            if (_messageBoxManager.Show("Confirmation",
                $"Are you sure want to move \"{SelectedNode.Name}\" to Recycle Bin?",
                MessageBoxType.Warning, MessageBoxButtons.YesNo) != true)
                return;

            try
            {
                FileSystemOperations.SendToRecycleBin(SelectedNode.ElementInfo.FullName);
                SelectedNode.Parent.IsSelected = true;
                SelectedNode.ReloadChildren();
            }
            catch (Exception e)
            {
                SelectedNode.ErrorMessage = e.Message;
            }
        }

        private void ForEachNode(Action<FileSystemNodeViewModel> action)
        {
            foreach (var node in Folders)
                action.Invoke(node);
        }
    }
}
