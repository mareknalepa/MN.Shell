using Caliburn.Micro;
using MN.Shell.Core.MainWindow;
using Ninject.Modules;

namespace MN.Shell.Core.Framework
{
    public class CoreModule : NinjectModule
    {
        public override string Name => "MN.Shell.Core";

        public override void Load()
        {
            Bind<ILog>().ToMethod(c => new AppLogger(NLog.LogManager.GetLogger(
                c.Request.Target.Member.DeclaringType.Name)));

            Bind<IWindowManager>().To<AppWindowManager>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();

            Bind<IShell>().To<MainWindowViewModel>().InSingletonScope();
        }
    }
}
