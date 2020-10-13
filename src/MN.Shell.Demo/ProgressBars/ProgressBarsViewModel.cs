using MN.Shell.PluginContracts;

namespace MN.Shell.Demo.ProgressBars
{
    public class ProgressBarsViewModel : ToolBase
    {
        public override ToolPosition InitialPosition => ToolPosition.Right;

        public override double MinWidth => 250.0;

        public ProgressBarsViewModel()
        {
            Title = "Progress Bars";
        }
    }
}
