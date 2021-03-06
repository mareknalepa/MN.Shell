﻿using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Converters;
using AvalonDock.Layout;
using Microsoft.Xaml.Behaviors;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MN.Shell.Framework.Docking
{
    public class ConductorBehavior : Behavior<DockingManager>
    {
        protected override void OnAttached()
        {
            AttachBindings();
            AttachLayoutModuleTemplateSelector();
            AttachLayoutModuleStyleSelector();
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

        private void AttachLayoutModuleTemplateSelector()
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(ContentControl));

            Binding viewModelBinding = new Binding
            {
                Mode = BindingMode.OneWay
            };
            factory.SetBinding(Binder.ViewModelProperty, viewModelBinding);

            factory.SetValue(Control.IsTabStopProperty, false);
            factory.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
            factory.SetValue(Control.VerticalContentAlignmentProperty, VerticalAlignment.Stretch);

            AssociatedObject.LayoutItemTemplateSelector = new LayoutModuleTemplateSelector()
            {
                LayoutModuleTemplate = new DataTemplate(typeof(ILayoutModule))
                {
                    VisualTree = factory,
                },
            };
        }

        private void AttachLayoutModuleStyleSelector()
        {
            Style commonLayoutItemStyle = new Style(typeof(LayoutItem));
            Style toolStyle = new Style(typeof(LayoutAnchorableItem), commonLayoutItemStyle);
            Style documentStyle = new Style(typeof(LayoutDocumentItem), commonLayoutItemStyle);

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.TitleProperty,
                new Binding($"Model.{nameof(ILayoutModule.Title)}")));

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.CloseCommandProperty,
                new Binding($"Model.{nameof(ILayoutModule.CloseCommand)}")));

            commonLayoutItemStyle.Setters.Add(new Setter(LayoutItem.IsActiveProperty,
                new Binding($"Model.{nameof(ILayoutModule.IsActive)}")));

            toolStyle.Setters.Add(new Setter(LayoutAnchorableItem.VisibilityProperty,
                new Binding($"Model.{nameof(ITool.IsVisible)}")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new BoolToVisibilityConverter(),
                    ConverterParameter = Visibility.Hidden,
                }));

            documentStyle.Setters.Add(new Setter(LayoutDocumentItem.DescriptionProperty,
                new Binding($"Model.{nameof(IDocument.Description)}")));

            AssociatedObject.LayoutItemContainerStyleSelector = new LayoutModuleStyleSelector
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
