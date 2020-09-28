using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

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
            var window = ProvideWindow(viewModel, false);
            window.Show();
        }

        /// <summary>
        /// Show dialog window for given ViewModel, manage its lifecycle and return dialog's result
        /// </summary>
        /// <param name="viewModel">ViewModel to show dialog for</param>
        /// <returns>Dialog result</returns>
        public bool? ShowDialog(object viewModel)
        {
            var window = ProvideWindow(viewModel, true);
            return window.ShowDialog();
        }

        /// <summary>
        /// Delegate to get current active Window, needs to be set up during applications bootup
        /// </summary>
        public Func<Window> GetActiveWindow { get; set; }

        /// <summary>
        /// Creates Window for given ViewModel, attaches necessary handlers to it, binds to ViewModel and returns it
        /// </summary>
        /// <param name="viewModel">ViewModel to create Window for</param>
        /// <param name="isDialog">True if Window is modal dialog, false if it's independent Window</param>
        /// <returns>Ready to use Window</returns>
        protected virtual Window ProvideWindow(object viewModel, bool isDialog)
        {
            var view = _viewManager.GetViewFor(viewModel);
            var window = EnsureWindow(view, isDialog);
            SetupWindow(viewModel, window, isDialog);
            AttachHandlers(viewModel, window, isDialog);
            BindWindow(viewModel, window, isDialog);

            return window;
        }

        /// <summary>
        /// Returns Window for given View - if View is not already a Window, creates one and wraps View with it
        /// </summary>
        /// <param name="view">View to ensure Window for</param>
        /// <param name="isDialog">True if Window is modal dialog, false if it's independent Window</param>
        /// <returns>View as a Window</returns>
        protected virtual Window EnsureWindow(FrameworkElement view, bool isDialog)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (!(view is Window window))
            {
                window = new Window()
                {
                    DataContext = view.DataContext,
                    SizeToContent = SizeToContent.Manual,
                    Content = view,
                };
            }

            return window;
        }

        /// <summary>
        /// Sets window's owner if applicable and sets startup location
        /// </summary>
        /// <param name="viewModel">ViewModel to bind to</param>
        /// <param name="window">Window to attach handlers for</param>
        /// <param name="isDialog">True if Window is modal dialog, false if it's independent Window</param>
        protected virtual void SetupWindow(object viewModel, Window window, bool isDialog)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            if (isDialog)
            {
                var activeWindow = GetActiveWindow?.Invoke();
                if (activeWindow != null && !ReferenceEquals(activeWindow, window))
                {
                    try
                    {
                        window.Owner = activeWindow;
                    }
                    catch (InvalidOperationException) { }
                }
            }

            if (window.Owner != null)
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// Attaches Window lifecycle events' handlers to forward them to ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to bind to</param>
        /// <param name="window">Window to attach handlers for</param>
        /// <param name="isDialog">True if Window is modal dialog, false if it's independent Window</param>
        protected virtual void AttachHandlers(object viewModel, Window window, bool isDialog)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            EventHandler onActivated = null;
            EventHandler onDeactivated = null;
            EventHandler<bool?> onCloseRequested = null;
            CancelEventHandler onClosing = null;

            if (viewModel is ILifecycleAware lifecycleAware)
            {
                onActivated = (sender, e) => lifecycleAware.Activate();
                window.Activated += onActivated;

                onDeactivated = (sender, e) => lifecycleAware.Deactivate();
                window.Deactivated += onDeactivated;
            }

            if (viewModel is IClosable closable)
            {
                onCloseRequested = (sender, dialogResult) =>
                {
                    if (!window.DialogResult.HasValue && !dialogResult.HasValue && isDialog)
                        window.DialogResult = dialogResult;
                    else
                        window.Close();
                };

                closable.CloseRequested += onCloseRequested;

                onClosing = (sender, e) => e.Cancel = !closable.CanBeClosed();
                window.Closing += onClosing;
            }

            void OnClosed(object sender, EventArgs e)
            {
                if (sender is Window wnd)
                {
                    if (wnd.DataContext is ILifecycleAware lifecycleAwareDataContext)
                        lifecycleAwareDataContext.Close();

                    if (wnd.DataContext is IClosable closableDataContext)
                        closableDataContext.CloseRequested -= onCloseRequested;

                    wnd.Activated -= onActivated;
                    wnd.Deactivated -= onDeactivated;
                    wnd.Closing -= onClosing;
                    wnd.Closed -= OnClosed;
                }
            }

            window.Closed += OnClosed;
        }

        /// <summary>
        /// Creates Window-specific bindings (e.g. Title) between Window and its ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to bind to</param>
        /// <param name="window">Window to attach handlers for</param>
        /// <param name="isDialog">True if Window is modal dialog, false if it's independent Window</param>
        protected virtual void BindWindow(object viewModel, Window window, bool isDialog)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (window == null)
                throw new ArgumentNullException(nameof(window));

            if (viewModel is IHaveTitle && string.IsNullOrEmpty(window.Title) &&
                BindingOperations.GetBindingBase(window, Window.TitleProperty) == null)
            {
                window.SetBinding(Window.TitleProperty, new Binding(nameof(IHaveTitle.Title))
                {
                    Mode = BindingMode.OneWay,
                });
            }
        }
    }
}
