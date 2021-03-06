﻿using MN.Shell.PluginContracts;
using Ninject;
using Ninject.Extensions.Factory;
using System;
using System.Runtime.CompilerServices;

namespace MN.Shell.Core
{
    /// <summary>
    /// Context injected externally by plugin loading infrastructure while loading the plugin,
    /// allowing access to various extension points and application-wide features by a plugin composition root
    /// </summary>
    public class PluginContext : IScopedPluginContext, IPluginContext
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
        /// Plugin calling operations on current context (set by plugin manager)
        /// </summary>
        public IPlugin PluginInScope { get; set; }

        /// <summary>
        /// Application context allowing access to application wide-features
        /// </summary>
        public IApplicationContext ApplicationContext => _kernel.Get<IApplicationContext>();

        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        public void UseTool<T>()
            where T : class, ITool
        {
            VerifyScope();
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
            VerifyScope();
            _kernel.Bind<T>().ToFactory();
        }

        /// <summary>
        /// Registers menu provider
        /// </summary>
        /// <typeparam name="T">Type of menu provider</typeparam>
        public void UseMenuProvider<T>()
            where T : class, IMenuProvider
        {
            VerifyScope();
            _kernel.Bind<IMenuProvider>().To<T>().InSingletonScope();
        }

        /// <summary>
        /// Registers status bar provider
        /// </summary>
        /// <typeparam name="T">Type of status bar provider</typeparam>
        public void UseStatusBarProvider<T>()
            where T : class, IStatusBarProvider
        {
            VerifyScope();
            _kernel.Bind<IStatusBarProvider>().To<T>().InSingletonScope();
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
            _kernel.Bind<TInterface, TService>().To<TService>().InSingletonScope();
        }

        private void VerifyScope([CallerMemberName] string callerName = null)
        {
            if (PluginInScope == null)
                throw new InvalidOperationException($"Cannot use {callerName} outside of the plugin scope");
        }
    }
}
