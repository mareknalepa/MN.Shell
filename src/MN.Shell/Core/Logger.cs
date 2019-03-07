using Caliburn.Micro;
using System;

namespace MN.Shell.Core
{
    public class Logger : ILog
    {
        private readonly NLog.Logger _logger;

        public Logger(NLog.Logger logger)
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
