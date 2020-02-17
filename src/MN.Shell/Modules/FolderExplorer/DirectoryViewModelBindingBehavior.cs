using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace MN.Shell.Modules.FolderExplorer
{
    public class DirectoryViewModelBindingBehavior : Behavior<TreeView>
    {
        protected override void OnAttached()
        {
            AttachItemContainerStyle();
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

            var showHiddenTrigger = new MultiDataTrigger();
            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.IsHidden)), true));
            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.ShowHidden)), false));
            showHiddenTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            itemContainerStyle.Triggers.Add(showHiddenTrigger);

            var showSystemTrigger = new MultiDataTrigger();
            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.IsSystem)), true));
            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.ShowSystem)), false));
            showSystemTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            itemContainerStyle.Triggers.Add(showSystemTrigger);

            AssociatedObject.ItemContainerStyle = itemContainerStyle;
        }
    }
}
