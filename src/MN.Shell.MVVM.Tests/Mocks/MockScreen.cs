namespace MN.Shell.MVVM.Tests.Mocks
{
    public class MockScreen : Screen
    {
        public int OnInitializedCalledCount { get; private set; }

        protected override void OnInitialized() => ++OnInitializedCalledCount;

        public int OnActivatedCalledCount { get; private set; }

        protected override void OnActivated() => ++OnActivatedCalledCount;

        public int OnDeactivatedCalledCount { get; private set; }

        protected override void OnDeactivated() => ++OnDeactivatedCalledCount;

        public int OnClosedCalledCount { get; private set; }

        protected override void OnClosed() => ++OnClosedCalledCount;

        public bool CanBeClosedReturnValue { get; set; } = true;

        public override bool CanBeClosed() => CanBeClosedReturnValue;
    }
}
