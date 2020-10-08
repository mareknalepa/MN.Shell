using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MN.Shell.Core
{
    /// <summary>
    /// Plugin manager holds references to all currently loaded plugins and manages their lifecycle
    /// </summary>
    public sealed class PluginManager : IDisposable
    {
        private readonly List<IPlugin> _plugins = new List<IPlugin>();

        /// <summary>
        /// Collection of plugins loaded and managed by this plugin manager
        /// </summary>
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();

        /// <summary>
        /// Adds collection of plugins to be managed by this plugin manager
        /// </summary>
        /// <param name="discoveredPlugins">Collection of plugins to add</param>
        public void AddPlugins(IEnumerable<IPlugin> discoveredPlugins) =>
            _plugins.AddRange(discoveredPlugins);

        /// <summary>
        /// Method which should be called upon application's startup process to allow plugins to hook-in
        /// </summary>
        /// <param name="e">StartupEventArgs passed to plugins</param>
        public void OnStartup(StartupEventArgs e)
        {
            _plugins.ForEach(p => p.OnStartup(e));
        }

        /// <summary>
        /// Method which should be called upon application's exit to allow plugins gracefully shutdown
        /// </summary>
        /// <param name="e">ExitEventArgs passed to plugins</param>
        public void OnExit(ExitEventArgs e)
        {
            _plugins.ForEach(p => p.OnExit(e));
        }

        /// <summary>
        /// Disposes all the plugins and plugin manager itself
        /// </summary>
        public void Dispose()
        {
            _plugins.OfType<IDisposable>().ToList().ForEach(p => p.Dispose());
            _plugins.Clear();
        }
    }
}
