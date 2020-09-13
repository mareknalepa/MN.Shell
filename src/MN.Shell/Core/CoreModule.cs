using MN.Shell.MVVM;
using Ninject.Modules;

namespace MN.Shell.Core
{
    public class CoreModule : NinjectModule
    {
        public override string Name => "MN.Shell.Core";

        public override void Load()
        {
            Bind<IViewManager, ViewManager>().To<ViewManager>().InSingletonScope();
            Bind<IWindowManager, WindowManager>().To<WindowManager>().InSingletonScope();
            Bind<IMessageBus, MessageBus>().To<MessageBus>().InSingletonScope();
        }
    }
}
