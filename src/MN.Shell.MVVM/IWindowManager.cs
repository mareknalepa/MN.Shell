namespace MN.Shell.MVVM
{
    /// <summary>
    /// Service to show windows or dialogs for given ViewModels using ViewModel-first approach
    /// </summary>
    interface IWindowManager
    {
        /// <summary>
        /// Show window for given ViewModel and manage its lifecycle
        /// </summary>
        /// <param name="viewModel">ViewModel to show window for</param>
        void ShowWindow(object viewModel);

        /// <summary>
        /// Show dialog window for given ViewModel, manage its lifecycle and return dialog's result
        /// </summary>
        /// <param name="viewModel">ViewModel to show dialog for</param>
        /// <returns></returns>
        bool? ShowDialog(object viewModel);
    }
}
