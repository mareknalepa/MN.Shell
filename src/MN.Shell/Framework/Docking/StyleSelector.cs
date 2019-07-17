using System.Windows;

namespace MN.Shell.Framework.Docking
{
    public class StyleSelector : System.Windows.Controls.StyleSelector
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
