using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Framework.Docking
{
    public class LayoutModuleTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LayoutModuleTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ILayoutModule)
                return LayoutModuleTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
