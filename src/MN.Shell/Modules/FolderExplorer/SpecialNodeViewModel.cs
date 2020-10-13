using MN.Shell.Framework.Tree;
using System;

namespace MN.Shell.Modules.FolderExplorer
{
    public class SpecialNodeViewModel : TreeNodeBase
    {
        public bool IsError { get; }

        public SpecialNodeViewModel(string contents)
        {
            Name = contents;
        }

        public SpecialNodeViewModel(Exception e)
        {
            Name = e?.Message;
            IsError = true;
        }

        protected override void OnIsSelectedChanged(bool isSelected)
        {
            if (!isSelected && Parent != null)
                Parent.DetachChild(this);
        }
    }
}
