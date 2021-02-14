    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.Util;

namespace TradingBotCS.Strategies
{
    public class B_Scanner_MACD_S_TrailingPercent : BaseStrategy
    {
        private static string Name = "B_Scanner_MACD_S_TrailingPercent";

        // buy parameters
        private decimal LastMacdHist { get; set; }

        // sell parameters
        private float TakeProfit { get; set; }
        private float BottomProfit { get; set; }
        private bool PassedBottom { get; set; }

        public async Task<(bool, int)> BuyStrategy(StrategyData data)
        {
            try
            {
                // if priceRange < 0.97 and priceRange > 0.88:
                //if (data.MacdHist > 0 && /data.Price )
                if (data.MacdHist > 0 )
                {
                    int Shares = (int)Math.Floor((decimal)Program.MaxTradeValue / (decimal)data.Price);
                    return (true, Shares);
                } 
                return (false, 0);
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
            return (false, 0);
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
