using Microsoft.Extensions.DependencyInjection;
using MN.Shell.Modules.Shell;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;

namespace MN.Shell.Core
{
    public static class CoreModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddShellCore(this IServiceCollection services)
            => services
                .AddSingleton<IViewManager, ViewManager>()
                .AddSingleton<IWindowManager, ShellWindowManager>()
                .AddSingleton<IMessageBus, MessageBus>()
                .AddSingleton<IApplicationContext, ApplicationContext>()
                .AddSingleton<ShellViewModel>();
    }
}
