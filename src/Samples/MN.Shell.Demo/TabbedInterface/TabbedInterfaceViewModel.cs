using MN.Shell.PluginContracts;

namespace MN.Shell.Demo.TabbedInterface
{
    public class TabbedInterfaceViewModel : DocumentBase
    {
        public TabbedInterfaceViewModel()
        {
            Title = "Tabbed Interface";
        }
    }

    public interface ITabbedInterfaceFactory : IDocumentFactory<TabbedInterfaceViewModel> { }
}
