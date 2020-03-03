using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ViewManager locates and creates Views for given ViewModels and then binds resolved Views to their ViewModels
    /// </summary>
    public interface IViewManager
    {
        /// <summary>
        /// Creates or reuses instance of View for given ViewModel, binds them together and returns it
        /// </summary>
        /// <param name="viewModel">ViewModel to create or reuse View for</param>
        /// <returns>View instance binded to given ViewModel, ready to use</returns>
        FrameworkElement GetViewFor(object viewModel);
    }
}
