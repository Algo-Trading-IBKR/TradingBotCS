﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.Messaging;

namespace TradingBotCS.Util
{
    public interface Logger
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
            try
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
                        await LogRepository.InsertLog(group, message, sLogLevel);
                        break;
                    case LogLevel.LogLevelCritical:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        sLogLevel = "CRITICAL";
                        //MobileService.SendTextMsg($"[ {group} / {DateTime.Now.ToString("HH:mm:ss")} ]  {message}", Program.PhoneNumbers);
                        await LogRepository.InsertLog(group, message, sLogLevel);
                        break;
                }
                if (Logger.logLevel <= logLevel)
                    //Console.WriteLine($"[{group}/{sLogLevel}] {message}");
                    Console.WriteLine($"[ {group} / {DateTime.Now.ToString("HH:mm:ss")} / {sLogLevel}]  {message}");
                Thread.Sleep(1);
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}]  {e}");
                Console.ResetColor();
            }
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
