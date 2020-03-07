namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for ViewModels which should be notified about lifecycle transitions of associated View
    /// </summary>
    public interface ILifecycleAware
    {
        /// <summary>
        /// Called once on ViewModel when associated View is initialized for the first time
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called on ViewModel when associated View is activated
        /// </summary>
        void Activate();

        /// <summary>
        /// Called on ViewModel when associated View is deactivated
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Called on ViewModel when associated View is closed
        /// </summary>
        void Close();
    }
}
