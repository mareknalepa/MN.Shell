using System;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for components which need to be closed (Screens, dialogs, documents)
    /// </summary>
    public interface IClosable
    {
        /// <summary>
        /// Event raised from within a ViewModel requesting to be closed
        /// </summary>
        event EventHandler<bool?> CloseRequested;

        /// <summary>
        /// Asks parent to close this component instance
        /// </summary>
        /// <param name="dialogResult">In case of dialog ViewModel, the result should be passed as an argument</param>
        void RequestClose(bool? dialogResult = null);

        /// <summary>
        /// Checks if current component can be closed
        /// </summary>
        /// <returns>True if closing is confirmed, false if closing should be canceled</returns>
        bool CanBeClosed();
    }
}
