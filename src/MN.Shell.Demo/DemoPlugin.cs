using MN.Shell.Demo.ControlsDemo;
using MN.Shell.Demo.Output;
using MN.Shell.Demo.ProgressBars;
using MN.Shell.Demo.TabbedInterface;
using MN.Shell.Modules.FolderExplorer;
using MN.Shell.PluginContracts;
using System.Windows;

namespace MN.Shell.Demo
{
    public class DemoPlugin : PluginBase
    {
        protected override void OnLoad()
        {
            Context.UseTool<FolderExplorerViewModel>();
            Context.UseTool<OutputViewModel>();
            Context.UseTool<ProgressBarsViewModel>();

            Context.UseDocumentFactory<IControlsDemoFactory, ControlsDemoViewModel>();
            Context.UseDocumentFactory<ITabbedInterfaceFactory, TabbedInterfaceViewModel>();
        }

        public override void OnStartup(StartupEventArgs e)
        {
            Context.ApplicationContext.ApplicationTitle = "MN.Shell Demo Application";

            Context.ApplicationContext.LoadDocumentUsingFactory<IControlsDemoFactory, ControlsDemoViewModel>();
            Context.ApplicationContext.LoadDocumentUsingFactory<ITabbedInterfaceFactory, TabbedInterfaceViewModel>();
        }
    }
}
