using Caliburn.Micro;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace MN.Shell.Framework.Docking
{
    public class ConductorBehavior : Behavior<DockingManager>
    {
        protected override void OnAttached()
        {
            AttachBindings();
            AttachLayoutItemTemplate();
            AttachLayoutItemContainerStyleSelector();
            AttachLayoutUpdateStrategy();
        }

        protected override void OnDetaching()
        {
        }

        private void AttachBindings()
        {
            AssociatedObject.SetBinding(DockingManager.AnchorablesSourceProperty, new Binding("Tools"));
            AssociatedObject.SetBinding(DockingManager.DocumentsSourceProperty, new Binding("Items"));
            AssociatedObject.SetBinding(DockingManager.ActiveContentProperty, new Binding("ActiveLayoutModule")
            {
                Mode = BindingMode.TwoWay,
            });
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

        private void AttachLayoutItemContainerStyleSelector()
        {
            Style commonLayoutItemStyle = new Style(typeof(LayoutItem));
            Style toolStyle = new Style(typeof(LayoutAnchorableItem), commonLayoutItemStyle);
            Style documentStyle = new Style(typeof(LayoutDocumentItem), commonLayoutItemStyle);

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.ContentIdProperty,
                new Binding($"Model.{nameof(ILayoutModule.ContentId)}")));

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.TitleProperty,
                new Binding($"Model.{nameof(ILayoutModule.DisplayName)}")));

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.CloseCommandProperty,
                new Binding($"Model.{nameof(ILayoutModule.CloseCommand)}")));

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.IsActiveProperty,
                new Binding($"Model.{nameof(ILayoutModule.IsActive)}")));

            toolStyle.Setters.Add(new Setter(LayoutAnchorableItem.VisibilityProperty,
                new Binding($"Model.{nameof(ITool.IsVisible)}")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new Xceed.Wpf.AvalonDock.Converters.BoolToVisibilityConverter(),
                    ConverterParameter = Visibility.Hidden,
                }));

            documentStyle.Setters.Add(new Setter(LayoutDocumentItem.DescriptionProperty,
                new Binding($"Model.{nameof(IDocument.Description)}")));

            AssociatedObject.LayoutItemContainerStyleSelector = new StyleSelector
            {
                ToolStyle = toolStyle,
                DocumentStyle = documentStyle,
            };
        }

        private void AttachLayoutUpdateStrategy()
        {
            var layoutRoot = new LayoutRoot();
            var topLevelLayoutPanel = layoutRoot.Descendents().OfType<LayoutPanel>().First();

            Orientation orientation = Orientation.Horizontal;
            if (topLevelLayoutPanel.Orientation == Orientation.Horizontal)
                orientation = Orientation.Vertical;

            var secondaryPanel = new LayoutPanel() { Orientation = orientation };
            topLevelLayoutPanel.Children.Clear();
            topLevelLayoutPanel.Children.Add(secondaryPanel);

            secondaryPanel.Children.Add(new LayoutDocumentPane());

            AssociatedObject.Layout = layoutRoot;

            AssociatedObject.LayoutUpdateStrategy = new LayoutUpdateStrategy();
        }
    }
}
