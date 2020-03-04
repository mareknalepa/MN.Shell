using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.Tests.Mocks
{
    /// <summary>
    /// Interaction logic for MockUserControlView.xaml
    /// </summary>
    public partial class MockUserControlView : UserControl
    {
        public MockUserControlView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            if (DataContext is MockWindowViewModel mockWindowViewModel)
            {
                mockWindowViewModel.WindowShown = true;

                if (Parent is Window parentWindow && parentWindow.Owner != null)
                    mockWindowViewModel.HasOwnerViewInOwnedWindows = parentWindow.Owner.OwnedWindows
                        .OfType<Window>()
                        .Contains(parentWindow);
            }

            if (Parent is Window window)
                window.Close();
        }
    }
}
