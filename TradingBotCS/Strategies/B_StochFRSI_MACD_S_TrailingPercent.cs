using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Strategies
{
    public class B_StochFRSI_MACD_S_TrailingPercent : BaseStrategy
    {
        private static string Name = "B_StochFRSI_MACD_S_TrailingPercent";
        private static int Counter = 3;

        // buy parameters
        private int MacdCounter { get; set; }
        private double FirstCounterprice { get; set; }
        private decimal LastMacdHist { get; set; }

        // sell parameters
        private float TakeProfit { get; set; }
        private float BottomProfit { get; set; }
        private bool PassedBottom { get; set; }

        public async Task<bool> BuyStrategy(StrategyData data)
        {
            try
            {
                if ((data.StochFRSIK <= 15 || data.StochFRSID <= 15) && data.MacdHist < 0)
                {
                    if (MacdCounter == 0)
                    {
                        FirstCounterprice = data.Price;
                        LastMacdHist = data.MacdHist;
                        MacdCounter += 1;
                    }else if (MacdCounter > 0 && MacdCounter < 3)
                    {
                        if (data.MacdHist < LastMacdHist)
                        {
                            LastMacdHist = data.MacdHist;
                            MacdCounter += 1;
                        }
                    }else if (MacdCounter == 3 && FirstCounterprice*1.02 > data.Price)
                    {
                        MacdCounter = 0;
                        decimal Shares = Math.Floor((decimal)Program.TradeCash / (decimal)data.Price);
                        if (Shares > 0)
                        {
                            PassedBottom = false;
                            TakeProfit = 0.055f;
                            BottomProfit = 0.05f;
                            return true;
                        }
                    }
                }

                if (data.MacdHist > 0)
                {
                    MacdCounter = 0;
                }
                return false;
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
                if ((((float)lastRawData.Close - avgPrice)/avgPrice) > TakeProfit)
                {
                    PassedBottom = true;
                }
                if ((((float)lastRawData.Close - avgPrice) / avgPrice) > TakeProfit*0.01)
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

        public B_StochFRSI_MACD_S_TrailingPercent()
        {

            MacdCounter = 0;

            PassedBottom = false;
            TakeProfit = 0.055f;
            BottomProfit = 0.05f;
        }

    }
}