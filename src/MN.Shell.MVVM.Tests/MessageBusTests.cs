using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MN.Shell.MVVM.Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        private MessageBus _messageBus;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBus();
        }

        [Test]
        public void PublishWithNoListenersTest()
        {
            var message = new Message1();

            Assert.DoesNotThrow(() => _messageBus.Publish(message));
        }

        [Test]
        public void SubscribePublishBasicTest()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);

            var message = new Message1();
            _messageBus.Publish(message);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
        }

        [Test]
        public void UnsubscribeBasicTest()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);

            _messageBus.Subscribe(listener);

            var message = new Message1();
            _messageBus.Publish(message);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);

            _messageBus.Unsubscribe(listener);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
        }

        [Test]
        public void SubscribePublishMessagesInheritanceTest()
        {
            var listener1 = new Listener1();
            _messageBus.Subscribe(listener1);

            var listener2 = new Listener2();
            _messageBus.Subscribe(listener2);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            var message2 = new Message2();
            _messageBus.Publish(message2);

            Assert.That(listener1.ProcessedMessages, Has.Exactly(2).Items);
            Assert.AreSame(message1, listener1.ProcessedMessages[0]);
            Assert.AreSame(message2, listener1.ProcessedMessages[1]);

            Assert.That(listener2.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener2.ProcessedMessages[0]);
        }

        [Test]
        public void SubscribeUnsubscribeBaseClassListenerTest()
        {
            var listener = new Listener3();
            _messageBus.Subscribe<Message1>(listener);
            _messageBus.Subscribe<Message2>(listener);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(1).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.IsEmpty(listener.ProcessedMessages2);

            var message2 = new Message2();
            _messageBus.Publish(message2);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(2).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);

            _messageBus.Unsubscribe<Message1>(listener);

            var anotherMessage1 = new Message1();
            _messageBus.Publish(anotherMessage1);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(2).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);

            var anotherMessage2 = new Message2();
            _messageBus.Publish(anotherMessage2);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(2).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(2).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);
            Assert.AreSame(anotherMessage2, listener.ProcessedMessages2[1]);
        }

        [Test]
        public void SubscribeUnsubscribeDerivedClassListenerTest()
        {
            var listener = new Listener3();
            _messageBus.Subscribe<Message1>(listener);
            _messageBus.Subscribe<Message2>(listener);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(1).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.IsEmpty(listener.ProcessedMessages2);

            var message2 = new Message2();
            _messageBus.Publish(message2);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(2).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);

            _messageBus.Unsubscribe<Message2>(listener);

            var anotherMessage1 = new Message1();
            _messageBus.Publish(anotherMessage1);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(3).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.AreSame(anotherMessage1, listener.ProcessedMessages1[2]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);

            var anotherMessage2 = new Message2();
            _messageBus.Publish(anotherMessage2);

            Assert.That(listener.ProcessedMessages1, Has.Exactly(4).Items);
            Assert.AreSame(message1, listener.ProcessedMessages1[0]);
            Assert.AreSame(message2, listener.ProcessedMessages1[1]);
            Assert.AreSame(anotherMessage1, listener.ProcessedMessages1[2]);
            Assert.AreSame(anotherMessage2, listener.ProcessedMessages1[3]);
            Assert.That(listener.ProcessedMessages2, Has.Exactly(1).Items);
            Assert.AreSame(message2, listener.ProcessedMessages2[0]);
        }

        [Test]
        public void SubscribePublishGCTest()
        {
            WeakReference<Listener1> CreateWeakReferenceToListener()
            {
                var listener = new Listener1();
                _messageBus.Subscribe(listener);
                return new WeakReference<Listener1>(listener);
            }

            var reference = CreateWeakReferenceToListener();

            GC.Collect();

            if (reference.TryGetTarget(out _))
                Assert.Fail();

            Assert.DoesNotThrow(() => _messageBus.Publish(new Message1()));

            if (reference.TryGetTarget(out _))
                Assert.Fail();
        }

        [Test]
        public void DoubleSubscribeTest()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);

            Assert.DoesNotThrow(() => _messageBus.Subscribe(listener));

            var message = new Message1();
            _messageBus.Publish(message);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
        }

        [Test]
        public void DoubleUnsubscribeTest()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);
            _messageBus.Unsubscribe(listener);

            Assert.DoesNotThrow(() => _messageBus.Unsubscribe(listener));

            _messageBus.Publish(new Message1());

            Assert.IsEmpty(listener.ProcessedMessages);
        }

        [Test]
        public void SubscribeFromHandlerTest()
        {
            var listener = new Listener4(_messageBus);
            _messageBus.Subscribe(listener);

            var message = new Message1();
            Assert.DoesNotThrow(() => _messageBus.Publish(message));

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
            Assert.NotNull(listener.InnerListener);
            Assert.IsEmpty(listener.InnerListener.ProcessedMessages);

            _messageBus.Unsubscribe(listener);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
            Assert.That(listener.InnerListener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(anotherMessage, listener.InnerListener.ProcessedMessages[0]);
        }

        [Test]
        public void UnsubscribeFromHandlerTest()
        {
            var listener = new Listener5(_messageBus);
            _messageBus.Subscribe(listener);

            var message = new Message1();
            Assert.DoesNotThrow(() => _messageBus.Publish(message));

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
        }

        [Test]
        public void PublishFromHandlerNotRecursiveTest()
        {
            var listener = new Listener6(_messageBus);
            _messageBus.Subscribe(listener);

            var anotherListener = new Listener1();
            _messageBus.Subscribe(anotherListener);

            var message = new Message1();
            Assert.DoesNotThrow(() => _messageBus.Publish(message));

            Assert.That(listener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);

            Assert.That(anotherListener.ProcessedMessages, Has.Exactly(1).Items);
            Assert.AreSame(message, anotherListener.ProcessedMessages[0]);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            Assert.That(listener.ProcessedMessages, Has.Exactly(2).Items);
            Assert.AreSame(message, listener.ProcessedMessages[0]);
            Assert.AreSame(anotherMessage, listener.ProcessedMessages[1]);

            Assert.That(anotherListener.ProcessedMessages, Has.Exactly(2).Items);
            Assert.AreSame(message, anotherListener.ProcessedMessages[0]);
            Assert.AreSame(anotherMessage, anotherListener.ProcessedMessages[1]);
        }
    }

    class Message1 { }

    class Message2 : Message1 { }

    class Listener1 : IListener<Message1>
    {
        public List<Message1> ProcessedMessages { get; } = new List<Message1>();
        public void Process(Message1 message) => ProcessedMessages.Add(message);
    }

    class Listener2 : IListener<Message2>
    {
        public List<Message2> ProcessedMessages { get; } = new List<Message2>();
        public void Process(Message2 message) => ProcessedMessages.Add(message);
    }

    class Listener3 : IListener<Message1>, IListener<Message2>
    {
        public List<Message1> ProcessedMessages1 { get; } = new List<Message1>();
        public List<Message1> ProcessedMessages2 { get; } = new List<Message1>();
        public void Process(Message1 message) => ProcessedMessages1.Add(message);
        public void Process(Message2 message) => ProcessedMessages2.Add(message);
    }

    class Listener4 : IListener<Message1>
    {
        private readonly IMessageBus _messageBus;

        public Listener4(IMessageBus messageBus) => _messageBus = messageBus;

        public List<Message1> ProcessedMessages { get; } = new List<Message1>();

        public Listener1 InnerListener { get; private set; }

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            InnerListener = new Listener1();
            _messageBus.Subscribe(InnerListener);
        }
    }

    class Listener5 : IListener<Message1>
    {
        private readonly IMessageBus _messageBus;

        public Listener5(IMessageBus messageBus) => _messageBus = messageBus;

        public List<Message1> ProcessedMessages { get; } = new List<Message1>();

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            _messageBus.Unsubscribe(this);
        }
    }

    class Listener6 : IListener<Message1>
    {
        private readonly IMessageBus _messageBus;

        public Listener6(IMessageBus messageBus) => _messageBus = messageBus;

        public List<Message1> ProcessedMessages { get; } = new List<Message1>();

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            _messageBus.Publish(new Message1());
        }
    }
}
