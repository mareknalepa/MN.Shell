using System.Windows;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    /// <summary>
    /// Interaction logic for WindowView.xaml
    /// </summary>
    public partial class WindowView : Window
    {
        public WindowView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (DataContext is WindowViewModel viewModel)
            {
                viewModel.OnLoadedAction?.Invoke(this);
            }
            Close();
        }
    }
}
