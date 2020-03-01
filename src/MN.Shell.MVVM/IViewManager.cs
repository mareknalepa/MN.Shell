using System;
using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ViewManager locates and creates Views for given ViewModels and then binds resolved Views to their ViewModels
    /// </summary>
    public interface IViewManager
    {
        /// <summary>
        /// Finds type of View for given ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to find View for</param>
        /// <returns>View type</returns>
        Type LocateViewFor(object viewModel);

        /// <summary>
        /// Creates instance of View for given View type
        /// </summary>
        /// <param name="viewType">View type</param>
        /// <returns>Instance of View</returns>
        FrameworkElement CreateViewFor(Type viewType);

        /// <summary>
        /// Bind View to ViewModel by setting the DataContext
        /// </summary>
        /// <param name="view">View to bind</param>
        /// <param name="viewModel">ViewModel to bind to</param>
        void BindViewToViewModel(FrameworkElement view, object viewModel);
    }
}
