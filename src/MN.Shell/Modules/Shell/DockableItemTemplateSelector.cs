using MN.Shell.Framework;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Modules.Shell
{
    public class DockableItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ITool || item is IDocument)
                return Template;

            return base.SelectTemplate(item, container);
        }
    }
}
