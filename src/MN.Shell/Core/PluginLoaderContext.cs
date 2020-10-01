﻿using MN.Shell.PluginContracts;
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

        /// <summary>
        /// Registers given tool to be available in shell
        /// </summary>
        /// <typeparam name="T">Type of tool</typeparam>
        public void UseTool<T>() where T : ITool => _kernel.Bind<ITool>().To<T>().InSingletonScope();
    }
}
