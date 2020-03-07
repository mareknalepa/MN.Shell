namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for components which need to confirm closing process
    /// </summary>
    public interface ICloseConfirm
    {
        /// <summary>
        /// Checks if current component can be closed
        /// </summary>
        /// <returns>True if closing is confirmed, false if closing should be canceled</returns>
        bool CanBeClosed();
    }
}
