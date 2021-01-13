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
using TradingBotCS.HelperClasses;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Messaging;
using TradingBotCS.Models_Indicators;
using TradingBotCS.Strategies;

namespace TradingBotCS
{
    class Program
    {
        public static string DevNumber = "32476067619";


        public static float TradeCash = 100;
        //static string Ip = "jorenvangoethem.duckdns.org";
        static string Ip = "192.168.50.107";
        static int Port = 4002;
        static int ApiId = 1;
        public static WrapperOverride IbClient = new WrapperOverride();
        static EReader IbReader;
        public static List<Symbol> SymbolObjects;

        public static MongoClient MongoDBClient = new MongoClient(); // automatically connects to localhost:27017

        private static string Name = "Program";

        public static bool ScannerReady = true;
        public static int GettingData = 0;

        public static List<string> SymbolList = new List<string>();

        public static List<Symbol> CorrectGapList = new List<Symbol>();
        public static List<Symbol> ActiveSymbolList = new List<Symbol>();

        public static Symbol TestSymbol;

        public static bool MarketState = true;
        public static int MarketHour = 9;
        public static int MarketMinute = 30;
        public static bool MarketClosedMessage = false;


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

            Logger.Info(Name, "Started");
            while(true)Console.ReadKey(); // zorgt er voor dat de console nooit sluit
        }

