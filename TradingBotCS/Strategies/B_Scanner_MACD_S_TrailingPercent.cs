    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Strategies
{
    class B_Scanner_MACD_S_TrailingPercent
    {
        private static string Name = "B_Scanner_MACD_S_TrailingPercent";

        // buy parameters
        private decimal LastMacdHist { get; set; }

        // sell parameters
        private float TakeProfit { get; set; }
        private float BottomProfit { get; set; }
        private bool PassedBottom { get; set; }

        public async Task<bool> BuyStrategy(StrategyData data)
        {
            try
            {
                if()
                {
                    
                }

                if (data.MacdHist > 0)
                {
                    
                }
                return false; //niet kopen, true is wel kopen
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
            return false;
        }

        public async Task<bool> SellStrategy(float avgPrice, RawData lastRawData, string symbol)
        {
            try
            {
                Console.WriteLine("test");
                if ((((float)lastRawData.Close - avgPrice) / avgPrice) > TakeProfit)
                {
                    PassedBottom = true;
                }
                if ((((float)lastRawData.Close - avgPrice) / avgPrice) > TakeProfit * 0.01)
                {
                    TakeProfit = (((float)lastRawData.Close - avgPrice) / avgPrice) - 0.01f;

                    if ((((float)lastRawData.Close - avgPrice) / avgPrice) > 0.5)
                    {
                        TakeProfit = (((float)lastRawData.Close - avgPrice) / avgPrice) - (((float)lastRawData.Close - avgPrice) / avgPrice) * 0.02f;
                    }
                    BottomProfit = TakeProfit - 0.01f;
                    if (TakeProfit > 1)
                    {
                        BottomProfit = TakeProfit * 0.9f;
                    }
                    Logger.Info(Name, $"{symbol} Borders: {BottomProfit}% - {TakeProfit}%");
                }
                else if ((((float)lastRawData.Close - avgPrice) / avgPrice) < TakeProfit && (((float)lastRawData.Close - avgPrice) / avgPrice) > BottomProfit && PassedBottom == true)
                {
                    Logger.Info(Name, $"Sell Order: {symbol} Current Price: {(float)lastRawData.Close}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
            return false;
        }

        public B_Scanner_MACD_S_TrailingPercent()
        {
            PassedBottom = false;
            TakeProfit = 0.055f;
            BottomProfit = 0.05f;
        }



    }
}
