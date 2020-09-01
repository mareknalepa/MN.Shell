using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using MN.Shell.Controls;

namespace MN.Shell.Behaviors
{
    public class PlaceOnTitleBarBehavior : Behavior<FrameworkElement>
    {
        public bool IsOnRightSide { get; set; }

        protected override void OnAttached() => AssociatedObject.Loaded += OnAssociatedObjectLoaded;

        protected override void OnDetaching() => AssociatedObject.Loaded -= OnAssociatedObjectLoaded;

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(AssociatedObject) is ShellWindow shellWindow)
            {
                var parent = VisualTreeHelper.GetParent(AssociatedObject);

                if (parent is Panel parentAsPanel)
                    parentAsPanel.Children.Remove(AssociatedObject);
                else if (parent is ContentControl parentAsContentControl)
                    parentAsContentControl.Content = null;
                else if (parent is Decorator parentAsDecorator)
                    parentAsDecorator.Child = null;

                if (IsOnRightSide)
                    shellWindow.TitleBarRightContent = AssociatedObject;
                else
                    shellWindow.TitleBarLeftContent = AssociatedObject;
            }
        }
    }
}
