using System.Windows;

namespace MN.Shell.MVVM.Tests.Mocks
{
    /// <summary>
    /// Interaction logic for MockWindowView.xaml
    /// </summary>
    public partial class MockWindowView : Window
    {
        public MockWindowView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (DataContext is MockWindowViewModel mockWindowViewModel)
                mockWindowViewModel.WindowShown = true;
            Close();
        }
    }
}
