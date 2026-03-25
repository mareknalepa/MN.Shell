using Microsoft.Extensions.DependencyInjection;
using MN.Shell.Framework.ColorSchemes;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.MessageBox;
using MN.Shell.Framework.StatusBar;

namespace MN.Shell.Framework
{
    public static class FrameworkModuleServiceCollectionExtensions
    {
        public static IServiceCollection AddShellFramework(this IServiceCollection services)
            => services
                .AddSingleton<IMenuManager, MenuManager>()
                .AddSingleton<IStatusBarManager, StatusBarManager>()
                .AddSingleton<IColorSchemeLoader, ColorSchemeLoader>()
                .AddSingleton<IMessageBoxManager, MessageBoxManager>();
    }
}
