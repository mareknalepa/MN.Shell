using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Framework.Tree
{
    public class TreeNodeBringIntoViewBehavior
    {
        #region "Attached Properties"

        private static bool IsBringIntoView { get; }

        public static bool GetIsBringIntoView(DependencyObject obj) =>
            (bool)obj.GetValue(IsBringIntoViewProperty);

        public static void SetIsBringIntoView(DependencyObject obj, bool value) =>
            obj.SetValue(IsBringIntoViewProperty, value);

        public static readonly DependencyProperty IsBringIntoViewProperty =
            DependencyProperty.RegisterAttached(nameof(IsBringIntoView), typeof(bool),
                typeof(TreeNodeBringIntoViewBehavior), new UIPropertyMetadata(false, OnIsBringIntoViewChanged));

        private static void OnIsBringIntoViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeViewItem treeViewItem && e.NewValue is bool isBringIntoView)
                treeViewItem.BringIntoView();
        }

        #endregion
    }
}
