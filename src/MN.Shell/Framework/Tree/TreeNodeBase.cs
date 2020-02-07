using Caliburn.Micro;
using MN.Shell.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MN.Shell.Framework.Tree
{
    public abstract class TreeNodeBase : PropertyChangedBase
    {
        private class LazyLoadingTreeNode : TreeNodeBase { }

        private static readonly TreeNodeBase _lazyLoadingNode = new LazyLoadingTreeNode();

        private bool _suppressExpandingParent = false;

        private string _name;

        public virtual string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ObservableCollection<TreeNodeBase> Children { get; } = new ObservableCollection<TreeNodeBase>();

        private TreeNodeBase _parent;

        public TreeNodeBase Parent
        {
            get => _parent;
            set => Set(ref _parent, value);
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                Set(ref _isExpanded, value);
                if (_isExpanded && Parent != null && !_suppressExpandingParent)
                    Parent.IsExpanded = true;
                if (_isExpanded && Children.Count == 1 && Children[0] == _lazyLoadingNode)
                {
                    Children.Remove(_lazyLoadingNode);
                    LoadChildren();
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        public ICommand ExpandAllCommand { get; }

        public ICommand CollapseAllCommand { get; }

        public TreeNodeBase(bool isLazyLoadable = false)
        {
            if (isLazyLoadable)
                Children.Add(_lazyLoadingNode);

            ExpandAllCommand = new RelayCommand(o => ExpandAll());
            CollapseAllCommand = new RelayCommand(o => CollapseAll());
        }

        public void AttachChild(TreeNodeBase child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void DetachChild(TreeNodeBase child)
        {
            Children.Remove(child);
            if (child.Parent == this)
                child.Parent = null;
        }

        protected virtual void LoadChildren() { }

        private void ExpandAll()
        {
            _suppressExpandingParent = true;
            IsExpanded = true;
            _suppressExpandingParent = false;
            foreach (var child in Children)
                child.ExpandAll();
        }

        private void CollapseAll()
        {
            IsExpanded = false;
            foreach (var child in Children)
                child.CollapseAll();
        }
    }
}
