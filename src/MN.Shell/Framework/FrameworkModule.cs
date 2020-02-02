using MN.Shell.Framework.ColorSchemes;
using MN.Shell.Framework.Menu;
using MN.Shell.Framework.MessageBox;
using MN.Shell.Framework.StatusBar;
using Ninject.Modules;

namespace MN.Shell.Framework
{
    public class FrameworkModule : NinjectModule
    {
        public override string Name => "MN.Shell.Framework";

        public override void Load()
        {
            Bind<IColorSchemeLoader, ColorSchemeLoader>().To<ColorSchemeLoader>().InSingletonScope();
            Bind<IMenuAggregator, MenuAggregator>().To<MenuAggregator>().InSingletonScope();
            Bind<IMenuProvider, MainMenuProvider>().To<MainMenuProvider>().InSingletonScope();
            Bind<IMessageBoxManager, MessageBoxManager>().To<MessageBoxManager>().InSingletonScope();
            Bind<IStatusBarAggregator, StatusBarAggregator>().To<StatusBarAggregator>().InSingletonScope();
        }
    }
}
