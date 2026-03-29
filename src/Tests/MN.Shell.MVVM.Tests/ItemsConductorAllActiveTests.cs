using MN.Shell.MVVM.Tests.Mocks;

namespace MN.Shell.MVVM.Tests
{
    public sealed class ItemsConductorAllActiveTests
    {
        private readonly ItemsConductorAllActiveStub _conductor = new();

        [Fact]
        public void ActivateItem_AddsItem()
        {
            _conductor.Items.ShouldNotBeNull();
            _conductor.Items.ShouldBeEmpty();

            var item1 = new object();
            _conductor.ActivateItem(item1);

            _conductor.Items.ShouldHaveSingleItem();
            _conductor.Items.First().ShouldBeSameAs(item1);

            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);
            _conductor.Items.First().ShouldBeSameAs(item1);
            _conductor.Items.Skip(1).First().ShouldBeSameAs(item2);
        }

        [Fact]
        public void CloseItem_ForNotActive_RemovesIt()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);

            _conductor.CloseItem(item1);

            _conductor.Items.ShouldHaveSingleItem();
        }

        [Fact]
        public void CloseItem_ForActive_RemovesIt()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);

            _conductor.CloseItem(item2);

            _conductor.Items.ShouldHaveSingleItem();
        }

        [Fact]
        public void ActivateItem_WhenConductorIsInactive_DoesNotActivateItem()
        {
            var item = new ScreenStub();

            _conductor.OnConductorActivatedCalledCount.ShouldBe(0);
            item.OnActivatedCalledCount.ShouldBe(0);

            _conductor.ActivateItem(item);

            _conductor.OnConductorActivatedCalledCount.ShouldBe(0);
            item.OnActivatedCalledCount.ShouldBe(0);
        }

        [Fact]
        public void ActivateItem_WhenConductorIsActive_ActivatesItem()
        {
            var item = new ScreenStub();

            _conductor.OnConductorActivatedCalledCount.ShouldBe(0);
            item.OnActivatedCalledCount.ShouldBe(0);

            _conductor.Activate();
            _conductor.ActivateItem(item);

            _conductor.OnConductorActivatedCalledCount.ShouldBe(1);
            item.OnActivatedCalledCount.ShouldBe(1);
        }

        [Fact]
        public void Activate_ActivatesItems()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();

            item1.OnActivatedCalledCount.ShouldBe(0);
            item2.OnActivatedCalledCount.ShouldBe(0);

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            item1.OnActivatedCalledCount.ShouldBe(0);
            item2.OnActivatedCalledCount.ShouldBe(0);

            _conductor.Activate();

            item1.OnActivatedCalledCount.ShouldBe(1);
            item2.OnActivatedCalledCount.ShouldBe(1);
        }

        [Fact]
        public void Deactivate_DeactivatesItems()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.Activate();

            item1.OnDeactivatedCalledCount.ShouldBe(0);
            item2.OnDeactivatedCalledCount.ShouldBe(0);

            _conductor.Deactivate();

            item1.OnDeactivatedCalledCount.ShouldBe(1);
            item2.OnDeactivatedCalledCount.ShouldBe(1);
        }

        [Fact]
        public void Close_ClosesItems()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            item1.OnClosedCalledCount.ShouldBe(0);
            item2.OnClosedCalledCount.ShouldBe(0);

            _conductor.Close();

            item1.OnClosedCalledCount.ShouldBe(1);
            item2.OnClosedCalledCount.ShouldBe(1);
        }

        [Fact]
        public void CanBeClosed_PropagatesItemsCanBeClosed()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            _conductor.CanBeClosed().ShouldBeTrue();

            _conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            _conductor.CanBeClosed().ShouldBeFalse();

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = true;

            _conductor.CanBeClosed().ShouldBeFalse();

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = false;

            _conductor.CanBeClosed().ShouldBeFalse();

            _conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = false;

            _conductor.CanBeClosed().ShouldBeFalse();
        }
    }
}
