using MN.Shell.PluginContracts;

namespace MN.Shell.Core
{
    public interface IScopedPluginContext : IPluginContext
    {
        /// <summary>
        /// Plugin calling operations on current context (set by plugin manager)
        /// </summary>
        IPlugin PluginInScope { get; set; }
    }
}
