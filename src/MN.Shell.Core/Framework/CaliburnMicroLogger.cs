using Caliburn.Micro;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN.Shell.Core.Framework
{
    public class CaliburnMicroLogger : ILog
    {
        private readonly Logger _logger;

        public CaliburnMicroLogger(Type type)
        {
            _logger = NLog.LogManager.GetLogger(type.Name);
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
