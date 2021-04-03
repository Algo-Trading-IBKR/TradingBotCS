using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using TradingBotCS.Util;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Models_Indicators;
using TradingBotCS.Strategies;
using System.Threading;

namespace TradingBotCS
{
    public class Symbol
    {
        //Class Variables
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

        public bool BOrder { get; set; }
        public bool SOrder { get; set; }

        public string Ticker { get; set; }
        public int Id { get; set; }
        public Contract Contract { get; set; }
        public ContractDetails ContractDetails { get; set; }
        public Order LatestOrder { get; set; }
        public float AvgPrice { get; set; }
        public List<RawData> RawDataList { get; set; }
        public RawData LastRawData { get; set; }
        public StrategyData StrategyData { get; set; }
        public B_StochFRSI_MACD_S_TrailingPercent Strategy { get; set; }
        public List<RawData> HistoricalData { get; set; }
        public bool GapCalculated { get; set; }

        public async Task ExecuteStrategy()
        {
            try
            {
                //if (CashBalance >= Program.TradeCash && buyEnabled == true)
                if (Program.BuyEnabled == true && Program.CashBalance >= Program.MaxTradeValue*2 && this.BOrder == false)
                {
                    // Strategy Data hier pas berekenen, cpu uitsparen als position 0 is en geld onder minimum
                    //bool calculationSucceeded = await CalculateData(HistoricalData);
                    bool calculationSucceeded = await CalculateData(RawDataList);

                    if (calculationSucceeded)
                    {
                        var Results = await Strategy.BuyStrategy(this.StrategyData);
                    
                        //Logger.Info(Name, $"{Result}");
                        if (Results.Item1)
                        {
                            //Logger.Info(Name, $"{this.Ticker}: BUY");

                            //symbolobject toevoegen aan een nieuwe lijst waarvoor we data moeten ophalen en execute strategy dan blijven uitvoeren
                            //Program.ActiveSymbolList.Add(this);

                            // market buy order

                            //var Result = await OrderManager.CreateOrder("BUY", "MKT", amount:Results.Item2);
                            var Result = await OrderManager.CreateOrder(symbol: this.Ticker, action: "BUY", type: "TRAIL LIMIT", amount: Results.Item2, trailStopPrice: LastRawData.Close * (1 + (Program.BTrailingPercent / 100)), priceOffset: Program.BPriceOffset, trailingPercent: Program.BTrailingPercent);

                            if (Result.Item1 == true)
                            {
                                this.BOrder = true;
                                Program.IbClient.ClientSocket.placeOrder(Program.IbClient.NextOrderId, this.Contract, Result.Item2);
                                Logger.Info(Name, $"Sent {Result.Item2.Action} {Result.Item2.OrderType} for {this.Ticker}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
            }
        }


        //public async Task CalculateGap()
        //{
        //    GapCalculated = true;
        //    List<RawData> CloseList = new List<RawData>();
        //    List<RawData> OpenList = new List<RawData>();
        //    string queryTime;
        //    //HistoricalData
        //    DateTime dateValue = Timezones.GetNewYorkTime();
        //    int DayOfWeek = (int)dateValue.DayOfWeek;
        //    if (DayOfWeek == 1)
        //    {
        //        queryTime = Timezones.GetNewYorkTime().AddDays(-3).ToString("ddMMyyyy HH:mm:ss");
        //    }
        //    else 
        //    {
        //        queryTime = Timezones.GetNewYorkTime().AddDays(-1).ToString("ddMMyyyy HH:mm:ss");
        //    }

        //    string[] words = queryTime.Split(' ');
        //    queryTime = words[0] + " " +"15:45:00";
        //    queryTime = queryTime.Insert(2, "-");
        //    queryTime = queryTime.Insert(5, "-");
        //    DateTime CloseTime = Convert.ToDateTime(queryTime);

        //    queryTime = Timezones.GetNewYorkTime().ToString("ddMMyyyy HH:mm:ss");
        //    words = queryTime.Split(' ');
        //    queryTime = words[0] +" "+ "09:30:00";
        //    //queryTime = words[0] + " " + "00:30:00";    
        //    queryTime = queryTime.Insert(2, "-");
        //    queryTime = queryTime.Insert(5, "-");
        //    DateTime OpenTime = Convert.ToDateTime(queryTime);
        //    // if priceRange < 0.97 and priceRange > 0.88:
        //    foreach (RawData item in HistoricalData)
        //    {
        //        if (item.DateTime == CloseTime)
        //        {
        //            CloseList.Add(item);
        //        }
        //        else if (item.DateTime == OpenTime)
        //        {
        //            OpenList.Add(item);
        //        }    
        //    }
        //    if(OpenList.Count == 1 && CloseList.Count == 1)
        //    {
        //        double gap = (OpenList[0].Close - CloseList[0].Close) / CloseList[0].Close * 100;
        //        if(gap > MinimumGap && gap <= MaximumGap)
        //        {
        //            Program.CorrectGapList.Add(this);
                    
        //            Logger.Info(Name, $"Correct Gap: {this.Ticker} {gap}%");
        //        }
        //    }
        //    else
        //    {
        //        Logger.Error(Name, "Gap Calculation Failed");
        //    }

        //}

        public async Task<bool> CalculateData(List<RawData> rawdata)
        {
            try
            {
                if (rawdata.Count < 200) return false;
                List<RawData> data = rawdata.OrderBy(R => R.DateTime).ToList();

                if (LastRawData == null)
                {
                    LastRawData = data.Last();
                }

                List<RawData> StratData = new List<RawData>();
                // lijst filteren op kwartieren en dan lastRawData toevoegen
                foreach (RawData R in data)
                {
                    if((R.DateTime.Minute == 00 || R.DateTime.Minute == 15 || R.DateTime.Minute == 30 || R.DateTime.Minute == 45) && R.DateTime.Second == 0) {
                        StratData.Add(R);
                    }
                }
                StratData.Add(LastRawData); // add check to see if last isn't already in the list
                if (StratData.Count < 50) return false;

                List<decimal> RawPriceList = new List<decimal>();
                foreach (RawData R in StratData.GetRange((StratData.Count - 50), 50)) RawPriceList.Add((decimal)R.Close);

                List<decimal> Rsi = await IndicatorRSI.RSI(RawPriceList, RsiPeriod);

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
                this.StrategyData = new StrategyData(Ticker, LastRawData.Close, Rsi.Last(), 0m, 0m, Macd.Last(), MacdHist.Last(), MacdSignal.Last(), LastRawData.DateTime);
                //Console.WriteLine(StrategyData.Price);
                //Console.WriteLine(StrategyData.StochFRSIK);
                //Console.WriteLine(StrategyData.StochFRSID);
                //Console.WriteLine(StrategyData.Macd);
                //Console.WriteLine(StrategyData.MacdHist);
                //Console.WriteLine(StrategyData.MacdSignal);
                //Console.WriteLine(StrategyData.DateTime);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
                return false;
            }
        }


        public Symbol(string ticker, int id)
        {
            this.Ticker = ticker;
            this.Id = id;
            this.Strategy = new B_StochFRSI_MACD_S_TrailingPercent();
            this.HistoricalData = new List<RawData>();
            this.RawDataList = new List<RawData>();
            this.GapCalculated = false;
            this.BOrder = false;
            this.SOrder = false;
            this.Contract = new Contract();
        }

        public override string ToString()
        {
            return Id + ": " + Ticker;
        }
    }
}
