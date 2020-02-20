using MN.Shell.Framework.Tree;
using System;

namespace MN.Shell.Modules.FolderExplorer
{
    public class SpecialNodeViewModel : TreeNodeBase
    {
        public bool IsError { get; } = false;

        public SpecialNodeViewModel(string contents)
        {
            Name = contents;
        }

        public SpecialNodeViewModel(Exception e)
        {
            Name = e.Message;
            IsError = true;
        }
    }
}
