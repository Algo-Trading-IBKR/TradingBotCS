using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;
using TradingBotCS.Models_Indicators;
using TradingBotCS.Strategies;

namespace TradingBotCS
{
    public class Symbol
    {
        //Class Variables
        public static float CashBalance { get; set; }
        private static string Name = "Symbol";
        // buy parameters
        private int RsiPeriod = 14;
        private int FastKperiod = 3;
        private int FastDPeriod = 3;
        private int MacdSlowPeriod = 28;
        private int MacdFastPeriod = 12;
        private int MacdSignalPeriod = 9;

        public string Ticker { get; set; }
        public int Id { get; set; }
        public Contract Contract { get; set; }
        public ContractDetails ContractDetails { get; set; }
        public Order LatestOrder { get; set; }
        public float AvgPrice { get; set; }
        public List<RawData> RawDataList { get; set; }
        public RawData LastRawData { get; set; }
        public StrategyData StrategyData { get; set; }
        public int Position { get; set; }
        public B_StochFRSI_MACD_S_TrailingPercent Strategy { get; set; }

        public async Task ExecuteStrategy()
        {
            try
            {
                if (Position > 0)
                {
                    bool Result = await Strategy.SellStrategy(AvgPrice, LastRawData, Ticker);
                    if (Result)
                    {
                        // sell order
                    }
                } else if (CashBalance >= Program.MinimumCash)
                {
                    // Strategy Data hier pas berekenen, cpu uitsparen als position 0 is en geld onder minimum
                    await CalculateData();

                    bool Result = await Strategy.BuyStrategy(StrategyData);
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

        public async Task CalculateData()
        {
            try
            {           
                List<decimal> RawPriceList = new List<decimal>();
                foreach (RawData R in RawDataList) RawPriceList.Add((decimal)R.Close);
            
                List<decimal> Rsi = await IndicatorRSI.RSI(RawPriceList, RsiPeriod);

                var StochRsi = await IndicatorRSI.stochRSI(Rsi, FastKperiod, FastDPeriod);
                List<decimal> K = StochRsi.Item1;
                List<decimal> D = StochRsi.Item2;
            
                List<decimal> Macd = await IndicatorMACD.MACD(RawPriceList, MacdSlowPeriod, MacdFastPeriod);
                List<decimal> MacdSignal = await IndicatorMACD.MACDsignal(RawPriceList, MacdSignalPeriod);
                List<decimal> MacdHist = await IndicatorMACD.MACDhist(Macd, MacdSignal);
                Console.WriteLine("test");

                Console.WriteLine(Rsi.Count);
                Console.WriteLine(K.Count);
                Console.WriteLine(D.Count);
                Console.WriteLine(Macd.Count);
                Console.WriteLine(MacdSignal.Count);
                Console.WriteLine(MacdHist.Count);

                StrategyData = new StrategyData((float)LastRawData.Close, K.Last(), D.Last(), Macd.Last(), MacdHist.Last(), MacdSignal.Last(), LastRawData.DateTime);

            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
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
