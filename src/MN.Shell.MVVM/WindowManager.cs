using System;
using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Service to show windows or dialogs for given ViewModels using ViewModel-first approach
    /// </summary>
    public class WindowManager : IWindowManager
    {
        private readonly IViewManager _viewManager;

        public WindowManager(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        /// <summary>
        /// Show window for given ViewModel and manage its lifecycle
        /// </summary>
        /// <param name="viewModel">ViewModel to show window for</param>
        public void ShowWindow(object viewModel)
        {
            var view = _viewManager.GetViewFor(viewModel);
            var window = ProvideWindow(view);
            window.Show();
        }

        /// <summary>
        /// Show dialog window for given ViewModel, manage its lifecycle and return dialog's result
        /// </summary>
        /// <param name="viewModel">ViewModel to show dialog for</param>
        /// <returns></returns>
        public bool? ShowDialog(object viewModel)
        {
            var view = _viewManager.GetViewFor(viewModel);
            var window = ProvideWindow(view);
            return window.ShowDialog();
        }

        /// <summary>
        /// If given View is not a Window, wraps it in a Window instance
        /// </summary>
        /// <param name="view">View which should be wrapped</param>
        /// <returns>Original View Window or Window holding View as content</returns>
        protected virtual Window ProvideWindow(FrameworkElement view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (!(view is Window window))
            {
                window = new Window()
                {
                    DataContext = view.DataContext,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Content = view,
                };
            }

            return window;
        }
    }
}
