namespace MN.Shell.MVVM.UnitTests
{
    public sealed class MessageBusTests
    {
        private readonly MessageBus _messageBus = new();

        [Fact]
        public void Publish_DoesNothing_WhenNoListenersSubscribed()
        {
            var message = new Message1();

            var act = () => _messageBus.Publish(message);

            act.ShouldNotThrow();
        }

        [Fact]
        public void Publish_NotifiesSubscribedListeners()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);

            var message = new Message1();
            _messageBus.Publish(message);

            listener.ProcessedMessages.ShouldContain(message);
        }

        [Fact]
        public void Unsubscribe_StopsReceivingMessages()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);

            _messageBus.Subscribe(listener);

            var message = new Message1();
            _messageBus.Publish(message);

            listener.ProcessedMessages.ShouldContain(message);

            _messageBus.Unsubscribe(listener);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            listener.ProcessedMessages.ShouldNotContain(anotherMessage);
        }

        [Fact]
        public void Publish_NotifiesMatchingListeners()
        {
            var listener1 = new Listener1();
            _messageBus.Subscribe(listener1);

            var listener2 = new Listener2();
            _messageBus.Subscribe(listener2);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            var message2 = new Message2();
            _messageBus.Publish(message2);

            listener1.ProcessedMessages.ShouldContain(message1);
            listener1.ProcessedMessages.ShouldContain(message2);

            listener2.ProcessedMessages.ShouldNotContain(message1);
            listener2.ProcessedMessages.ShouldContain(message2);
        }

        [Fact]
        public void Unsubscribe_StopsReceivingBaseMessages()
        {
            var listener = new Listener3();
            _messageBus.Subscribe<Message1>(listener);
            _messageBus.Subscribe<Message2>(listener);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages2.ShouldBeEmpty();

            var message2 = new Message2();
            _messageBus.Publish(message2);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages2.ShouldContain(message2);

            _messageBus.Unsubscribe<Message1>(listener);

            var anotherMessage1 = new Message1();
            _messageBus.Publish(anotherMessage1);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages1.ShouldNotContain(anotherMessage1);
            listener.ProcessedMessages2.ShouldContain(message2);

            var anotherMessage2 = new Message2();
            _messageBus.Publish(anotherMessage2);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages1.ShouldNotContain(anotherMessage1);
            listener.ProcessedMessages2.ShouldContain(message2);
            listener.ProcessedMessages2.ShouldContain(anotherMessage2);
        }

        [Fact]
        public void Unsubscribe_StopsReceivingDerivedMessages()
        {
            var listener = new Listener3();
            _messageBus.Subscribe<Message1>(listener);
            _messageBus.Subscribe<Message2>(listener);

            var message1 = new Message1();
            _messageBus.Publish(message1);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages2.ShouldBeEmpty();

            var message2 = new Message2();
            _messageBus.Publish(message2);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages2.ShouldContain(message2);

            _messageBus.Unsubscribe<Message2>(listener);

            var anotherMessage1 = new Message1();
            _messageBus.Publish(anotherMessage1);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages1.ShouldContain(anotherMessage1);
            listener.ProcessedMessages2.ShouldContain(message2);

            var anotherMessage2 = new Message2();
            _messageBus.Publish(anotherMessage2);

            listener.ProcessedMessages1.ShouldContain(message1);
            listener.ProcessedMessages1.ShouldContain(message2);
            listener.ProcessedMessages1.ShouldContain(anotherMessage1);
            listener.ProcessedMessages1.ShouldContain(anotherMessage2);
            listener.ProcessedMessages2.ShouldContain(message2);
            listener.ProcessedMessages2.ShouldNotContain(anotherMessage2);
        }

        [Fact]
        public void SubscribeUsesWeakReferencesInternally()
        {
            WeakReference<Listener1> CreateWeakReferenceToListener()
            {
                var listener = new Listener1();
                _messageBus.Subscribe(listener);
                return new WeakReference<Listener1>(listener);
            }

            var reference = CreateWeakReferenceToListener();

            int tries = 0;
            do
            {
                GC.Collect();
                ++tries;
            } while (reference.TryGetTarget(out _) && tries < 10);

            if (reference.TryGetTarget(out _))
            {
                throw new InvalidOperationException("GC didn't collect the reference");
            }

            var act = () => _messageBus.Publish(new Message1());
            act.ShouldNotThrow();

            if (reference.TryGetTarget(out _))
            {
                throw new InvalidOperationException("GC didn't collect the reference");
            }
        }

        [Fact]
        public void Subscribe_DoesNothingOnSecondCall()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);
            _messageBus.Subscribe(listener);

            var message = new Message1();
            _messageBus.Publish(message);

            listener.ProcessedMessages.ShouldContain(message);
        }

        [Fact]
        public void Unsubscribe_DoesNothingOnSecondCall()
        {
            var listener = new Listener1();
            _messageBus.Subscribe(listener);
            _messageBus.Unsubscribe(listener);
            _messageBus.Unsubscribe(listener);

            _messageBus.Publish(new Message1());

            listener.ProcessedMessages.ShouldBeEmpty();
        }

        [Fact]
        public void Subscribe_CanBeCalledFromHandler()
        {
            var listener = new Listener4(_messageBus);
            _messageBus.Subscribe(listener);

            var message = new Message1();
            var act = () => _messageBus.Publish(message);
            act.ShouldNotThrow();

            listener.ProcessedMessages.ShouldContain(message);
            listener.InnerListener.ShouldNotBeNull();
            listener.InnerListener.ProcessedMessages.ShouldBeEmpty();

            _messageBus.Unsubscribe(listener);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            listener.ProcessedMessages.ShouldContain(message);
            listener.ProcessedMessages.ShouldNotContain(anotherMessage);
            listener.InnerListener.ProcessedMessages.ShouldNotContain(message);
            listener.InnerListener.ProcessedMessages.ShouldContain(anotherMessage);
        }

        [Fact]
        public void Unsubscribe_CanBeCalledFromHandler()
        {
            var listener = new Listener5(_messageBus);
            _messageBus.Subscribe(listener);

            var message = new Message1();
            var act = () => _messageBus.Publish(message);
            act.ShouldNotThrow();

            listener.ProcessedMessages.ShouldContain(message);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            listener.ProcessedMessages.ShouldContain(message);
            listener.ProcessedMessages.ShouldNotContain(anotherMessage);
        }

        [Fact]
        public void Publish_IsReentrant()
        {
            var listener = new Listener6(_messageBus);
            _messageBus.Subscribe(listener);

            var anotherListener = new Listener1();
            _messageBus.Subscribe(anotherListener);

            var message = new Message1();
            var act = () => _messageBus.Publish(message);
            act.ShouldNotThrow();

            listener.ProcessedMessages.ShouldContain(message);
            anotherListener.ProcessedMessages.ShouldContain(message);

            var anotherMessage = new Message1();
            _messageBus.Publish(anotherMessage);

            listener.ProcessedMessages.ShouldContain(message);
            listener.ProcessedMessages.ShouldContain(anotherMessage);
            anotherListener.ProcessedMessages.ShouldContain(message);
            anotherListener.ProcessedMessages.ShouldContain(anotherMessage);
        }
    }

    internal class Message1 { }

    internal class Message2 : Message1 { }

    internal class Listener1 : IListener<Message1>
    {
        public List<Message1> ProcessedMessages { get; } = [];
        public void Process(Message1 message) => ProcessedMessages.Add(message);
    }

    internal class Listener2 : IListener<Message2>
    {
        public List<Message2> ProcessedMessages { get; } = [];
        public void Process(Message2 message) => ProcessedMessages.Add(message);
    }

    internal class Listener3 : IListener<Message1>, IListener<Message2>
    {
        public List<Message1> ProcessedMessages1 { get; } = [];
        public List<Message1> ProcessedMessages2 { get; } = [];
        public void Process(Message1 message) => ProcessedMessages1.Add(message);
        public void Process(Message2 message) => ProcessedMessages2.Add(message);
    }

    internal class Listener4(IMessageBus messageBus) : IListener<Message1>
    {
        public List<Message1> ProcessedMessages { get; } = [];

        public Listener1? InnerListener { get; private set; }

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            InnerListener = new Listener1();
            messageBus.Subscribe(InnerListener);
        }
    }

    internal class Listener5(IMessageBus messageBus) : IListener<Message1>
    {
        public List<Message1> ProcessedMessages { get; } = [];

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            messageBus.Unsubscribe(this);
        }
    }

    internal class Listener6(IMessageBus messageBus) : IListener<Message1>
    {
        public List<Message1> ProcessedMessages { get; } = [];

        public void Process(Message1 message)
        {
            ProcessedMessages.Add(message);
            messageBus.Publish(new Message1());
        }
    }
}
