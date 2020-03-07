namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for components having title (like Window or single Tab ViewModel)
    /// </summary>
    public interface IHaveTitle
    {
        /// <summary>
        /// Title property of component displayed in UI
        /// </summary>
        string Title { get; }
    }
}
