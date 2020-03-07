namespace MN.Shell.MVVM
{
    /// <summary>
    /// MessageBus allows various components to subscribe to particular message types as well as to publish them
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Subscribe given listener to particular type of messages
        /// </summary>
        /// <typeparam name="T">Type of messages</typeparam>
        /// <param name="listener">Listener to subscribe</param>
        void Subscribe<T>(IListener<T> listener);

        /// <summary>
        /// Unsubscribe given listener from particular type of messages
        /// </summary>
        /// <typeparam name="T">Type of messages</typeparam>
        /// <param name="listener">Listener to unsubscribe</param>
        void Unsubscribe<T>(IListener<T> listener);

        /// <summary>
        /// Publish given message on MessageBus and notify all relevant subscribed listeners
        /// </summary>
        /// <typeparam name="T">Type of message</typeparam>
        /// <param name="message">Message to publish</param>
        void Publish<T>(T message);
    }
}
