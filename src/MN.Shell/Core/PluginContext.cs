using MN.Shell.PluginContracts;
using Ninject;
using Ninject.Extensions.Factory;

namespace MN.Shell.Core
{
    /// <summary>
    /// Context injected externally by plugin loading infrastructure while loading the plugin,
    /// allowing access to various extension points and application-wide features by a plugin composition root
    /// </summary>
    public class PluginContext : IPluginContext
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Creates new plugin context using dependency injection container
        /// </summary>
        /// <param name="kernel">Dependency injection container</param>
        public PluginContext(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Application context allowing access to application wide-features
        /// </summary>
        public IApplicationContext ApplicationContext => _kernel.Get<IApplicationContext>();

        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        public void UseTool<T>()
            where T : ITool
        {
            _kernel.Bind<ITool>().To<T>().InSingletonScope();
        }

        /// <summary>
        /// Registers given interface type as auto-implemented document factory
        /// </summary>
        /// <typeparam name="T">Interface of document factory</typeparam>
        public void UseDocumentFactory<T, TDocument>()
            where T : class, IDocumentFactory<TDocument>
            where TDocument : IDocument
        {
            _kernel.Bind<T>().ToFactory();
        }
    }
}
