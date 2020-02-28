using Caliburn.Micro;
using MN.Shell.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MN.Shell.Framework.Tree
{
    public abstract class TreeNodeBase : PropertyChangedBase
    {
        private class LazyLoadingTreeNode : TreeNodeBase { }

        private static readonly TreeNodeBase _lazyLoadingNode = new LazyLoadingTreeNode();

        private bool _suppressExpandingParent = false;

        private string _name = string.Empty;

        public virtual string Name
        {
            get => _name;
            set { _name = value; NotifyOfPropertyChange(); }
        }

        private readonly ObservableCollection<TreeNodeBase> _children = new ObservableCollection<TreeNodeBase>();

        public IEnumerable<TreeNodeBase> Children => _children;

        private TreeNodeBase _parent;

        public TreeNodeBase Parent
        {
            get => _parent;
            set { _parent = value; NotifyOfPropertyChange(); }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange();
                if (_isExpanded && Parent != null && !_suppressExpandingParent)
                    Parent.IsExpanded = true;
                if (_isExpanded && _children.Count == 1 && _children[0] == _lazyLoadingNode)
                {
                    _children.Remove(_lazyLoadingNode);
                    LoadChildren();
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyOfPropertyChange();
                    OnIsSelectedChanged(_isSelected);
                }
            }
        }

        protected virtual void OnIsSelectedChanged(bool isSelected) { }

        public ICommand ExpandAllCommand { get; }

        public ICommand CollapseAllCommand { get; }

        public TreeNodeBase(bool isLazyLoadable = false)
        {
            if (isLazyLoadable)
                _children.Add(_lazyLoadingNode);

            ExpandAllCommand = new RelayCommand(o => ExpandAll());
            CollapseAllCommand = new RelayCommand(o => CollapseAll());
        }

        public void ClearChildren()
        {
            var previousChildren = _children.ToList();
            _children.Clear();

            foreach (var child in previousChildren)
            {
                if (child.Parent == this)
                    child.Parent = null;
            }
        }

        public void AttachChild(TreeNodeBase child, int index = -1)
        {
            if (index < 0)
                _children.Add(child);
            else
                _children.Insert(index, child);
            child.Parent = this;
        }

        public void DetachChild(TreeNodeBase child)
        {
            _children.Remove(child);
            if (child.Parent == this)
                child.Parent = null;
        }

        public void ReloadChildren()
        {
            ClearChildren();
            LoadChildren();
        }

        protected virtual void LoadChildren() { }

        public void ExpandAll()
        {
            _suppressExpandingParent = true;
            IsExpanded = true;
            _suppressExpandingParent = false;
            foreach (var child in Children)
                child.ExpandAll();
        }

        public void CollapseAll()
        {
            IsExpanded = false;
            foreach (var child in Children)
                child.CollapseAll();
        }
    }
}
