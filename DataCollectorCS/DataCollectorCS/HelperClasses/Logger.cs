using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.HelperClasses
{
    class Logger
    {
        private static LogLevel logLevel = LogLevel.LogLevelInfo;

        public enum LogLevel
        {
            LogLevelVerbose,
            LogLevelInfo,
            LogLevelWarn,
            LogLevelError,
            LogLevelCritical
        } // will only print logs below the set log level, if verbose is set everything will be logged

        private static async void Log(LogLevel logLevel, string group, string message)
        {
            string sLogLevel = "";

            
            switch (logLevel)
            {
                case LogLevel.LogLevelVerbose:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    sLogLevel = "VERBOSE";
                    break;
                case LogLevel.LogLevelInfo:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    sLogLevel = "INFO";
                    break;
                case LogLevel.LogLevelWarn:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    sLogLevel = "WARN";
                    break;
                case LogLevel.LogLevelError:
                    Console.ForegroundColor = ConsoleColor.Red;
                    sLogLevel = "ERROR";
                    break;
                case LogLevel.LogLevelCritical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    sLogLevel = "CRITICAL";
                    break;
            }
            if (Logger.logLevel <= logLevel)
                //Console.WriteLine($"[{group}/{sLogLevel}] {message}");
                Console.WriteLine($"[ {group} / {DateTime.Now.ToString("HH:mm:ss")} ]  {message}");
            Console.ResetColor();
        }

        public static async void SetLogLevel(LogLevel logLevel)
        {
            Logger.logLevel = logLevel;
        }

        public static async void Verbose(string group, string message)
        {
            if (Logger.logLevel <= LogLevel.LogLevelVerbose)
                Logger.Log(LogLevel.LogLevelVerbose, group, message);
        }
        public static async void Info(string group, string message)
        {
            if (Logger.logLevel <= LogLevel.LogLevelInfo)
                Logger.Log(LogLevel.LogLevelInfo, group, message);
        }

        public static async void Warn(string group, string message)
        {
            if (Logger.logLevel <= LogLevel.LogLevelWarn)
                Logger.Log(LogLevel.LogLevelWarn, group, message);
        }

        public static async void Error(string group, string message)
        {
            if (Logger.logLevel <= LogLevel.LogLevelError)
                Logger.Log(LogLevel.LogLevelError, group, message);
        }

        public static async void Critical(string group, string message)
        {
            if (Logger.logLevel <= LogLevel.LogLevelCritical)
                Logger.Log(LogLevel.LogLevelCritical, group, message);
        }       
    }
}
