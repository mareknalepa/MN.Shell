using System.Linq;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace MN.Shell.Framework.Docking
{
    public class LayoutUpdateStrategy : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow,
            ILayoutContainer destinationContainer)
        {
            if (anchorableToShow.Content is ITool tool)
            {
                var pane = layout.Descendents().OfType<LayoutAnchorablePane>()
                    .FirstOrDefault(p => p.Name == $"{tool.InitialPosition}Pane");

                if (pane == null)
                {
                    Orientation orientation = Orientation.Horizontal;
                    if (tool.InitialPosition == ToolPosition.Top || tool.InitialPosition == ToolPosition.Bottom)
                        orientation = Orientation.Vertical;

                    LayoutPanel parent = layout.Descendents().OfType<LayoutPanel>()
                        .First(p => p.Orientation == orientation);

                    pane = new LayoutAnchorablePane() { Name = $"{tool.InitialPosition}Pane" };

                    if (tool.InitialPosition == ToolPosition.Left || tool.InitialPosition == ToolPosition.Top)
                        parent.InsertChildAt(0, pane);
                    else
                        parent.Children.Add(pane);
                }

                pane.Children.Add(anchorableToShow);

                if (pane.DockMinWidth < tool.MinWidth)
                    pane.DockMinWidth = tool.MinWidth;
                if (pane.DockMinHeight < tool.MinHeight)
                    pane.DockMinHeight = tool.MinHeight;

                return true;
            }

            return false;
        }

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            if (anchorableShown.Content is ITool tool)
            {
                anchorableShown.AutoHideMinWidth = tool.AutoHideMinWidth;
                anchorableShown.AutoHideMinHeight = tool.AutoHideMinHeight;
            }
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow,
            ILayoutContainer destinationContainer) => false;

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown) { }
    }
}
