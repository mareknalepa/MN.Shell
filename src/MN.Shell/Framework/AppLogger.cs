using Caliburn.Micro;
using NLog;
using System;

namespace MN.Shell.Framework
{
    public class AppLogger : ILog
    {
        private readonly Logger _logger;

        public AppLogger(Logger logger)
        {
            _logger = logger;
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception, exception.Message);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }
    }
}
