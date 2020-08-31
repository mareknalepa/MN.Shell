using System;
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
            OnLoadedAction?.Invoke(this);
            if (Parent is Window window)
                window.Close();
        }

        public Action<UserControl> OnLoadedAction { get; set; }
    }
}
