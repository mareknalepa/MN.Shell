using MN.Shell.MVVM.Tests.Mocks;
using NUnit.Framework;
using System.Linq;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class ItemsConductorOneActiveTests
    {
        private MockItemsConductorOneActive _conductor;

        [SetUp]
        public void SetUp()
        {
            _conductor = new MockItemsConductorOneActive();
        }

        [Test]
        public void ActivateItemAddsItemTest()
        {
            Assert.NotNull(_conductor.Items);
            Assert.IsEmpty(_conductor.Items);
            Assert.Null(_conductor.ActiveItem);

            var item1 = new object();
            _conductor.ActivateItem(item1);

            Assert.That(_conductor.Items, Has.Exactly(1).Items);
            Assert.AreSame(item1, _conductor.Items.First());
            Assert.AreSame(item1, _conductor.ActiveItem);

            var item2 = new object();
            _conductor.ActivateItem(item2);

            Assert.That(_conductor.Items, Has.Exactly(2).Items);
            Assert.AreSame(item1, _conductor.Items.First());
            Assert.AreSame(item2, _conductor.Items.Skip(1).First());
            Assert.AreSame(item2, _conductor.ActiveItem);
        }

        [Test]
        public void CloseItemForNotActiveTest()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            Assert.That(_conductor.Items, Has.Exactly(2).Items);
            Assert.AreSame(item2, _conductor.ActiveItem);

            _conductor.CloseItem(item1);

            Assert.That(_conductor.Items, Has.Exactly(1).Items);
            Assert.AreSame(item2, _conductor.ActiveItem);
        }

        [Test]
        public void CloseItemForActiveTest()
        {
            var item1 = new object();
            _conductor.ActivateItem(item1);
            var item2 = new object();
            _conductor.ActivateItem(item2);

            Assert.That(_conductor.Items, Has.Exactly(2).Items);
            Assert.AreSame(item2, _conductor.ActiveItem);

            _conductor.CloseItem(item2);

            Assert.That(_conductor.Items, Has.Exactly(1).Items);
            Assert.AreSame(item1, _conductor.ActiveItem);
        }

        [Test]
        public void ActivateItemWhenConductorIsInactiveTest()
        {
            var item = new MockScreen();

            Assert.AreEqual(0, _conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);

            _conductor.ActivateItem(item);

            Assert.AreEqual(0, _conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);
        }

        [Test]
        public void ActivateItemWhenConductorIsActiveTest()
        {
            var item = new MockScreen();

            Assert.AreEqual(0, _conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);

            _conductor.Activate();
            _conductor.ActivateItem(item);

            Assert.AreEqual(1, _conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(1, item.OnActivatedCalledCount);
        }

        [Test]
        public void ChangeActiveItemActivateDeactivateTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item1);

            Assert.AreEqual(1, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            _conductor.ActivateItem(item2);

            Assert.AreEqual(1, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            _conductor.ActiveItem = item1;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);

            _conductor.ActiveItem = item2;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(2, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(2, item2.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);

            _conductor.ActiveItem = null;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(2, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(2, item2.OnActivatedCalledCount);
            Assert.AreEqual(2, item2.OnDeactivatedCalledCount);
        }

        [Test]
        public void ActivateConductorTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);

            _conductor.Activate();

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
        }

        [Test]
        public void DeactivateConductorTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.Activate();

            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            _conductor.Deactivate();

            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);
        }

        [Test]
        public void CloseConductorTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            Assert.AreEqual(0, item1.OnClosedCalledCount);
            Assert.AreEqual(0, item2.OnClosedCalledCount);

            _conductor.Close();

            Assert.AreEqual(1, item1.OnClosedCalledCount);
            Assert.AreEqual(1, item2.OnClosedCalledCount);
        }

        [Test]
        public void NextItemAfterFirstClosedTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item1;
            _conductor.CloseItem(item1);

            Assert.AreEqual(item2, _conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterLastClosedTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item4;
            _conductor.CloseItem(item4);

            Assert.AreEqual(item3, _conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterMiddleClosedTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);
            _conductor.ActivateItem(item3);
            _conductor.ActivateItem(item4);

            _conductor.ActiveItem = item2;
            _conductor.CloseItem(item2);

            Assert.AreEqual(item3, _conductor.ActiveItem);

            _conductor.CloseItem(item3);

            Assert.AreEqual(item4, _conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterOnlyItemClosedTest()
        {
            var item = new MockScreen();

            _conductor.Activate();
            _conductor.ActivateItem(item);

            Assert.AreEqual(item, _conductor.ActiveItem);

            _conductor.CloseItem(item);

            Assert.Null(_conductor.ActiveItem);
        }

        [Test]
        public void CanBeClosedTest()
        {
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            _conductor.ActivateItem(item1);
            _conductor.ActivateItem(item2);

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            Assert.True(_conductor.CanBeClosed());

            _conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            Assert.False(_conductor.CanBeClosed());

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = true;

            Assert.False(_conductor.CanBeClosed());

            _conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = false;

            Assert.False(_conductor.CanBeClosed());

            _conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = false;

            Assert.False(_conductor.CanBeClosed());
        }
    }
}
