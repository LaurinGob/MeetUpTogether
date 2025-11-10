using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace MeetUpTogether.BLL.Services
{
    public class LoggingService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LoggingService));

        static LoggingService()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configFile = Path.Combine(AppContext.BaseDirectory, "log4net.config");

            if (File.Exists(configFile))
            {
                XmlConfigurator.Configure(logRepository, new FileInfo(configFile));
            }
            else
            {
                Console.WriteLine("⚠️ log4net.config not found, logging will not work.");
            }
        }

        public void Info(string message) => _logger.Info(message);
        public void Warn(string message) => _logger.Warn(message);
        public void Error(string message, Exception ex = null) => _logger.Error(message, ex);
    }
}