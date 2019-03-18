using System;

namespace MN.Shell.Framework.Menu
{
    public class InconsistentMenuException : Exception
    {
        public InconsistentMenuException()
        { }

        public InconsistentMenuException(string message) : base(message)
        { }

        public InconsistentMenuException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
