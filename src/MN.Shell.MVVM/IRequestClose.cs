namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for components
    /// </summary>
    public interface IRequestClose : IChild
    {
        /// <summary>
        /// Asks parent to close this component instance
        /// </summary>
        /// <param name="dialogResult">In case of dialog ViewModel, the result should be passed as an argument</param>
        void RequestClose(bool? dialogResult = null);
    }
}
