namespace MN.Shell.MVVM
{
    /// <summary>
    /// Non-generic interface for components which can subscribe to Message Bus for particular message types
    /// </summary>
    public interface IListener { }

    /// <summary>
    /// Generic interface for components which can subscribe to Message Bus for particular message types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListener<T> : IListener
    {
        /// <summary>
        /// Called when message of type T is published in Message Bus
        /// </summary>
        /// <typeparam name="T">Type of message</typeparam>
        /// <param name="message">Message to process</param>
        void Process(T message);
    }
}
