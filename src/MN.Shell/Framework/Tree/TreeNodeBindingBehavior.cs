using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace MN.Shell.Framework.Tree
{
    public class TreeNodeBindingBehavior : Behavior<TreeView>
    {
        protected override void OnAttached()
        {
            AttachItemContainerStyle();
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

            AssociatedObject.ItemContainerStyle = itemContainerStyle;
        }

        private void AttachItemTemplate()
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));

            factory.SetBinding(TextBlock.TextProperty, new Binding(nameof(TreeNodeBase.Name)));

            AssociatedObject.ItemTemplate = new HierarchicalDataTemplate(typeof(TreeNodeBase))
            {
                ItemsSource = new Binding(nameof(TreeNodeBase.Children)),
                VisualTree = factory,
            };
        }
    }
}
