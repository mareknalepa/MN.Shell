using MN.Shell.Demo.Output;
using MN.Shell.Demo.ProgressBars;
using MN.Shell.Modules.FolderExplorer;
using MN.Shell.PluginContracts;

namespace MN.Shell.Demo
{
    public class DemoPlugin : PluginBase
    {
        protected override void OnLoad()
        {
            Context.UseTool<FolderExplorerViewModel>();
            Context.UseTool<OutputViewModel>();
            Context.UseTool<ProgressBarsViewModel>();
        }
    }
}
