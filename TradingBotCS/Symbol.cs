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

        // Market Gap Parameters
        private float MinimumGap = 3;
        private float MaximumGap = 12;

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
        public B_Scanner_MACD_S_TrailingPercent Strategy { get; set; }
        public List<RawData> HistoricalData { get; set; }
        public bool GapCalculated { get; set; }

        public async Task ExecuteStrategy(bool buyEnabled = false)
        {
            try
            {
                //if (Position > 0)
                //{
                //    bool Result = await Strategy.SellStrategy(AvgPrice, LastRawData, Ticker);
                //    Logger.Warn(Name, $"{Result}");
                //    if (Result)
                //    {
                //        // sell order
                //        //Program.IbClient.ClientSocket.placeOrder(Program.IbClient.NextOrderId, this.Contract, /*this moet een order worden. moeten we nog aanmaken. komt in orderManager*/);
                //    }
                //} else 
                if (CashBalance >= Program.TradeCash && buyEnabled == true)
                {
                    // Strategy Data hier pas berekenen, cpu uitsparen als position 0 is en geld onder minimum
                    await CalculateData(HistoricalData);

                    var Results = await Strategy.BuyStrategy(this.StrategyData);
                    //Logger.Info(Name, $"{Result}");
                    if (Results.Item1)
                    {
                        Logger.Info(Name, "I try to buy stuff");
                        //symbolobject toevoegen aan een nieuwe lijst waarvoor we data moeten ophalen en execute strategy dan blijven uitvoeren
                        Program.ActiveSymbolList.Add(this);

                        // buy order
                        Order Order = await OrderManager.CreateOrder("BUY", "MKT", Results.Item2);
                        Program.IbClient.ClientSocket.placeOrder(Program.IbClient.NextOrderId, this.Contract, Order);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
        }


        public async Task CalculateGap()
        {
            List<RawData> CloseList = new List<RawData>();
            List<RawData> OpenList = new List<RawData>();
            string queryTime;
            //HistoricalData
            DateTime dateValue = DateTime.Now;
            int DayOfWeek = (int)dateValue.DayOfWeek;
            if (DayOfWeek == 1)
            {
                queryTime = DateTime.Now.AddDays(-3).ToString("ddMMyyyy HH:mm:ss");
            }
            else 
            {
                queryTime = DateTime.Now.AddDays(-1).ToString("ddMMyyyy HH:mm:ss");
            }

            string[] words = queryTime.Split(' ');
            queryTime = words[0] + " " +"21:45:00";
            queryTime = queryTime.Insert(2, "-");
            queryTime = queryTime.Insert(5, "-");
            DateTime CloseTime = Convert.ToDateTime(queryTime);

            queryTime = DateTime.Now.ToString("ddMMyyyy HH:mm:ss");
            words = queryTime.Split(' ');
            //queryTime = words[0] +" "+ "15:30:00";
            queryTime = words[0] + " " + "00:30:00";    
            queryTime = queryTime.Insert(2, "-");
            queryTime = queryTime.Insert(5, "-");
            DateTime OpenTime = Convert.ToDateTime(queryTime);
            // if priceRange < 0.97 and priceRange > 0.88:
            foreach (RawData item in HistoricalData)
            {
                if (item.DateTime == CloseTime)
                {
                    CloseList.Add(item);
                }
                else if (item.DateTime == OpenTime)
                {
                    OpenList.Add(item);
                }    
            }
            if(OpenList.Count == 1 && CloseList.Count == 1)
            {
                double gap = (OpenList[0].Close - CloseList[0].Close) / CloseList[0].Close * 100;
                if(gap > MinimumGap && gap <= MaximumGap)
                {
                    Program.CorrectGapList.Add(this);
                    GapCalculated = true;
                    Logger.Info(Name, $"Correct Gap: {this.Ticker} {gap}%");
                }
            }
            else
            {
                Logger.Error(Name, "Gap Calculation Failed");
            }

        }

        public async Task CalculateData(List<RawData> data)
        {
            try
            {
                if (LastRawData == null)
                {
                    LastRawData = data.Last();
                }
                List<decimal> RawPriceList = new List<decimal>();

                if (data.Count > 50)
                {
                    foreach (RawData R in data.GetRange((data.Count - 40), 40)) RawPriceList.Add((decimal)R.Close);
                }
                else
                {
                    foreach (RawData R in data) RawPriceList.Add((decimal)R.Close);
                }

                //List<decimal> Rsi = await IndicatorRSI.RSI(RawPriceList, RsiPeriod);

                //var StochRsi = await IndicatorRSI.stochRSI(Rsi, FastKperiod, FastDPeriod);
                //List<decimal> K = StochRsi.Item1;
                //List<decimal> D = StochRsi.Item2;
            
                List<decimal> Macd = await IndicatorMACD.MACD(RawPriceList, MacdSlowPeriod, MacdFastPeriod);
                List<decimal> MacdSignal = await IndicatorMACD.MACDsignal(Macd, MacdSignalPeriod);
                List<decimal> MacdHist = await IndicatorMACD.MACDhist(Macd, MacdSignal);

                //Console.WriteLine(Rsi.Count);
                //Console.WriteLine(K.Count);
                //Console.WriteLine(D.Count);
                //Console.WriteLine(Macd.Count);
                //Console.WriteLine(MacdSignal.Count);
                //Console.WriteLine(MacdHist.Count);

                //StrategyData = new StrategyData(LastRawData.Close, K.Last(), D.Last(), Macd.Last(), MacdHist.Last(), MacdSignal.Last(), LastRawData.DateTime);
                StrategyData = new StrategyData(LastRawData.Close, 0m, 0m, Macd.Last(), MacdHist.Last(), MacdSignal.Last(), LastRawData.DateTime);
                //Console.WriteLine(StrategyData.Price);
                //Console.WriteLine(StrategyData.StochFRSIK);
                //Console.WriteLine(StrategyData.StochFRSID);
                //Console.WriteLine(StrategyData.Macd);
                //Console.WriteLine(StrategyData.MacdHist);
                //Console.WriteLine(StrategyData.MacdSignal);
                //Console.WriteLine(StrategyData.DateTime);
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
            this.Strategy = new B_Scanner_MACD_S_TrailingPercent();
            this.HistoricalData = new List<RawData>();
            this.GapCalculated = false;
        }

        public override string ToString()
        {
            return Id + ": " + Ticker;
        }
    }
}