        static async Task InfiniteStartup()
        {
            new Thread(() =>
            {
                (int, int) marketHours = (9,30);
                while (true)
                {
                    try
                    {
                        DateTime NYtime = GetNewYorkTime();
                        if (!IbClient.ClientSocket.IsConnected() && NYtime.Hour >= 7 && NYtime.Minute >= 00)
                        {
                            Connect();
                            Thread.Sleep(10000);
                        }
                        else if(IbClient.ClientSocket.IsConnected() && NYtime.Hour >= 7 && NYtime.Minute >= 00)
                        {
                            Thread.Sleep(10000);
                            CheckMartketHours();
                            if (MarketState == false && MarketClosedMessage == false)
                            {
                                Logger.Critical(Name, "MARKET IS CLOSED TODAY");
                                MarketClosedMessage = true;
                            }
                        }

                        NYtime = GetNewYorkTime();

                        if (MarketState && NYtime.Hour == MarketHour && NYtime.Minute == MarketMinute)
                        {
                            MarketClosedMessage = false;
                            if (!IbClient.ClientSocket.IsConnected())
                            {
                                Connect();
                            }
                            else
                            {
                                AccountUpdates();
                                IbClient.ClientSocket.reqPositions();
                                MarketScanner();

                                SymbolList = SymbolList.OrderBy(x => Guid.NewGuid()).ToList();

                                SymbolObjects = CreateSymbolObjects(SymbolList);

                                RequestSymbolContracts();

                                GetData();

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

        public static DateTime GetNewYorkTime() 
        {
            //get datetime now in new york
            DateTime databaseUtcTime = DateTime.UtcNow;
            TimeZoneInfo NYtimezoneinfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
            DateTime NYtime = TimeZoneInfo.ConvertTimeFromUtc(databaseUtcTime, NYtimezoneinfo);
            return NYtime;
        }


        static async Task CheckMartketHours()
        {
            try
            {
                TestSymbol = new Symbol("TSLA", 99999);

                Contract Contract = new Contract();
                Contract.Symbol = TestSymbol.Ticker;
                Contract.SecType = "STK";
                Contract.Exchange = "SMART";
                Contract.Currency = "USD";

                TestSymbol.Contract = Contract;
                IbClient.ClientSocket.reqContractDetails(TestSymbol.Id, Contract);

                while(TestSymbol.ContractDetails == null)
                {
                    Thread.Sleep(10000);
                }

                // check hours
                string hourstring = TestSymbol.ContractDetails.LiquidHours;

                hourstring = hourstring.Split(';')[0];
                if(hourstring.Contains("CLOSED"))
                {
                    MarketState = false;
                    MarketHour = 0;
                    MarketMinute = 0;
                }
                else
                {
                    MarketState = true;
                
                    string startstring = hourstring.Split('-')[0];
                    startstring = startstring.Split(':')[1];

                    int hour = Convert.ToInt32(startstring.Substring(0, 2));
                    int minute = Convert.ToInt32(startstring.Substring(2, 2));
                    Logger.Verbose(Name, $"{hour}, {minute}");
                    MarketHour = hour;
                    MarketMinute = minute;
                }
            }
            catch (Exception ex)
            {
                Logger.Critical(Name, $"{ex}");
                MarketState = false;
                MarketHour = 0;
                MarketMinute = 0;
            }
        }

        static async Task Connect()
        {
            Logger.Verbose(Name, "Connecting To API");

            IbClient.ClientSocket.eConnect(Ip, Port, ApiId);
            IbReader = new EReader(IbClient.ClientSocket, IbClient.Signal);
            IbReader.Start();

            new Thread(() =>
            {
                while (IbClient.ClientSocket.IsConnected())
                {
                    IbClient.Signal.waitForSignal();
                    IbReader.processMsgs();
                }
            })
            { IsBackground = true }.Start();
        }

        static List<Symbol> CreateSymbolObjects(List<string> symbolList)
        {
            Logger.Info(Name, "Creating Symbol Objects");

            List<Symbol> Result = new List<Symbol>();
            for(int i = 0; i < symbolList.Count; i++)
            {
                Symbol s = new Symbol(symbolList[i], i);
                Result.Add(s);
            }
            return Result;
        }

        static async Task RequestSymbolContracts()
        {
            Logger.Info(Name, "Creating Symbol Contracts");

            foreach(Symbol S in SymbolObjects)
            {
                Contract Contract = await CreateContract(S.Ticker);
                S.Contract = Contract;
                IbClient.ClientSocket.reqContractDetails(S.Id, Contract);
                Thread.Sleep(20);
                //GetMarketData(Contract, i);
            }
        }

        static async Task MarketScanner()
        {
            ScannerSubscription temp = new ScannerSubscription();
            temp.NumberOfRows = 50;
            temp.ScanCode = "HIGH_OPEN_GAP";
            temp.Instrument = "STK";
            temp.LocationCode = "STK.NASDAQ";
            
            temp.StockTypeFilter = "CORP";
            for (double i = 1; i < 25; i++)
            {
                temp.AbovePrice = i;
                temp.BelowPrice = i+1;
                List<TagValue> test = new List<TagValue>();
                List<TagValue> test2 = new List<TagValue>();
                while (ScannerReady == false)
                {
                    Thread.Sleep(1);
                }
                if (ScannerReady) IbClient.ClientSocket.reqScannerSubscription((int)i, temp, test, test2);
                if (i == 24) Thread.Sleep(1000);
                ScannerReady = false;
            }
        }

       static async Task GetData()
       {
            DateTime NYtime = GetNewYorkTime();
            String queryTime = GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");


            //foreach (Symbol S in SymbolObjects.GetRange(0, 300))
            foreach (Symbol S in SymbolObjects)
            {
                try
                {
                    //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
                    //if (GettingData < 50 && DateTime.Now.Hour >= 15 && DateTime.Now.Minute < 45)
                    if (GettingData < 50 && NYtime.Hour >= 15 && NYtime.Minute < 45)
                    {
                        while (GettingData >= 49)
                        {
                            Thread.Sleep(1);
                            if (S == SymbolObjects.Last()) break;
                        };
                        IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, queryTime, "4 D", "15 mins", "MIDPOINT", 1, 1, false, null); // maar 50 tegelijk
                        GettingData += 1;
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
                    DateTime NYtime = GetNewYorkTime();
                    //if (DateTime.Now.Hour >= 1 && DateTime.Now.Minute >= 45)
                    if (NYtime.Hour >= 15 && NYtime.Minute >= 45)
                    {
                        //get latest datapoint
                        String queryTime = GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");

                        foreach (Symbol S in SymbolObjects)
                        {
                            try
                            {
                                //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
                                if (GettingData < 50)
                                {
                                    while (GettingData >= 49)
                                    {
                                        Thread.Sleep(1);
                                        if (S == SymbolObjects.Last()) break;
                                    };
                                    IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, queryTime, "3600 S", "15 mins", "MIDPOINT", 1, 1, false, null); // timing aanpassen dat het enkel het laatste punt is of checken of het al bestaat
                                    GettingData += 1;
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
                        if(GettingData == 0)
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




        static async Task GetDataForActiveSymbols()
        {
            List<TagValue> MktDataOptions = new List<TagValue>();
            Logger.Info("Active List", $"{ActiveSymbolList.Count()}");
            foreach (Symbol S in ActiveSymbolList)
            {
                //IbClient.ClientSocket.reqMktData(S.Id, S.Contract, "", false, false, MktDataOptions);
                IbClient.ClientSocket.reqRealTimeBars(S.Id, S.Contract, 5, "MIDPOINT", false, null);
            }
        }

        static async Task AccountUpdates()
        {
            Logger.Verbose(Name, "Requesting Account Updates");
            new Thread(() =>
            {
                if (IbClient.ClientSocket.IsConnected())
                {
                    IbClient.ClientSocket.reqAccountUpdates(true, "DU2361307");
                    IbClient.ClientSocket.reqPositions();
                }
            })
            { IsBackground = false }.Start();
        }

        static async Task<Contract> CreateContract(string symbol, string secType = "STK", string exchange = "SMART", string currency = "USD")
        {
            Logger.Verbose(Name, "Creating Contract");
            Contract Contract =  new Contract();
            Contract.Symbol = symbol;
            Contract.SecType = secType;
            Contract.Exchange = exchange;
            Contract.Currency = currency;

            return Contract;
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
