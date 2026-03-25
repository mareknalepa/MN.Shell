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
            Context?.UseTool<FolderExplorerViewModel>();
            Context?.UseTool<OutputViewModel>();
            Context?.UseTool<ProgressBarsViewModel>();

            Context?.UseDocumentFactory<ControlsDemoViewModel>();
            Context?.UseDocumentFactory<TabbedInterfaceViewModel>();

            Context?.UseMenuProvider<DemoMenuProvider>();

            Context?.UseStatusBarProvider<DemoStatusBarProvider>();
        }

        public override void OnStartup(StartupEventArgs e, IApplicationContext applicationContext)
        {
            applicationContext.ApplicationTitle = "MN.Shell Demo Application";

            applicationContext.LoadDocumentUsingFactory<ControlsDemoViewModel>();
            applicationContext.LoadDocumentUsingFactory<TabbedInterfaceViewModel>();
        }
    }
}
