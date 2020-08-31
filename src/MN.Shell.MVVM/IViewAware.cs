using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Allows ViewModels to cache their associated Views instances for reusing
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Reference to the View associated with this ViewModel
        /// </summary>
        FrameworkElement View { get; }

        /// <summary>
        /// Attaches View to this ViewModel
        /// </summary>
        /// <param name="view">View to attach to ViewModel</param>
        void AttachView(FrameworkElement view);
    }
}
