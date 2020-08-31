using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ApplicationLoader is a smart resource dictionary used in App.xaml to setup Bootstrapper
    /// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class ApplicationLoader : ResourceDictionary
#pragma warning restore CA1710 // Identifiers should have correct suffix
#pragma warning restore CA1010 // Collections should implement generic interface
    {
        private IBootstrapper _bootstrapper;

        /// <summary>
        /// Bootstrapper instance associated with currently running application, should be set via App.xaml
        /// </summary>
        public IBootstrapper Bootstrapper
        {
            get => _bootstrapper;
            set
            {
                _bootstrapper = value;
                _bootstrapper?.Setup(Application.Current);
            }
        }
    }
}
