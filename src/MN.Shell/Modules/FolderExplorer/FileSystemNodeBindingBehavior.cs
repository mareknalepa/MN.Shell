using MN.Shell.Framework.Tree;
using System.Windows;
using System.Windows.Data;

namespace MN.Shell.Modules.FolderExplorer
{
    public class FileSystemNodeBindingBehavior : TreeNodeBindingBehavior
    {
        #region "Dependency Properties"

        public bool ShowHidden
        {
            get { return (bool)GetValue(ShowHiddenProperty); }
            set { SetValue(ShowHiddenProperty, value); }
        }

        public static readonly DependencyProperty ShowHiddenProperty =
            DependencyProperty.Register(nameof(ShowHidden), typeof(bool), typeof(FileSystemNodeBindingBehavior),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ShowSystem
        {
            get { return (bool)GetValue(ShowSystemProperty); }
            set { SetValue(ShowSystemProperty, value); }
        }

        public static readonly DependencyProperty ShowSystemProperty =
            DependencyProperty.Register(nameof(ShowSystem), typeof(bool), typeof(FileSystemNodeBindingBehavior),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ShowFiles
        {
            get { return (bool)GetValue(ShowFilesProperty); }
            set { SetValue(ShowFilesProperty, value); }
        }

        public static readonly DependencyProperty ShowFilesProperty =
            DependencyProperty.Register(nameof(ShowFiles), typeof(bool), typeof(FileSystemNodeBindingBehavior),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        public override bool AttachingItemTemplateEnabled => false;

        protected override void OverrideItemContainerStyle(Style style)
        {
            var showHiddenTrigger = new MultiDataTrigger();
            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(ShowHidden))
            {
                Source = this
            }, false));

            showHiddenTrigger.Conditions.Add(new Condition(new Binding(nameof(FileSystemNodeViewModel.IsHidden)), true));
            showHiddenTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            style.Triggers.Add(showHiddenTrigger);

            var showSystemTrigger = new MultiDataTrigger();
            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(ShowSystem))
            {
                Source = this
            }, false));

            showSystemTrigger.Conditions.Add(new Condition(new Binding(nameof(FileSystemNodeViewModel.IsSystem)), true));
            showSystemTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            style.Triggers.Add(showSystemTrigger);

            var showFilesTrigger = new MultiDataTrigger();
            showFilesTrigger.Conditions.Add(new Condition(new Binding(nameof(ShowFiles))
            {
                Source = this
            }, false));

            showFilesTrigger.Conditions.Add(new Condition(new Binding(nameof(FileSystemNodeViewModel.IsFile)), true));
            showFilesTrigger.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            style.Triggers.Add(showFilesTrigger);
        }
    }
}
