using Microsoft.Extensions.DependencyInjection;
using MN.Shell.PluginContracts;
using System.Runtime.CompilerServices;

namespace MN.Shell.Core
{
    /// <summary>
    /// Context injected externally by plugin loading infrastructure while loading the plugin,
    /// allowing access to various extension points and application-wide features by a plugin composition root
    /// </summary>
    public class PluginContext : IScopedPluginContext, IPluginContext
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Creates new plugin context using dependency injection container
        /// </summary>
        /// <param name="services">Dependency injection container</param>
        public PluginContext(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Plugin calling operations on current context (set by plugin manager)
        /// </summary>
        public IPlugin? PluginInScope { get; set; }

        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        public void UseTool<T>()
            where T : class, ITool
        {
            VerifyScope();
            _services.AddSingleton<ITool, T>();
        }

        /// <summary>
        /// Registers given interface type as auto-implemented document factory
        /// </summary>
        /// <typeparam name="T">Interface of document factory</typeparam>
        public void UseDocumentFactory<T>()
            where T : class, IDocument
        {
            VerifyScope();
            _services.AddTransient<T>();
            _services.AddTransient<Func<T>>(sp => () => sp.GetRequiredService<T>());
        }

        /// <summary>
        /// Registers menu provider
        /// </summary>
        /// <typeparam name="T">Type of menu provider</typeparam>
        public void UseMenuProvider<T>()
            where T : class, IMenuProvider
        {
            VerifyScope();
            _services.AddSingleton<IMenuProvider, T>();
        }

        /// <summary>
        /// Registers status bar provider
        /// </summary>
        /// <typeparam name="T">Type of status bar provider</typeparam>
        public void UseStatusBarProvider<T>()
            where T : class, IStatusBarProvider
        {
            VerifyScope();
            _services.AddSingleton<IStatusBarProvider, T>();
        }

        /// <summary>
        /// Registers service to be injected into any object requiring it
        /// </summary>
        /// <typeparam name="TInterface">Type of service base interface</typeparam>
        /// <typeparam name="TService">Type of service implementation</typeparam>
        public void UseService<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            VerifyScope();
            _services.AddSingleton<TInterface, TService>();
        }

        private void VerifyScope([CallerMemberName] string? callerName = null)
        {
            if (PluginInScope == null)
                throw new InvalidOperationException($"Cannot use {callerName} outside of the plugin scope");
        }
    }
}
