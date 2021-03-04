using IBApi;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Models_Indicators;
using TradingBotCS.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradingBotCS.DataModels;
using TradingBotCS.Database;

namespace TradingBotCS
{
    class Program
    {
        public static Configuration Config;

        // API
        public static string AccountId;
        public static string Ip;
        public static int Port;
        public static int ApiId;

        // Strategy
        private static int StartingHour;
        public static float MaxTradeValue;
        public static int MarketHour;
        public static int MarketMinute;
        public static float CashBalance;

        // Messaging
        public static List<string> PhoneNumbers;
        public static List<string> MailAddresses;

        // Buy
        public static bool BuyEnabled;
        public static bool BUseTrailLimitOrders;
        public static float BPriceOffset;
        public static float BTrailingPercent;

        // Sell Trailing Limit order
        public static bool SellEnabled;
        public static bool SUseTrailLimitOrders;
        public static float SMinimumProfit;
        public static float SPriceOffset;
        public static float STrailingPercent;

        // Error handling
        public static List<int> InfoCodes;
        public static List<int> WarningCodes;

        // IB objects
        public static WrapperOverride IbClient = new WrapperOverride();
        public static EReader IbReader;

        // Databases
        public static MongoClient MongoDBClient = new MongoClient(); // automatically connects to localhost:27017

        // File specific
        private static readonly string Name = "Program";

        // historical data getter
        public static bool ScannerReady = true;
        public static int GettingHistoricalData = 0;

        // market data checker
        //public static List<string> SymbolList = new List<string>(); //either made by a scanner or manually
        public static List<string> SymbolList = new List<string>() { "ACHC", "ARAY", "ALVR", "ATEC", "ALXO", "AMTI", "ABUS", "AYTU", "BEAM", "BLFS", "CAN", "CRDF", "CDNA", "CELH", "CDEV", "CHFS", "CTRN", "CLSK", "CVGI", "CUTR", "DNLI", "FATE", "FPRX", "FRHC", "FNKO", "GEVO", "GDEN", "GRBK", "GRPN", "GRWG", "HMHC", "IMAB", "IMVT", "NTLA", "KURA", "LE", "LXRX", "LOB", "LAZR", "AMD", "RRR", "IBKR", "MARA", "MESA", "MEOH", "MVIS", "COOP", "NNDM", "NSTG", "NNOX", "NFE", "NXGN", "OPTT", "OCUL", "ORBC", "OESX", "PENN", "PSNL", "PLUG", "PGEN", "QNST", "RRGB", "REGI", "SGMS", "RUTH", "RIOT", "SWTX", "SPWR", "SUNW", "SGRY", "SNDX", "TCBI", "TA", "UPWK", "VSTM", "WPRT", "WWR", "XPEL" };
        
        public static Symbol TestSymbol;
        public static bool MarketState = true;
        public static bool MarketClosedMessage = false;

        // SymbolList for trading
        public static List<string> OwnedStocks = new List<string>();
        public static List<Symbol> OwnedSymbols = new List<Symbol>();
        public static List<Symbol> SymbolObjects = new List<Symbol>(); // ALL symbol object stay in this list
        public static List<Symbol> CorrectGapList = new List<Symbol>(); // only used for strategy with gap up/down
        public static List<Symbol> ActiveSymbolList = new List<Symbol>(); // list for active trading or realtime data


        static async Task Main(string[] args)
        {
            Logger.SetLogLevel(Logger.LogLevel.LogLevelVerbose); // Custom Logger Test
            Logger.Verbose(Name, "Startup");

            new Thread(() =>
            {
                CreateHostBuilder(args).Build().Run();
            })
            { IsBackground = false }.Start();

            await UpdateConfigs();

            //OrderRepository.DeleteOrders("*"); // reset orders, orders that get executed do not get removed from DB yet, this is a temporary solution

            //List<string> Messages = new List<string>() { "test 3", "HA GAYY" };
            //List<string> Numbers = new List<string>() { "32476067619", "32470579542" };
            //await MobileService.SendTextMsg(Messages, Numbers);

            //MongoDBtest();

            //Test();

            //PaperTrailTest();

            InfiniteStartup();

            while (true) Console.ReadKey(); // zorgt er voor dat de console nooit sluit
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
                            //OrderRepository.DeleteOrders("*");
                            Thread.Sleep(2000);
                        }
                        else if (IbClient.ClientSocket.IsConnected() && NYtime.Hour >= StartingHour)
                        {
                            Thread.Sleep(2000);
                            Market.CheckMartketHours();
                            if (MarketState == false && MarketClosedMessage == false)
                            {
                                Logger.Critical(Name, "MARKET IS CLOSED TODAY");
                                MarketClosedMessage = true;
                            }
                        }

