using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// MessageBus allows various components to subscribe to particular message types as well as to publish them
    /// </summary>
    public class MessageBus : IMessageBus
    {
        private class MessageBusListener
        {
            public MessageBusListener(IListener listener)
            {
                Listener = new WeakReference<IListener>(listener);
            }

            public WeakReference<IListener> Listener { get; }
            public List<MessageBusHandler> Handlers { get; } = new List<MessageBusHandler>();
        }

        private class MessageBusHandler
        {
            public MessageBusHandler(Type messageType)
            {
                MessageType = messageType;

                var targetInterface = typeof(IListener<>).MakeGenericType(new[] { MessageType });

                var method = targetInterface
                    .GetMethod(nameof(IListener<object>.Process));

                var target = Expression.Parameter(typeof(object));
                var message = Expression.Parameter(typeof(object));

                Invoker = Expression
                    .Lambda<Action<object, object>>(Expression.Call(
                        Expression.Convert(target, targetInterface),
                        method,
                        Expression.Convert(message, MessageType)), target, message)
                    .Compile();
            }

            public Type MessageType { get; }
            public Action<object, object> Invoker { get; }
        }

        private readonly List<MessageBusListener> _listeners = new();

        private bool _inHandler;

        private readonly Queue<Tuple<IListener, Type>> _listenersToAdd = new();

        private readonly Queue<Tuple<IListener, Type>> _listenersToRemove = new();

        /// <summary>
        /// Subscribe given listener to particular type of messages
        /// </summary>
        /// <typeparam name="T">Type of messages</typeparam>
        /// <param name="listener">Listener to subscribe</param>
        public void Subscribe<T>(IListener<T> listener)
        {
            if (_inHandler)
                _listenersToAdd.Enqueue(Tuple.Create(listener as IListener, typeof(T)));
            else
                InternalSubscribe(listener, typeof(T));
        }

        private void InternalSubscribe(IListener listener, Type messageType)
        {
            var internalListener = _listeners
                .FirstOrDefault(l => l.Listener.TryGetTarget(out var target) && ReferenceEquals(target, listener));

            if (internalListener != null)
            {
                if (internalListener.Handlers.Any(h => h.MessageType == messageType))
                    return;
            }
            else
            {
                internalListener = new MessageBusListener(listener);
                _listeners.Add(internalListener);
            }

            internalListener.Handlers.Add(new MessageBusHandler(messageType));
        }

        /// <summary>
        /// Unsubscribe given listener from particular type of messages
        /// </summary>
        /// <typeparam name="T">Type of messages</typeparam>
        /// <param name="listener">Listener to unsubscribe</param>
        public void Unsubscribe<T>(IListener<T> listener)
        {
            if (_inHandler)
                _listenersToRemove.Enqueue(Tuple.Create(listener as IListener, typeof(T)));
            else
                InternalUnsubscribe(listener, typeof(T));
        }

        private void InternalUnsubscribe(IListener listener, Type messageType)
        {
            _listeners.RemoveAll(l => !l.Listener.TryGetTarget(out var target));

            var internalListener = _listeners
                .FirstOrDefault(l => l.Listener.TryGetTarget(out var target) && ReferenceEquals(target, listener));

            if (internalListener != null)
            {
                internalListener.Handlers.RemoveAll(h => h.MessageType == messageType);
                if (!internalListener.Handlers.Any())
                    _listeners.Remove(internalListener);
            }
        }

        /// <summary>
        /// Publish given message on MessageBus and notify all relevant subscribed listeners
        /// </summary>
        /// <typeparam name="T">Type of message</typeparam>
        /// <param name="message">Message to publish</param>
        public void Publish<T>(T message)
        {
            if (_inHandler)
                return;
            try
            {
                _inHandler = true;

                foreach (var listener in _listeners.ToList())
                {
                    if (!listener.Listener.TryGetTarget(out var target))
                        _listeners.Remove(listener);
                    else
                    {
                        listener.Handlers.ForEach(h =>
                        {
                            if (h.MessageType.IsAssignableFrom(typeof(T)))
                                h.Invoker.Invoke(target, message);
                        });
                    }
                }
            }
            finally
            {
                _inHandler = false;

                while (_listenersToAdd.Count > 0)
                {
                    var listenerToAdd = _listenersToAdd.Dequeue();
                    InternalSubscribe(listenerToAdd.Item1, listenerToAdd.Item2);
                }

                while (_listenersToRemove.Count > 0)
                {
                    var listenerToRemove = _listenersToRemove.Dequeue();
                    InternalUnsubscribe(listenerToRemove.Item1, listenerToRemove.Item2);
                }
            }
        }
    }
}
