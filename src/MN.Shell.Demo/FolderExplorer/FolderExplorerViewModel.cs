using MN.Shell.Framework;

namespace MN.Shell.Demo.FolderExplorer
{
    public class FolderExplorerViewModel : ToolBase
    {
        public override string DisplayName => "Folder Explorer";

        public override ToolPosition InitialPosition => ToolPosition.Left;
    }
}
