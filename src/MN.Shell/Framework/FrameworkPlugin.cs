using MN.Shell.Framework.Menu;
using MN.Shell.PluginContracts;

namespace MN.Shell.Framework
{
    public class FrameworkPlugin : PluginBase
    {
        protected override void OnLoad()
        {
            Context.UseMenuProvider<MainMenuProvider>();
        }
    }
}
