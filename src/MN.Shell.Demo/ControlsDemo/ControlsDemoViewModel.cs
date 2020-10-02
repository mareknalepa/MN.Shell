using MN.Shell.PluginContracts;

namespace MN.Shell.Demo.ControlsDemo
{
    public class ControlsDemoViewModel : DocumentBase
    {
        public ControlsDemoViewModel()
        {
            Title = "Controls Demo";
        }
    }

    public interface IControlsDemoFactory : IDocumentFactory<ControlsDemoViewModel> { }
}
