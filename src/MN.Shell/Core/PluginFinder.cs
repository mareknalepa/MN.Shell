using Microsoft.Extensions.Logging;
using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MN.Shell.Core
{
    public class PluginFinder
    {
        private readonly ILogger _logger;

        public PluginFinder(ILogger logger)
        {
            _logger = logger;
        }

        public IList<IPlugin> DiscoverPlugins(string path)
        {
            _logger.LogInformation($"Starting plugins discovery process using directory [{path}]...");

            var assemblies = LoadAssemblies(path);
            var pluginTypes = FindPluginTypes(assemblies);
            var plugins = CreatePluginInstances(pluginTypes).ToList();

            _logger.LogInformation($"Plugins discovery finished, found {plugins.Count} plugins.");

            return plugins;
        }

        private IEnumerable<Assembly> LoadAssemblies(string path)
        {
            return Directory.GetFiles(path, "*.dll").Select(f =>
            {
                try
                {
                    return Assembly.LoadFrom(f);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    _logger.LogError(e, $"Failed to load assembly from file [{f}]");
                    return null;
                }
            }).
            Concat(Enumerable.Repeat(Assembly.GetExecutingAssembly(), 1)).
            Where(a => a != null).
            Distinct();
        }

        private IEnumerable<Type> FindPluginTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a =>
            {
                _logger.LogDebug($"Scanning assembly [{a.FullName}] for plugins...");
                try
                {
                    return a.GetExportedTypes().
                        Where(t => t.IsClass && t.IsPublic && typeof(IPlugin).IsAssignableFrom(t));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    _logger.LogError(e, e.Message);
                    return Enumerable.Empty<Type>();
                }
            });
        }

        private IEnumerable<IPlugin> CreatePluginInstances(IEnumerable<Type> pluginTypes)
        {
            return pluginTypes.Select(pluginType =>
            {
                try
                {
                    object pluginInstance = Activator.CreateInstance(pluginType);
                    if (pluginInstance is IPlugin plugin)
                        return plugin;

                    return null;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    _logger.LogError(e, $"Cannot create instance of type [{pluginType}]: {e.Message}");
                    return null;
                }
            }).
            Where(p => p != null);
        }
    }
}
