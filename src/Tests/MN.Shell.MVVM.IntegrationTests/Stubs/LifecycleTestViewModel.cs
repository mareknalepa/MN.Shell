namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    internal sealed class LifecycleTestViewModel : Screen
    {
        public int ActivateCalledCount { get; private set; }

        protected override void OnActivated() => ++ActivateCalledCount;

        public int DeactivateCalledCount { get; private set; }

        protected override void OnDeactivated() => ++DeactivateCalledCount;

        public int CloseCalledCount { get; private set; }

        protected override void OnClosed() => ++CloseCalledCount;
    }
}
