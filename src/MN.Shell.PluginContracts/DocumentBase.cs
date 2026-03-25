using MN.Shell.MVVM;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    /// <summary>
    /// Base class for document view models
    /// </summary>
    public class DocumentBase : Screen, IDocument
    {
        /// <summary>
        /// Command used to close given layout module
        /// </summary>
        public ICommand CloseCommand { get; }

        private string _description = string.Empty;

        /// <summary>
        /// Description of document
        /// </summary>
        public virtual string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        /// <summary>
        /// Creates new document view model
        /// </summary>
        public DocumentBase()
        {
            CloseCommand = new Command(() => RequestClose(null));
        }
    }
}
