using System;
using NLog;

namespace DimitriVranken.PanoramaCreator
{
    static class Logger
    {
        private static readonly NLog.Logger _default = LogManager.GetLogger("Default");
        private static readonly NLog.Logger _userInterface = LogManager.GetLogger("UserInterface");


        public static NLog.Logger Default
        {
            get { return _default; }
        }

        public static NLog.Logger UserInterface
        {
            get { return _userInterface; }
        }


        public static void UpdateLogLevel(string loggerPattern, LogLevel logLevel)
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                if (rule.NameMatches(loggerPattern))
                {
                    rule.EnableLoggingForLevel(logLevel);
                }
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void UpdateLogLevel(this NLog.Logger logger, LogLevel logLevel)
        {
            UpdateLogLevel(logger.Name, logLevel);
        }
    }
}
