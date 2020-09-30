using MN.Shell.PluginContracts;
using Ninject;

namespace MN.Shell.Core
{
    public class PluginLoaderContext : IPluginLoaderContext
    {
        private readonly IKernel _kernel;

        public PluginLoaderContext(IKernel kernel)
        {
            _kernel = kernel;
        }
    }
}
