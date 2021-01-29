using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS
{
    public static class Constants
    {
        // API
        public static string Ip = "127.0.0.1";
        public static int Port = 4002;
        public static int ApiId = 1;

        // Strategy
        public static int StartingHour = 7;
        public static float TradeCash = 100;
        public static int MarketHour = 9;
        public static int MarketMinute = 30;

        // Messaging
        public static List<string> DevNumbers = new List<string>() { "32476067619" };



    }
}