                        NYtime = Timezones.GetNewYorkTime();

                        //if (MarketState && NYtime.Hour == MarketHour && NYtime.Minute == MarketMinute)
                        if (MarketState && NYtime.Hour == 19-6 && NYtime.Minute == 30)
                        {
                            Logger.Info(Name, "Starting...");
                            MarketClosedMessage = false;

                            //OrderRepository.DeleteOrders("*"); // reset orders, orders that get executed do not get removed from DB yet, this is a temporary solution

                            if (!IbClient.ClientSocket.IsConnected())
                            {
                                ApiConnection.Connect();
                            }
                            else
                            {
                                //MarketScanner.GapUp();

                                // a lot of this could be moved up
                                SymbolList = SymbolList.OrderBy(x => Guid.NewGuid()).ToList();

                                SymbolObjects = SymbolManager.CreateSymbolObjects(SymbolList);

                                DataManager.ReadRawData(SymbolObjects);

                                ContractManager.RequestSymbolContracts(SymbolObjects);

                                ApiConnection.AccountUpdates();

                                //realtime bars opvragen voor symbollist
                                DataManager.GetRealTimeBars(SymbolObjects);

                                //DataManager.GetHistoricalBars(SymbolObjects, DateTime.Now, duration: "1 M"); // nog niet getest
                                //checkTime();
                                //DataManager.GetHistoricalBars(SymbolObjects, DateTime.Now, duration: "1 W", barSize: "3 mins"); // nog niet getest
                                Thread.Sleep(120000);

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Logger.Critical(Name, $"{ex}");
                    }
                }

            })
            { IsBackground = false }.Start();
        }


        public static async Task PaperTrailTest()
        {
            ApiConnection.Connect();
            IbClient.ClientSocket.reqManagedAccts();
            ApiConnection.AccountUpdates();
        }

        public static async Task UpdateConfigs()
        {
            Logger.Verbose(Name, "Updating Config");
            Config = await ConfigRepository.ReadConfig();

            // API
            AccountId = Config.ApiConfig.AccountId;
            Ip = Config.ApiConfig.Ip;
            Port = Config.ApiConfig.Port;
            ApiId = Config.ApiConfig.ApiId;

            // Strategy
            StartingHour = Config.StrategyConfig.StartingHour;
            MaxTradeValue = Config.StrategyConfig.MaxTradeValue;
            MarketHour = Config.StrategyConfig.MarketHour;
            MarketMinute = Config.StrategyConfig.MarketMinute;

            // Messaging
            PhoneNumbers = Config.MessagingConfig.PhoneNumbers;
            MailAddresses = Config.MessagingConfig.MailAddresses;

            // Buy
            BuyEnabled = Config.BuyConfig.BuyEnabled;
            BUseTrailLimitOrders = Config.BuyConfig.UseTrailLimitOrders;
            BPriceOffset = Config.BuyConfig.PriceOffset;
            BTrailingPercent = Config.BuyConfig.TrailingPercent;

            // Sell Trailing Limit order
            SellEnabled = Config.SellConfig.SellEnabled;
            SUseTrailLimitOrders = Config.SellConfig.UseTrailLimitOrders;
            SMinimumProfit = Config.SellConfig.MinimumProfit; // make sure minimum profit is higher than TrailingPercent to prevent sell with loss
            SPriceOffset = Config.SellConfig.PriceOffset;
            STrailingPercent = Config.SellConfig.TrailingPercent;

            // Error handling
            InfoCodes = Config.ErrorCodeConfig.InfoCodes;
            WarningCodes = Config.ErrorCodeConfig.WarningCodes;
        }

        // api test stuff
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
               return Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        
                        //logging.AddConsole(); // logging uitschakelen van de api
                    })
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
        }

        //static async Task checkTime()
        //{
        //    
        //        while (IbClient.ClientSocket.IsConnected())
        //        {
        //            DateTime NYtime = Timezones.GetNewYorkTime();
        //            //if (DateTime.Now.Hour >= 1 && DateTime.Now.Minute >= 45)
        //            if (NYtime.Hour >= 15 && NYtime.Minute >= 45)
        //            {
        //                //get latest datapoint
        //                String queryTime = Timezones.GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");

        //                foreach (Symbol S in SymbolObjects)
        //                {
        //                    try
        //                    {
        //                        //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
        //                        if (GettingHistoricalData < 50)
        //                        {
        //                            while (GettingHistoricalData >= 49)
        //                            {
        //                                Thread.Sleep(1);
        //                                if (S == SymbolObjects.Last()) break;
        //                            };
        //                            IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, queryTime, "3600 S", "15 mins", "MIDPOINT", 1, 1, false, null); // timing aanpassen dat het enkel het laatste punt is of checken of het al bestaat
        //                            GettingHistoricalData += 1;
        //                            Thread.Sleep(20);
        //                        }
        //                        //IbClient.ClientSocket.reqRealTimeBars(S.Id, S.Contract, 5, "MIDPOINT", false, null); // false om ook data buiten trading hours te krijgen
        //                        //S.ExecuteStrategy();
        //                        //Console.WriteLine(S.Ticker);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Logger.Error(Name, $"{S.Ticker} Failed: \n{ex}");
        //                    }
        //                }
        //                if(GettingHistoricalData == 0)
        //                {
        //                    Logger.Info(Name, "DONE buying stocks");
        //                    //GetDataForActiveSymbols();
        //                    break;
        //                }
        //            }
        //        }
        //    })
        //    { IsBackground = false }.Start();
        //}

        //public static async Task MongoDBtest()
        //{
        //    Logger.Verbose(Name, "Started MongoDB Test");

        //    var db = MongoDBClient.GetDatabase("TradingBot");
        //    var collection = db.GetCollection<BsonDocument>("AccountInfo");
        //    var count = await collection.CountDocumentsAsync(new BsonDocument("Type", "cashbalance")); // ALWAYS USE AWAIT
        //    var filter = new BsonDocument();
        //    using (var cursor = await collection.Find(filter).ToCursorAsync())
        //    {
        //        while (await cursor.MoveNextAsync())
        //        {
        //            foreach (var doc in cursor.Current)
        //            {
        //                //Console.WriteLine(doc);
        //            }
        //        }
        //    }
        //}

        public static async void Test()
        {
            List<double> intList1 = new List<double>() { 54.3, 54.3, 54.3, 54.31, 54.3, 54.29, 54.32, 54.28, 54.27, 54.275, 54.27, 54.27, 54.265, 54.27, 54.275, 54.28, 54.28, 54.265, 54.265, 54.265, 54.265, 54.27, 54.265, 54.27, 54.28, 54.27, 54.27, 54.275, 54.28, 54.295, 54.35, 54.37, 54.36, 54.34, 54.34, 54.32, 54.32, 54.31, 54.35, 54.31, 54.32, 54.3, 54.3, 54.32, 54.31, 54.32, 54.33, 54.31, 54.29, 54.295, 54.3, 54.3, 54.33, 54.34, 54.34, 54.34, 54.335, 54.34, 54.33, 54.34, 54.345, 54.34, 54.35, 54.36, 54.355, 54.33, 54.33, 54.33 };


            List<decimal> intList = new List<decimal>();
            intList1.ForEach(item => intList.Add((decimal)item));


            List<decimal> declist = new List<decimal>();
            List<decimal> declist2 = new List<decimal>();
            List<decimal> declist3 = new List<decimal>();
            //declist = await IndicatorMA.SMA(intList, 9);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            //declist = await IndicatorMA.EMA(intList, 9);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            //declist = await IndicatorMA.DEMA(intList, 5);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            //declist = await IndicatorMA.KAMA(intList, 10, 2, 30);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            //declist = await IndicatorMACD.MACD(intList, 28, 12, 2, 2);
            //declist2 = await IndicatorMACD.MACDsignal(declist, 10);
            //declist3 = await IndicatorMACD.MACDhist(declist, declist2);
            //foreach (decimal x in declist3)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();

            declist = await IndicatorRSI.RSI(intList, 14);
            foreach (decimal x in declist)
            {
                Console.WriteLine(x);
            }
            Console.ReadKey();

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
