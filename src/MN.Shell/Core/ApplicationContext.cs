using MN.Shell.PluginContracts;
using Ninject;
using System;
using System.Collections.Generic;

namespace MN.Shell.Core
{
    public class ApplicationContext : IApplicationContext
    {
        private readonly IKernel _kernel;

        public ApplicationContext(IKernel kernel)
        {
            _kernel = kernel;
        }

        #region "Application Title"

        private string _applicationTitle = string.Empty;

        public event EventHandler<string> ApplicationTitleChanged;

        /// <summary>
        /// Application title shown on title bar
        /// </summary>
        public string ApplicationTitle
        {
            get => _applicationTitle;
            set
            {
                if (_applicationTitle != value)
                {
                    _applicationTitle = value;
                    ApplicationTitleChanged?.Invoke(this, _applicationTitle);
                }
            }
        }

        #endregion

        #region "Exit handling"

        public event EventHandler ApplicationExitRequested;

        /// <summary>
        /// Requests application to gracefully shutdown
        /// </summary>
        public void RequestApplicationExit() => ApplicationExitRequested?.Invoke(this, EventArgs.Empty);

        #endregion

        #region "Documents"

        public event EventHandler DocumentLoadRequested;

        public Queue<IDocument> DocumentsToLoad { get; } = new Queue<IDocument>();

        /// <summary>
        /// Loads document of given type using its factory
        /// </summary>
        /// <typeparam name="T">Document factory type</typeparam>
        /// <typeparam name="TDocument">Document type</typeparam>
        public void LoadDocumentUsingFactory<T, TDocument>()
            where T : IDocumentFactory<TDocument>
            where TDocument : IDocument
        {
            var factory = _kernel.Get<T>();
            var vm = factory.Create();

            DocumentsToLoad.Enqueue(vm);
            DocumentLoadRequested?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
