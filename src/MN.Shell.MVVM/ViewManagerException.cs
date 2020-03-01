using System;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Exception thrown by ViewManager
    /// </summary>
    public class ViewManagerException : Exception
    {
        public ViewManagerException() { }
        public ViewManagerException(string message) : base(message) { }
        public ViewManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
