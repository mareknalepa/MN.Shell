using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MN.Shell.Behaviors
{
    public class KeyboardFocusBehavior : Behavior<TextBox>
    {
        #region "Dependency Properties"

        public bool HasKeyboardFocus
        {
            get { return (bool)GetValue(HasKeyboardFocusProperty); }
            set { SetValue(HasKeyboardFocusProperty, value); }
        }

        public static readonly DependencyProperty HasKeyboardFocusProperty =
            DependencyProperty.Register(nameof(HasKeyboardFocus), typeof(bool), typeof(KeyboardFocusBehavior),
                new UIPropertyMetadata(false, new PropertyChangedCallback(OnHasKeyboardFocusChanged)));

        #endregion

        private bool _isAttached;

        protected override void OnAttached() => _isAttached = true;

        protected override void OnDetaching() => _isAttached = false;

        private static void OnHasKeyboardFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyboardFocusBehavior behavior && behavior._isAttached && behavior.HasKeyboardFocus)
            {
                behavior.AssociatedObject.Focus();
                Keyboard.Focus(behavior.AssociatedObject);
            }
        }
    }
}
