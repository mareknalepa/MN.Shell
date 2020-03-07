namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for child components (i.e. having logical parent component)
    /// </summary>
    public interface IChild
    {
        /// <summary>
        /// Logical parent component
        /// </summary>
        IParent Parent { get; set; }
    }
}
