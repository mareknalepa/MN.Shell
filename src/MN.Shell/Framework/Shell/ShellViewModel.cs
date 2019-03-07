using Caliburn.Micro;
using MN.Shell.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Framework.Shell
{
    public class ShellViewModel : Screen, IShell
    {
        private readonly ILog _log;

        public ObservableCollection<ITool> Tools { get; }

        public ObservableCollection<IDocument> Documents { get; }

        public ShellViewModel(ILog log, IEnumerable<ITool> tools, IEnumerable<IDocument> documents)
        {
            _log = log;
            _log.Info("Creating ShellViewModel instance...");

            DisplayName = "MN.Shell";

            Tools = new ObservableCollection<ITool>(tools);
            Documents = new ObservableCollection<IDocument>(documents);
        }
    }
}
