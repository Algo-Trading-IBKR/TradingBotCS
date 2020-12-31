using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;
using TradingBotCS.Strategies;

namespace TradingBotCS
{
    public class Symbol
    {
        //Class Variables
        public static float CashBalance { get; set; }
        private static string Name = "Symbol";


        public string Ticker { get; set; }
        public int Id { get; set; }
        public Contract Contract { get; set; }
        public ContractDetails ContractDetails { get; set; }
        public Order LatestOrder { get; set; }
        public float AvgPrice { get; set; }
        public float LatestPrice { get; set; }
        public List<RawData> RawDatalist { get; set; }
        public List<StrategyData> StrategyDatalist { get; set; }
        public int Position { get; set; }
        public B_StochFRSI_MACD_S_TrailingPercent Strategy { get; set; }

        public async Task ExecuteStrategy()
        {
            try
            {
                if (Position > 0)
                {
                    // Strategy Data hier pas berekenen, cpu uitsparen als position 0 is en geld onder minimum
                    bool Result = await Strategy.SellStrategy(Position, AvgPrice, LatestPrice, StrategyDatalist, Ticker);
                    if (Result)
                    {
                        // sell order
                    }
                } else if (CashBalance >= Program.MinimumCash)
                {
                    // Strategy Data hier pas berekenen, cpu uitsparen als position 0 is en geld onder minimum
                    bool Result = await Strategy.BuyStrategy(LatestPrice, StrategyDatalist);
                    if (Result)
                    {
                        // Buy order
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
        }



        public Symbol(string ticker, int id)
        {
            this.Ticker = ticker;
            this.Id = id;
        }

        public override string ToString()
        {
            return Id + ": " + Ticker;
        }
    }
}
