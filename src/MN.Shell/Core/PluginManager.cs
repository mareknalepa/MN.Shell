using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly List<IPlugin> _plugins = new();

        public PluginManager(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Collection of plugins loaded and managed by this plugin manager
        /// </summary>
        public IReadOnlyCollection<IPlugin> Plugins => _plugins.AsReadOnly();

        /// <summary>
        /// Adds already discovered plugins to internal list and loads them by calling their composition roots
        /// </summary>
        /// <param name="discoveredPlugins">Collection of plugins to load</param>
        /// <param name="context">Plugin context injected into each plugin composition root</param>
        public void LoadPlugins(IEnumerable<IPlugin> discoveredPlugins, IScopedPluginContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _plugins.AddRange(discoveredPlugins);

            foreach (var plugin in _plugins)
            {
                _logger.LogInformation($"Loading plugin: [{plugin.Name}]");

                context.PluginInScope = plugin;
                plugin.Load(context);
                context.PluginInScope = null;
            }
        }

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
