using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.Util;

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

        public async Task<(bool, int)> BuyStrategy(StrategyData data)
        {
            try
            {
                if (data.RSI <= 15 && data.MacdHist < 0)
                {
                    if (MacdCounter == 0)
                    {
                        FirstCounterprice = data.Price;
                        LastMacdHist = data.MacdHist;
                        MacdCounter += 1;
                    }else if (MacdCounter > 0 && MacdCounter < Counter)
                    {
                        if (data.MacdHist < LastMacdHist)
                        {
                            LastMacdHist = data.MacdHist;
                            MacdCounter += 1;
                        }
                    }else if (MacdCounter == Counter && FirstCounterprice*1.02 > data.Price)
                    {
                        MacdCounter = 0;
                        decimal Shares = Math.Floor((decimal)Program.MaxTradeValue / (decimal)data.Price);
                        if (Shares > 0)
                        {
                            return (true, Convert.ToInt32(Shares));
                        }
                    }
                }

                if (data.MacdHist > 0)
                {
                    MacdCounter = 0;
                }
                return (false,0);
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
            return (false,0);
        }

        public B_StochFRSI_MACD_S_TrailingPercent()
        {
            MacdCounter = 0;
        }

    }
}