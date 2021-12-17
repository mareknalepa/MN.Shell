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
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic);

            var potentialAssemblies = Directory.GetFiles(path, "*.dll").Select(f =>
            {
                try
                {
                    return Assembly.LoadFrom(f);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to load assembly from file [{f}]");
                    return null;
                }
            }).
            Where(a => a != null);

            return loadedAssemblies.Concat(potentialAssemblies).Distinct();
        }

        private IEnumerable<Type> FindPluginTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a =>
            {
                _logger.LogTrace($"Scanning assembly [{a.FullName}] for plugins...");
                try
                {
                    return a.GetExportedTypes().
                        Where(t => t.IsClass && t.IsPublic && !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));
                }
                catch (Exception e)
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
                catch (Exception e)
                {
                    _logger.LogError(e, $"Cannot create instance of type [{pluginType}]: {e.Message}");
                    return null;
                }
            }).
            Where(p => p != null);
        }
    }
}
