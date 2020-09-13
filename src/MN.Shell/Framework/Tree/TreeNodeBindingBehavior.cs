using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MN.Shell.Framework.Tree
{
    public class TreeNodeBindingBehavior : Behavior<TreeView>
    {
        public virtual bool AttachingItemContainerStyleEnabled { get; set; } = true;

        public virtual bool AttachingItemTemplateEnabled { get; set; } = true;

        protected override void OnAttached()
        {
            if (AttachingItemContainerStyleEnabled)
                AttachItemContainerStyle();

            if (AttachingItemTemplateEnabled)
                AttachItemTemplate();
        }

        private void AttachItemContainerStyle()
        {
            var itemContainerStyle = AssociatedObject.ItemContainerStyle;
            if (itemContainerStyle == null)
            {
                if (Application.Current.TryFindResource(typeof(TreeViewItem)) is Style basicTreeViewItemStyle)
                    itemContainerStyle = new Style(typeof(TreeViewItem), basicTreeViewItemStyle);
                else
                    itemContainerStyle = new Style(typeof(TreeViewItem));
            }

            itemContainerStyle.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty,
                new Binding(nameof(TreeNodeBase.IsExpanded))
                {
                    Mode = BindingMode.TwoWay,
                }));

            itemContainerStyle.Setters.Add(new Setter(TreeViewItem.IsSelectedProperty,
                new Binding(nameof(TreeNodeBase.IsSelected))
                {
                    Mode = BindingMode.TwoWay,
                }));

            var selectedTrigger = new Trigger()
            {
                Property = TreeViewItem.IsSelectedProperty,
                Value = true,
            };

            selectedTrigger.Setters.Add(new Setter(TreeNodeBringIntoViewBehavior.IsBringIntoViewProperty, true));

            itemContainerStyle.Triggers.Add(selectedTrigger);

            OverrideItemContainerStyle(itemContainerStyle);

            AssociatedObject.ItemContainerStyle = itemContainerStyle;
        }

        protected virtual void OverrideItemContainerStyle(Style style) { }

        private void AttachItemTemplate()
        {
            var template = new HierarchicalDataTemplate(typeof(TreeNodeBase))
            {
                ItemsSource = new Binding(nameof(TreeNodeBase.Children)),
                VisualTree = GetItemTemplateFactory(),
            };
            template.Seal();
            AssociatedObject.ItemTemplate = template;
        }

        protected virtual FrameworkElementFactory GetItemTemplateFactory()
        {
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding(nameof(TreeNodeBase.Name)));

            return factory;
        }
    }
}
