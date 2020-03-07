namespace MN.Shell.MVVM
{
    /// <summary>
    /// Interface for ViewModels which are logical screens (e.g. Window ViewModel or Tab content ViewModel)
    /// </summary>
    public interface IScreen : IViewAware, IChild, IHaveTitle, ILifecycleAware, ICloseConfirm, IRequestClose { }
}
