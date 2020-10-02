using MN.Shell.PluginContracts;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MN.Shell.Core
{
    public static class PluginLoader
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<IPlugin> DiscoverAndLoadPlugins(string path, IPluginContext context)
        {
            _logger.Info($"Starting plugins discovery process using directory [{path}]...");

            var assemblies = LoadAssemblies(path);
            var pluginTypes = FindPluginTypes(assemblies);
            var plugins = CreatePluginInstances(pluginTypes).ToList();

            _logger.Info($"Plugins discovery finished, found {plugins.Count} plugins.");

            foreach (var plugin in plugins)
            {
                _logger.Debug($"Loading plugin [{plugin.Name}]...");
                plugin.Load(context);
            }

            return plugins;
        }

        private static IEnumerable<Assembly> LoadAssemblies(string path)
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
                    _logger.Error(e, $"Failed to load assembly from file [{f}]");
                    return null;
                }
            }).
            Concat(Enumerable.Repeat(Assembly.GetExecutingAssembly(), 1)).
            Where(a => a != null);
        }

        private static IEnumerable<Type> FindPluginTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a =>
            {
                _logger.Debug($"Scanning assembly [{a.FullName}] for plugins...");
                try
                {
                    return a.GetExportedTypes().
                        Where(t => t.IsClass && t.IsPublic && typeof(IPlugin).IsAssignableFrom(t));
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    _logger.Error(e);
                    return Enumerable.Empty<Type>();
                }
            });
        }

        private static IEnumerable<IPlugin> CreatePluginInstances(IEnumerable<Type> pluginTypes)
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
                    _logger.Error(e, $"Cannot create instance of type [{pluginType}]: {e.Message}");
                    return null;
                }
            }).
            Where(p => p != null);
        }
    }
}
