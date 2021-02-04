using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using TradingBotCS.Database;
using TradingBotCS.Email;
using TradingBotCS.Util;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Messaging;
using TradingBotCS.Models_Indicators;
using TradingBotCS.Strategies;
using TradingBotCS;

namespace TradingBotCS
{
    class Program
    {
        // API
        public static string Ip = Constants.Ip;
        public static int Port = Constants.Port;
        public static int ApiId = Constants.ApiId;

        // Strategy
        private static int StartingHour = Constants.StartingHour;
        public static float TradeCash = Constants.TradeCash;
        public static int MarketHour = Constants.MarketHour;
        public static int MarketMinute = Constants.MarketMinute;

        // Messaging
        public static List<string> DevNumbers = Constants.DevNumbers;

        // IB objects
        public static WrapperOverride IbClient = new WrapperOverride();
        public static EReader IbReader;
        

        // Databases
        public static MongoClient MongoDBClient = new MongoClient(); // automatically connects to localhost:27017

        // File specific
        private static string Name = "Program";

        // historical data getter
        public static bool ScannerReady = true;
        public static int GettingHistoricalData = 0;

        // market data checker
        public static List<string> SymbolList = new List<string>();
        public static Symbol TestSymbol;
        public static bool MarketState = true;
        public static bool MarketClosedMessage = false;

        // SymbolList for trading
        public static List<Symbol> SymbolObjects; // ALL symbol object stay in this list
        public static List<Symbol> CorrectGapList = new List<Symbol>(); // only used for strategy with gap up/down
        public static List<Symbol> ActiveSymbolList = new List<Symbol>(); // list for active trading or realtime data

        
        static async Task Main(string[] args)
        {
            Logger.SetLogLevel(Logger.LogLevel.LogLevelInfo); // Custom Logger Test
            Logger.Verbose(Name, "Start");


            //List<string> Messages = new List<string>() { "test 3", "HA GAYY" };
            //List<string> Numbers = new List<string>() { "32476067619", "32470579542" };
            //await MobileService.SendTextMsg(Messages, Numbers);

            //MongoDBtest();

            //test();

            InfiniteStartup();

            Logger.Verbose(Name, "Started");
            while(true)Console.ReadKey(); // zorgt er voor dat de console nooit sluit
        }

        static async Task InfiniteStartup()
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        DateTime NYtime = Timezones.GetNewYorkTime();
                        if (!IbClient.ClientSocket.IsConnected() && NYtime.Hour >= StartingHour)
                        {
                            ApiConnection.Connect();
                            Thread.Sleep(5000);
                        }
                        else if(IbClient.ClientSocket.IsConnected() && NYtime.Hour >= StartingHour)
                        {
                            Thread.Sleep(5000);
                            Market.CheckMartketHours();
                            if (MarketState == false && MarketClosedMessage == false)
                            {
                                Logger.Critical(Name, "MARKET IS CLOSED TODAY");
                                MarketClosedMessage = true;
                            }
                        }

                        NYtime = Timezones.GetNewYorkTime();

