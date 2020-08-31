namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for ViewModels which should be notified about lifecycle transitions of associated View
    /// </summary>
    public interface ILifecycleAware
    {
        /// <summary>
        /// Specifies the lifecycle state of ViewModel
        /// </summary>
        LifecycleState State { get; }

        /// <summary>
        /// Checks if ViewModel is active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Activates ViewModel
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivates ViewModel
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Closes ViewModel
        /// </summary>
        void Close();
    }
}
