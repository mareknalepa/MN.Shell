using MN.Shell.MVVM.UnitTests.Stubs;

namespace MN.Shell.MVVM.UnitTests
{
    public sealed class ItemsConductorOneActiveTests
    {
        private readonly ItemsConductorOneActiveStub _conductor = new();

        [Fact]
        public void ActivateItem_AddsItem()
        {
            _conductor.Items.ShouldNotBeNull();
            _conductor.Items.ShouldBeEmpty();
            _conductor.ActiveItem.ShouldBeNull();

            var item1 = new object();
            _conductor.ActivateItem(item1);

            _conductor.Items.ShouldHaveSingleItem();
            _conductor.Items.First().ShouldBeSameAs(item1);
            _conductor.ActiveItem.ShouldBeSameAs(item1);

            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);
            _conductor.Items.First().ShouldBeSameAs(item1);
            _conductor.Items.Skip(1).First().ShouldBeSameAs(item2);
            _conductor.ActiveItem.ShouldBeSameAs(item2);
        }

        [Fact]
        public void CloseItem_ForNotActive_RemovesIt()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);
            _conductor.ActiveItem.ShouldBeSameAs(item2);

            _conductor.CloseItem(item1);

            _conductor.Items.ShouldHaveSingleItem();
            _conductor.ActiveItem.ShouldBeSameAs(item2);
        }

        [Fact]
        public void CloseItem_ForActive_RemovesIt()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            _conductor.Items.Count().ShouldBe(2);
            _conductor.ActiveItem.ShouldBeSameAs(item2);

            _conductor.CloseItem(item2);

            _conductor.Items.ShouldHaveSingleItem();
            _conductor.ActiveItem.ShouldBeSameAs(item1);
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
        public void ActivateItem_DeactivatesPreviousAndActivatesCurrent()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item1);

            item1.OnActivatedCalledCount.ShouldBe(1);
            item1.OnDeactivatedCalledCount.ShouldBe(0);
            item2.OnActivatedCalledCount.ShouldBe(0);
            item2.OnDeactivatedCalledCount.ShouldBe(0);

            _conductor.ActivateItem(item2);

            item1.OnActivatedCalledCount.ShouldBe(1);
            item1.OnDeactivatedCalledCount.ShouldBe(1);
            item2.OnActivatedCalledCount.ShouldBe(1);
            item2.OnDeactivatedCalledCount.ShouldBe(0);

            _conductor.ActiveItem = item1;

            item1.OnActivatedCalledCount.ShouldBe(2);
            item1.OnDeactivatedCalledCount.ShouldBe(1);
            item2.OnActivatedCalledCount.ShouldBe(1);
            item2.OnDeactivatedCalledCount.ShouldBe(1);

            _conductor.ActiveItem = item2;

            item1.OnActivatedCalledCount.ShouldBe(2);
            item1.OnDeactivatedCalledCount.ShouldBe(2);
            item2.OnActivatedCalledCount.ShouldBe(2);
            item2.OnDeactivatedCalledCount.ShouldBe(1);

            _conductor.ActiveItem = null;

            item1.OnActivatedCalledCount.ShouldBe(2);
            item1.OnDeactivatedCalledCount.ShouldBe(2);
            item2.OnActivatedCalledCount.ShouldBe(2);
            item2.OnDeactivatedCalledCount.ShouldBe(2);
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

            item1.OnActivatedCalledCount.ShouldBe(0);
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

            item1.OnDeactivatedCalledCount.ShouldBe(0);
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
        public void CloseItem_ActivatesNextOneAfterFirstOne()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();
            var item3 = new ScreenStub();
            var item4 = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item1;
            _conductor.CloseItem(item1);

            _conductor.ActiveItem.ShouldBeSameAs(item2);
        }

        [Fact]
        public void CloseItem_ActivatesPreviousOneBeforeLastOne()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();
            var item3 = new ScreenStub();
            var item4 = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item4;
            _conductor.CloseItem(item4);

            _conductor.ActiveItem.ShouldBeSameAs(item3);
        }

        [Fact]
        public void CloseItem_ActivatesNextOneAfterMiddleOne()
        {
            var item1 = new ScreenStub();
            var item2 = new ScreenStub();
            var item3 = new ScreenStub();
            var item4 = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item2;
            _conductor.CloseItem(item2);

            _conductor.ActiveItem.ShouldBeSameAs(item3);

            _conductor.CloseItem(item3);

            _conductor.ActiveItem.ShouldBeSameAs(item4);
        }

        [Fact]
        public void CloseItem_ActivatesNothingAfterClosingTheOnlyOne()
        {
            var item = new ScreenStub();

            _conductor.Activate();
            _conductor.ActivateItem(item);

            _conductor.ActiveItem.ShouldBeSameAs(item);

            _conductor.CloseItem(item);

            _conductor.ActiveItem.ShouldBeNull();
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
