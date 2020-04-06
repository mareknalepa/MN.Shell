namespace MN.Shell.MVVM.Tests.Mocks
{
    public class MockItemsConductorAllActive : ItemsConductorAllActive<object>
    {
        public int OnConductorActivatedCalledCount { get; private set; }

        protected override void OnConductorActivated() => ++OnConductorActivatedCalledCount;

        public int OnConductorDeactivatedCalledCount { get; private set; }

        protected override void OnConductorDeactivated() => ++OnConductorDeactivatedCalledCount;

        public int OnConductorClosedCalledCount { get; private set; }

        protected override void OnConductorClosed() => ++OnConductorClosedCalledCount;

        public bool CanBeClosedReturnValue { get; set; } = true;

        protected override bool ConductorCanBeClosed() => CanBeClosedReturnValue;
    }
}
