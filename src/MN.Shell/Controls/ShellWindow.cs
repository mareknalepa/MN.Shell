using System.Windows;
using System.Windows.Input;

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
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnClose));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximize, OnCanResize));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimize, OnCanMinimize));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestore, OnCanResize));
        }

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