                        if (MarketState && NYtime.Hour == MarketHour && NYtime.Minute == MarketMinute)
                        {
                            Logger.Info(Name, "Starting...");
                            MarketClosedMessage = false;
                            if (!IbClient.ClientSocket.IsConnected())
                            {
                                ApiConnection.Connect();
                            }
                            else
                            {
                                ApiConnection.AccountUpdates();
                                IbClient.ClientSocket.reqPositions();
                                MarketScanner.GapUp();

                                SymbolList = SymbolList.OrderBy(x => Guid.NewGuid()).ToList();

                                SymbolObjects = SymbolManager.CreateSymbolObjects(SymbolList);

                                ContractManager.RequestSymbolContracts(SymbolObjects);

                                DataManager.GetHistoricalBars(SymbolObjects, DateTime.Now);

                                checkTime();
                            }
                        }
                        Thread.Sleep(100);

                    }
                    catch (Exception ex)
                    {
                        Logger.Critical(Name, $"{ex}");
                    }
                }

            })
            { IsBackground = false }.Start();
        }

       static async Task GetData()
       {
            DateTime NYtime = Timezones.GetNewYorkTime();
            String queryTime = Timezones.GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");

            //foreach (Symbol S in SymbolObjects.GetRange(0, 300))
            foreach (Symbol S in SymbolObjects)
            {
                try
                {
                    //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
                    //if (GettingData < 50 && DateTime.Now.Hour >= 15 && DateTime.Now.Minute < 45)
                    if (GettingHistoricalData < 50 && NYtime.Hour >= 15 && NYtime.Minute < 45)
                    {
                        while (GettingHistoricalData >= 49)
                        {
                            Thread.Sleep(1);
                            if (S == SymbolObjects.Last()) break;
                        };
                        IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, queryTime, "4 D", "15 mins", "MIDPOINT", 1, 1, false, null); // maar 50 tegelijk
                        GettingHistoricalData += 1;
                        Thread.Sleep(20);
                    }
                    //IbClient.ClientSocket.reqRealTimeBars(S.Id, S.Contract, 5, "MIDPOINT", false, null); // false om ook data buiten trading hours te krijgen
                    //S.ExecuteStrategy();
                    //Console.WriteLine(S.Ticker);
                }
                catch (Exception ex)
                {
                    Logger.Error(Name, $"{S.Ticker} Failed: \n{ex}");
                }
            }
            Logger.Info(Name, "Historical Data Done");
       }

        static async Task checkTime()
        {
            new Thread(() =>
            {
                while (IbClient.ClientSocket.IsConnected())
                {
                    DateTime NYtime = Timezones.GetNewYorkTime();
                    //if (DateTime.Now.Hour >= 1 && DateTime.Now.Minute >= 45)
                    if (NYtime.Hour >= 15 && NYtime.Minute >= 45)
                    {
                        //get latest datapoint
                        String queryTime = Timezones.GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");

                        foreach (Symbol S in SymbolObjects)
                        {
                            try
                            {
                                //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
                                if (GettingHistoricalData < 50)
                                {
                                    while (GettingHistoricalData >= 49)
                                    {
                                        Thread.Sleep(1);
                                        if (S == SymbolObjects.Last()) break;
                                    };
                                    IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, queryTime, "3600 S", "15 mins", "MIDPOINT", 1, 1, false, null); // timing aanpassen dat het enkel het laatste punt is of checken of het al bestaat
                                    GettingHistoricalData += 1;
                                    Thread.Sleep(20);
                                }
                                //IbClient.ClientSocket.reqRealTimeBars(S.Id, S.Contract, 5, "MIDPOINT", false, null); // false om ook data buiten trading hours te krijgen
                                //S.ExecuteStrategy();
                                //Console.WriteLine(S.Ticker);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(Name, $"{S.Ticker} Failed: \n{ex}");
                            }
                        }
                        if(GettingHistoricalData == 0)
                        {
                            Logger.Info(Name, "DONE buying stocks");
                            //GetDataForActiveSymbols();
                            break;
                        }
                    }
                }
            })
            { IsBackground = false }.Start();
        }
        
        public static async Task MongoDBtest()
        {
            Logger.Verbose(Name, "Started MongoDB Test");

            var db = MongoDBClient.GetDatabase("TradingBot");
            var collection = db.GetCollection<BsonDocument>("AccountInfo");
            var count = await collection.CountDocumentsAsync(new BsonDocument("Type", "cashbalance")); // ALWAYS USE AWAIT
            var filter = new BsonDocument();
            using (var cursor = await collection.Find(filter).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var doc in cursor.Current)
                    {
                        //Console.WriteLine(doc);
                    }
                }
            }
        }

        public static async void test()
        {
            List<decimal> intList = new List<decimal>() { 1, 5, 9, 7, 4, 5, 6, 8, 5, 4, 1, 2, 3, 6, 9, 8, 7, 4, 5, 8, 9, 6, 5, 4, 6, 8, 7, 5, 2, 4, 3, 2, 7, 8, 9, 8, 9, 7, 5, 6, 7, 8, 9, 1, 5, 9, 7, 4, 5, 6, 8, 5, 4, 1, 2, 3, 6, 9, 8, 7, 4, 5, 8, 9, 6, 5 };
            List<decimal> declist = new List<decimal>();
            List<decimal> declist2 = new List<decimal>();
            List<decimal> declist3 = new List<decimal>();
            //declist = await MovingAverage.SMA(intList, 9);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await MovingAverage.EMA(intList, 9);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await MovingAverage.DEMA(intList, 5);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await MovingAverage.KAMA(intList, 10, 2, 30);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await IndicatorMACD.MACD(intList, 28, 12);
            //declist2 = await IndicatorMACD.MACDsignal(declist, 10);
            //declist3 = await IndicatorMACD.MACDhist(declist, declist2);
            //foreach (decimal x in declist3)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await IndicatorRSI.RSI(intList, 14);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            //var Results = await IndicatorRSI.stochRSI(declist, 14, 3);
            //declist = Results.Item1;
            //declist2 = Results.Item2;
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();
            //foreach (decimal x in declist2)
            //{
            //    Console.WriteLine(x);
            //}

            //Console.ReadKey();
        }
    }
}
