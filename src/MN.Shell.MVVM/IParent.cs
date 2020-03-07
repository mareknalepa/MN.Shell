namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for Parent components (i.e. conductors which can manage their children lifecycle)
    /// </summary>
    public interface IParent
    {
        /// <summary>
        /// Attach given child and make it owned by current component
        /// </summary>
        /// <param name="child">Child to attach</param>
        void AttachChild(IChild child);

        /// <summary>
        /// Detach given child and disown it from current component
        /// </summary>
        /// <param name="child">Child to detach</param>
        void DetachChild(IChild child);

        /// <summary>
        /// Close given child component
        /// </summary>
        /// <param name="child">Component to close</param>
        void CloseChild(IChild child);
    }
}
