using System.Windows;
using System.Windows.Controls;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    /// <summary>
    /// Interaction logic for LifecycleTestView.xaml
    /// </summary>
    public partial class LifecycleTestView : UserControl
    {
        public LifecycleTestView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            if (Parent is Window window)
            {
                window.Close();
            }
        }
    }
}
