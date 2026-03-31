using System.Windows;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    /// <summary>
    /// Interaction logic for WindowWithTitleView.xaml
    /// </summary>
    public partial class WindowWithTitleView : Window
    {
        public WindowWithTitleView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (DataContext is WindowWithTitleViewModel viewModel)
            {
                viewModel.OnLoadedAction?.Invoke(this);
            }
            Close();
        }
    }
}
