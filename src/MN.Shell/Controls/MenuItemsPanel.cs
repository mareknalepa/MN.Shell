using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MN.Shell.Controls
{
    public class MenuItemsPanel : Panel
    {
        #region "Dependency Properties"

        public MenuItem EllipsisMenuItem
        {
            get { return (MenuItem)GetValue(EllipsisMenuItemProperty); }
            set { SetValue(EllipsisMenuItemProperty, value); }
        }

        public static readonly DependencyProperty EllipsisMenuItemProperty =
            DependencyProperty.Register(nameof(EllipsisMenuItem), typeof(MenuItem), typeof(MenuItemsPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelSize = new Size(0.0, 0.0);

            if (EllipsisMenuItem != null)
            {
                EllipsisMenuItem.Measure(availableSize);
                if (EllipsisMenuItem.Items.Count > 0)
                {
                    EllipsisMenuItem.Items.Clear();

                    var parent = VisualTreeHelper.GetParent(this);
                    while (parent != null && !(parent is Menu))
                        parent = VisualTreeHelper.GetParent(parent);

                    if (parent is Menu menu)
                        menu.Items.Refresh();
                }
            }

            var childrenUiElements = InternalChildren.OfType<UIElement>().ToList();
            int itemsLeft = childrenUiElements.Count;

            foreach (var child in childrenUiElements)
            {
                child.Measure(availableSize);

                double nextLenght = child.DesiredSize.Width;
                if (itemsLeft > 1 && EllipsisMenuItem != null)
                    nextLenght += EllipsisMenuItem.ActualWidth;

                if ((panelSize.Width + nextLenght) <= availableSize.Width)
                {
                    panelSize.Width += child.DesiredSize.Width;
                    panelSize.Height = Math.Max(panelSize.Height, child.DesiredSize.Height);
                    --itemsLeft;
                }
            }

            if (itemsLeft > 0 && EllipsisMenuItem != null)
            {
                panelSize.Width += EllipsisMenuItem.ActualWidth;
                panelSize.Height = Math.Max(panelSize.Height, EllipsisMenuItem.ActualHeight);
            }

            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double horizontalOffset = 0.0;
            bool isCollapsed = false;

            var childrenUiElements = InternalChildren.OfType<UIElement>().ToList();
            int itemsLeft = childrenUiElements.Count;

            foreach (var child in childrenUiElements)
            {
                double nextLenght = child.DesiredSize.Width;
                if (itemsLeft > 1 && EllipsisMenuItem != null)
                    nextLenght += EllipsisMenuItem.ActualWidth;

                if (!isCollapsed && (horizontalOffset + nextLenght) <= finalSize.Width)
                {
                    child.Arrange(new Rect(horizontalOffset, 0.0, child.DesiredSize.Width, child.DesiredSize.Height));
                    horizontalOffset += child.DesiredSize.Width;
                    --itemsLeft;
                }
                else
                {
                    isCollapsed = true;
                    child.Arrange(new Rect(0.0, finalSize.Height, child.DesiredSize.Width, child.DesiredSize.Height));
                    if (EllipsisMenuItem != null)
                        EllipsisMenuItem.Items.Add(child);
                }
            }

            if (EllipsisMenuItem != null)
            {
                EllipsisMenuItem.Margin = new Thickness(horizontalOffset, 0.0, 0.0, 0.0);

                if (itemsLeft > 0)
                    EllipsisMenuItem.Visibility = Visibility.Visible;
                else
                    EllipsisMenuItem.Visibility = Visibility.Hidden;
            }

            return finalSize;
        }
    }
}
