using Caliburn.Micro;
using Ninject.Modules;

namespace MN.Shell.Core
{
    public class CoreModule : NinjectModule
    {
        public override string Name => "MN.Shell.Core";

        public override void Load()
        {
            Bind<ILog>().ToMethod(c => new Logger(NLog.LogManager.GetLogger(
                c.Request.Target.Member.DeclaringType.Name)));

            Bind<IWindowManager>().To<WindowManager>().InSingletonScope();
            Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
        }
    }
}
