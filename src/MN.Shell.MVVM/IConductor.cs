namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for ViewModels having many children components and managing their lifecycle
    /// </summary>
    /// <typeparam name="T">Type of managed items</typeparam>
    public interface IConductor<T> : IScreen
        where T : class
    {
        /// <summary>
        /// Make given item owned by current component and activate it
        /// </summary>
        /// <param name="item">Item to activate</param>
        void ActivateItem(T item);

        /// <summary>
        /// Close given item and disown it from current component
        /// </summary>
        /// <param name="item">Item to close</param>
        void CloseItem(T item);
    }
}
