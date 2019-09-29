using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.Controls
{
    public class MenuItemsPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            Size panelSize = new Size(0.0, 0.0);

            foreach (var child in InternalChildren.OfType<UIElement>())
            {
                child.Measure(availableSize);

                if ((panelSize.Width + child.DesiredSize.Width) <= availableSize.Width)
                {
                    panelSize.Width += child.DesiredSize.Width;
                    panelSize.Height = Math.Max(panelSize.Height, child.DesiredSize.Height);
                }
            }

            return panelSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double horizontalOffset = 0.0;
            bool isCollapsed = false;

            foreach (var child in InternalChildren.OfType<UIElement>())
            {
                if (!isCollapsed && (horizontalOffset + child.DesiredSize.Width) <= finalSize.Width)
                {
                    child.Arrange(new Rect(horizontalOffset, 0.0, child.DesiredSize.Width, child.DesiredSize.Height));
                    horizontalOffset += child.DesiredSize.Width;
                }
                else
                {
                    isCollapsed = true;
                    child.Arrange(new Rect(0.0, finalSize.Height, child.DesiredSize.Width, child.DesiredSize.Height));
                }
            }

            return finalSize;
        }
    }
}
