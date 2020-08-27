using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ApplicationLoaderDictionary is a smart resource dictionary used in App.xaml to setup Bootstrapper
    /// </summary>
#pragma warning disable CA1010 // Collections should implement generic interface
    public class ApplicationLoaderDictionary : ResourceDictionary
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
