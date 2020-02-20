using MN.Shell.Framework.Tree;
using System.Windows;
using System.Windows.Data;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FileSystemNodeBindingBehavior : TreeNodeBindingBehavior
    {
        public override bool AttachingItemTemplateEnabled => false;

        protected override void OverrideItemContainerStyle(Style style)
        {
            var showHiddenTrigger = new MultiDataTrigger();
            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.IsHidden)), true));
            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.ShowHidden)), false));
            showHiddenTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            style.Triggers.Add(showHiddenTrigger);

            var showSystemTrigger = new MultiDataTrigger();
            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.IsSystem)), true));
            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(DirectoryViewModel.ShowSystem)), false));
            showSystemTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            style.Triggers.Add(showSystemTrigger);
        }
    }
}
