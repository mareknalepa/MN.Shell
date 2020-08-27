using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Bootstrapper acts as an application entry point and composition root
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Called at application startup to allow Bootstrapper to attach itself to currently running application
        /// in order to start MVVM framework
        /// </summary>
        /// <param name="application">Currently running application to attach to</param>
        void Setup(Application application);
    }
}
