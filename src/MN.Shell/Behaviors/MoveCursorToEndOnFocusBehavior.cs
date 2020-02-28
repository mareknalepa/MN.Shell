using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MN.Shell.Behaviors
{
    public class MoveCursorToEndOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached() => AssociatedObject.GotFocus += OnGotFocus;

        protected override void OnDetaching() => AssociatedObject.GotFocus -= OnGotFocus;

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(AssociatedObject);

            AssociatedObject.Focus();
            AssociatedObject.CaretIndex = AssociatedObject.Text.Length;
        }
    }
}
