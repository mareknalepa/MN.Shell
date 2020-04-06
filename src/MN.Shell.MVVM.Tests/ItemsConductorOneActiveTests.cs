using System.Linq;
using MN.Shell.MVVM.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class ItemsConductorOneActiveTests
    {
        private Mock<ItemsConductorOneActive<object>> _conductorMock;
        private ItemsConductorOneActive<object> _conductor;

        [SetUp]
        public void SetUp()
        {
            _conductorMock = new Mock<ItemsConductorOneActive<object>>(MockBehavior.Loose) { CallBase = true };
            _conductor = _conductorMock.Object;
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
            var conductor = new MockItemsConductorOneActive();
            var item = new MockScreen();

            Assert.AreEqual(0, conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);

            conductor.ActivateItem(item);

            Assert.AreEqual(0, conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);
        }

        [Test]
        public void ActivateItemWhenConductorIsActiveTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item = new MockScreen();

            Assert.AreEqual(0, conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(0, item.OnActivatedCalledCount);

            conductor.Activate();
            conductor.ActivateItem(item);

            Assert.AreEqual(1, conductor.OnConductorActivatedCalledCount);
            Assert.AreEqual(1, item.OnActivatedCalledCount);
        }

        [Test]
        public void ChangeActiveItemActivateDeactivateTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item1);

            Assert.AreEqual(1, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            conductor.ActivateItem(item2);

            Assert.AreEqual(1, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            conductor.ActiveItem = item1;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);

            conductor.ActiveItem = item2;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(2, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(2, item2.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);

            conductor.ActiveItem = null;

            Assert.AreEqual(2, item1.OnActivatedCalledCount);
            Assert.AreEqual(2, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(2, item2.OnActivatedCalledCount);
            Assert.AreEqual(2, item2.OnDeactivatedCalledCount);
        }

        [Test]
        public void ActivateConductorTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);

            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(0, item2.OnActivatedCalledCount);

            conductor.Activate();

            Assert.AreEqual(0, item1.OnActivatedCalledCount);
            Assert.AreEqual(1, item2.OnActivatedCalledCount);
        }

        [Test]
        public void DeactivateConductorTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);
            conductor.Activate();

            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(0, item2.OnDeactivatedCalledCount);

            conductor.Deactivate();

            Assert.AreEqual(0, item1.OnDeactivatedCalledCount);
            Assert.AreEqual(1, item2.OnDeactivatedCalledCount);
        }

        [Test]
        public void CloseConductorTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);

            Assert.AreEqual(0, item1.OnClosedCalledCount);
            Assert.AreEqual(0, item2.OnClosedCalledCount);

            conductor.Close();

            Assert.AreEqual(1, item1.OnClosedCalledCount);
            Assert.AreEqual(1, item2.OnClosedCalledCount);
        }

        [Test]
        public void NextItemAfterFirstClosedTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);
            conductor.ActivateItem(item3);
            conductor.ActivateItem(item4);

            conductor.ActiveItem = item1;
            conductor.CloseItem(item1);

            Assert.AreEqual(item2, conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterLastClosedTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);
            conductor.ActivateItem(item3);
            conductor.ActivateItem(item4);

            conductor.ActiveItem = item4;
            conductor.CloseItem(item4);

            Assert.AreEqual(item3, conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterMiddleClosedTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();
            var item3 = new MockScreen();
            var item4 = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);
            conductor.ActivateItem(item3);
            conductor.ActivateItem(item4);

            conductor.ActiveItem = item2;
            conductor.CloseItem(item2);

            Assert.AreEqual(item3, conductor.ActiveItem);

            conductor.CloseItem(item3);

            Assert.AreEqual(item4, conductor.ActiveItem);
        }

        [Test]
        public void NextItemAfterOnlyItemClosedTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item = new MockScreen();

            conductor.Activate();
            conductor.ActivateItem(item);

            Assert.AreEqual(item, conductor.ActiveItem);

            conductor.CloseItem(item);

            Assert.Null(conductor.ActiveItem);
        }

        [Test]
        public void CanBeClosedTest()
        {
            var conductor = new MockItemsConductorOneActive();
            var item1 = new MockScreen();
            var item2 = new MockScreen();

            conductor.ActivateItem(item1);
            conductor.ActivateItem(item2);

            conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            Assert.True(conductor.CanBeClosed());

            conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = true;

            Assert.False(conductor.CanBeClosed());

            conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = true;

            Assert.False(conductor.CanBeClosed());

            conductor.CanBeClosedReturnValue = true;
            item1.CanBeClosedReturnValue = true;
            item2.CanBeClosedReturnValue = false;

            Assert.False(conductor.CanBeClosed());

            conductor.CanBeClosedReturnValue = false;
            item1.CanBeClosedReturnValue = false;
            item2.CanBeClosedReturnValue = false;

            Assert.False(conductor.CanBeClosed());
        }
    }
}
