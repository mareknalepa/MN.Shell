using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MN.Shell.Controls
{
    public class ShellMenuItemsPanel : Panel
    {
        private MenuItem _ellipsisMenuItem;

        #region "Attached Properties"

        public static bool GetIsEllipsis(DependencyObject obj) =>
            (bool)obj.GetValue(IsEllipsisProperty);

        public static void SetIsEllipsis(DependencyObject obj, bool value) =>
            obj.SetValue(IsEllipsisProperty, value);

        public static readonly DependencyProperty IsEllipsisProperty =
            DependencyProperty.RegisterAttached("IsEllipsis", typeof(bool), typeof(ShellMenuItemsPanel),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsEllipsisChanged)));

        #endregion

        private static void OnIsEllipsisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem menuItem && e.NewValue is bool boolValue && boolValue)
            {
                var parentPanel = GetVisualParentOfType<ShellMenuItemsPanel>(menuItem);
                if (parentPanel != null)
                    parentPanel._ellipsisMenuItem = menuItem;

                if (menuItem.TryFindResource("ShellMenuItemStyle") is Style shellMenuItemStyle)
                {
                    var ellipsisSubMenuItemStyle = new Style(typeof(MenuItem), shellMenuItemStyle);
                    var isOverflownTrigger = new DataTrigger()
                    {
                        Binding = new Binding("FitsIntoMainMenu"),
                        Value = true,
                    };
                    isOverflownTrigger.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));

                    ellipsisSubMenuItemStyle.Triggers.Add(isOverflownTrigger);
                    menuItem.ItemContainerStyle = ellipsisSubMenuItemStyle;
                }
            }
        }

        private static T GetVisualParentOfType<T>(DependencyObject d)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(d);
            while (parent != null && !(parent is T))
                parent = VisualTreeHelper.GetParent(parent);

            if (parent is T parentOfType)
                return parentOfType;

            return null;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelSize = new Size(0.0, 0.0);

            if (_ellipsisMenuItem != null)
            {
                _ellipsisMenuItem.Measure(availableSize);
            }

            var childrenUiElements = InternalChildren.OfType<UIElement>().Where(c => c != _ellipsisMenuItem).ToList();
            int itemsLeft = childrenUiElements.Count;

            foreach (var child in childrenUiElements)
            {
                child.Measure(availableSize);

                double nextLenght = child.DesiredSize.Width;
                if (itemsLeft > 1 && _ellipsisMenuItem != null)
                    nextLenght += _ellipsisMenuItem.ActualWidth;

                if ((panelSize.Width + nextLenght) <= availableSize.Width)
                {
                    panelSize.Width += child.DesiredSize.Width;
                    panelSize.Height = Math.Max(panelSize.Height, child.DesiredSize.Height);
                    --itemsLeft;
                }
            }

            if (itemsLeft > 0 && _ellipsisMenuItem != null)
            {
                panelSize.Width += _ellipsisMenuItem.ActualWidth;
                panelSize.Height = Math.Max(panelSize.Height, _ellipsisMenuItem.ActualHeight);
            }

            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double horizontalOffset = 0.0;
            bool isCollapsed = false;

            var childrenUiElements = InternalChildren.OfType<UIElement>().Where(c => c != _ellipsisMenuItem).ToList();
            int itemsLeft = childrenUiElements.Count;

            foreach (var child in childrenUiElements)
            {
                double nextLenght = child.DesiredSize.Width;
                if (itemsLeft > 1 && _ellipsisMenuItem != null)
                    nextLenght += _ellipsisMenuItem.ActualWidth;

                if (!isCollapsed && (horizontalOffset + nextLenght) <= finalSize.Width)
                {
                    child.Arrange(new Rect(horizontalOffset, 0.0, child.DesiredSize.Width, child.DesiredSize.Height));
                    horizontalOffset += child.DesiredSize.Width;
                    --itemsLeft;

                    if (child is MenuItem menuItem && menuItem.DataContext is Framework.Menu.MenuItemViewModel vm)
                        vm.FitsIntoMainMenu = true;
                }
                else
                {
                    isCollapsed = true;
                    child.Arrange(new Rect(0.0, finalSize.Height, child.DesiredSize.Width, child.DesiredSize.Height));

                    if (child is MenuItem menuItem && menuItem.DataContext is Framework.Menu.MenuItemViewModel vm)
                        vm.FitsIntoMainMenu = false;
                }
            }

            if (_ellipsisMenuItem != null)
            {
                _ellipsisMenuItem.Arrange(new Rect(horizontalOffset, 0.0,
                    _ellipsisMenuItem.DesiredSize.Width, _ellipsisMenuItem.DesiredSize.Height));

                if (itemsLeft > 0)
                    _ellipsisMenuItem.Visibility = Visibility.Visible;
                else
                    _ellipsisMenuItem.Visibility = Visibility.Collapsed;
            }

            return finalSize;
        }
    }
}
