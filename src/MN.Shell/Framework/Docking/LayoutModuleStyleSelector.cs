using MN.Shell.PluginContracts;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Framework.Docking
{
    public class LayoutModuleStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }
        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolStyle;
            else if (item is IDocument)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
