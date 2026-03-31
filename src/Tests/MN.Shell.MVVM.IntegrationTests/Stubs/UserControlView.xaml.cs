using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    /// <summary>
    /// Interaction logic for UserControlView.xaml
    /// </summary>
    public partial class UserControlView : UserControl
    {
        public UserControlView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (DataContext is UserControlViewModel viewModel)
            {
                viewModel.OnLoadedAction?.Invoke(this);
            }
            if (Parent is Window window)
            {
                window.Close();
            }
        }
    }
}
