using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.Tactic.Logging
{
    public static class LoggerFacade
    {
        static LoggerFacade()
        {
            Logger = new DebugLogger();
        }

        public static ILogger Logger
        { get; set; }

        public static void Log(string message, Category category, Priority priority)
        {
            var logger = Logger;
            if (logger != null)
                logger.Log(message, category, priority);
        }

        public static void Log(string message, Category category)
        {
            Log(message, category, Priority.None);
        }

        public static void LogInfo(string message, Priority priority)
        {
            Log(message, Category.Info, priority);
        }

        public static void LogInfo(string message)
        {
            Log(message, Category.Info);
        }

        public static void LogDebug(string message, Priority priority)
        {
            Log(message, Category.Debug, priority);
        }

        public static void LogDebug(string message)
        {
            Log(message, Category.Debug);
        }

        public static void LogWarn(string message, Priority priority)
        {
            Log(message, Category.Warn, priority);
        }

        public static void LogWarn(string message)
        {
            Log(message, Category.Warn);
        }

        public static void LogException(string message, Priority priority)
        {
            Log(message, Category.Exception, priority);
        }

        public static void LogException(string message)
        {
            Log(message, Category.Exception);
        }

        public static void LogException(Exception exception, Priority priority)
        {
            LogException(FormatException(exception), priority);
        }

        public static void LogException(Exception exception)
        {
            LogException(exception, Priority.None);
        }

        private static string FormatException(Exception exception)
        {
            return exception != null ? exception.ToString() : string.Empty;
        }
    }
}
