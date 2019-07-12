using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Controls;

namespace MN.Shell.Framework.Docking
{
    public class ConductorBehavior : Behavior<DockingManager>
    {
        protected override void OnAttached()
        {
            AttachLayoutItemTemplate();
            AttachLayoutItemContainerStyle();
        }

        protected override void OnDetaching()
        {
        }

        private void AttachLayoutItemTemplate()
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ContentControl));

            Binding viewModelBinding = new Binding
            {
                Mode = BindingMode.OneWay
            };
            factory.SetBinding(View.ModelProperty, viewModelBinding);

            factory.SetValue(View.ApplyConventionsProperty, false);
            factory.SetValue(Control.IsTabStopProperty, false);
            factory.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
            factory.SetValue(Control.VerticalContentAlignmentProperty, VerticalAlignment.Stretch);

            AssociatedObject.LayoutItemTemplate = new DataTemplate(typeof(ILayoutModule))
            {
                VisualTree = factory,
            };
        }

        private void AttachLayoutItemContainerStyle()
        {
            Style style = new Style(typeof(LayoutItem));
            style.Setters.Add(new Setter(LayoutItem.TitleProperty, new Binding("Model.DisplayName")));

            AssociatedObject.LayoutItemContainerStyle = style;
        }
    }
}
