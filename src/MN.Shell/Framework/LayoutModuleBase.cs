using Caliburn.Micro;
using System;
using System.Windows.Input;

namespace MN.Shell.Framework
{
    public abstract class LayoutModuleBase : Screen, ILayoutModule
    {
        public string ContentId { get; protected set; } = Guid.NewGuid().ToString();

        public ICommand CloseCommand { get; protected set; }
    }
}
