using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS
{
    public static class Constants
    {
        // Account Info
        public static string AccountId = "DU3134572"; // this is no longer correct

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

        // Buy
        public static bool BuyEnabled = true;

        // Sell Trailing Limit order
        public static bool SUseTrailLimitOrders = true;
        public static float SMinimumProfit = 0.10f; // make sure minimum profit is higher than TrailingPercent to prevent sell with loss
        public static float SPriceOffset = 0.01f;
        public static float STrailingPercent = 8;

        // Buy Trailing Limit order
        public static bool BUseTrailLimitOrders = true;
        public static float BPriceOffset = 0.01f;
        public static float BTrailingPercent = 8;

        // info/warn/error codes
        public static List<int> InfoCodes = new List<int>() { 1102, 2106, 2107, 2158 };
        public static List<int> WarningCodes = new List<int>();
    }
}
