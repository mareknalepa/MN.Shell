using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MN.Shell.Controls
{
    public class ShellWindow : Window
    {
        static ShellWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShellWindow),
                new FrameworkPropertyMetadata(typeof(ShellWindow)));
        }

        public ShellWindow()
        {
            SetResourceReference(TitleBarBackgroundProperty, "BaseBrush");
            SetResourceReference(TitleBarForegroundProperty, "ActiveForegroundBrush");

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnClose));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximize, OnCanResize));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimize, OnCanMinimize));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestore, OnCanResize));
        }

        #region "Dependency Properties"

        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(nameof(TitleBarBackground), typeof(Brush), typeof(ShellWindow),
                new FrameworkPropertyMetadata(Brushes.DarkGray, FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));


        public Brush TitleBarForeground
        {
            get { return (Brush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }

        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(nameof(TitleBarForeground), typeof(Brush), typeof(ShellWindow),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));


        public FrameworkElement TitleBarLeftContent
        {
            get { return (FrameworkElement)GetValue(TitleBarLeftContentProperty); }
            set { SetValue(TitleBarLeftContentProperty, value); }
        }

        public static readonly DependencyProperty TitleBarLeftContentProperty =
            DependencyProperty.Register(nameof(TitleBarLeftContent), typeof(FrameworkElement), typeof(ShellWindow),
                new PropertyMetadata(null));


        public FrameworkElement TitleBarRightContent
        {
            get { return (FrameworkElement)GetValue(TitleBarRightContentProperty); }
            set { SetValue(TitleBarRightContentProperty, value); }
        }

        public static readonly DependencyProperty TitleBarRightContentProperty =
            DependencyProperty.Register(nameof(TitleBarRightContent), typeof(FrameworkElement), typeof(ShellWindow),
                new PropertyMetadata(null));

        #endregion

        private void OnClose(object sender, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(this);

        private void OnMaximize(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MaximizeWindow(this);

        private void OnMinimize(object sender, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

        private void OnRestore(object sender, ExecutedRoutedEventArgs e) => SystemCommands.RestoreWindow(this);

        private void OnCanResize(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;

        private void OnCanMinimize(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
    }
}
