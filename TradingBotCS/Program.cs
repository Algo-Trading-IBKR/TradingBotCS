using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using TradingBotCS.Email;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Models_Indicators;

namespace TradingBotCS
{
    class Program
    {
        static string Ip = "192.168.1.165";
        static int Port = 4002;
        static int ApiId = 5;
        public static WrapperOverride IbClient = new WrapperOverride();
        static EReader IbReader;
        public static List<Symbol> SymbolObjects;

        public static MongoClient MongoDBClient = new MongoClient(); // automatically connects to localhost:27017
        

        static List<string> SymbolList = new List<string>() { "ACHC", "ARAY", "ALVR", "ATEC", "ALXO", "AMTI", "ABUS", "AYTU", "BEAM", "BLFS", "CAN", "CRDF", "CDNA", "CELH", "CDEV", "CHFS", "CTRN", "CLSK", "CVGI", "CUTR", "DNLI", "FATE", "FPRX", "FRHC", "FNKO", "GEVO", "GDEN", "GRBK", "GRPN", "GRWG", "HMHC", "IMAB", "IMVT", "NTLA", "KURA", "LE", "LXRX", "LOB", "LAZR", "AMD", "RRR", "IBKR", "MARA", "MESA", "MEOH", "MVIS", "COOP", "NNDM", "NSTG", "NNOX", "NFE", "NXGN", "OPTT", "OCUL", "ORBC", "OESX", "PEIX", "PENN", "PSNL", "PLUG", "PGEN", "QNST", "RRGB", "REGI", "SGMS", "RUTH", "RIOT", "SWTX", "SPWR", "SUNW", "SGRY", "SNDX", "TCBI", "TA", "UPWK", "VSTM", "WPRT", "WWR", "XPEL" };

        static async Task Main(string[] args)
        {
            MongoDBtest();
            

            test();

            await Connect();
            Console.WriteLine("getting updates");
            await AccountUpdates();
            Console.WriteLine("got updates");
            Console.ReadKey();

            SymbolObjects = await CreateSymbolObjects(SymbolList);

            IbClient.ClientSocket.reqOpenOrders();

            while(true)Console.ReadKey(); // zorgt er voor dat de console nooit sluit
        }

        static async Task Connect()
        {
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

        static async Task<List<Symbol>> CreateSymbolObjects(List<string> symbolList)
        {
            List<Symbol> Result = new List<Symbol>();
            for(int i = 0; i < symbolList.Count; i++)
            {
                Result.Add(new Symbol(symbolList[i], i));
                Contract Contract = await CreateContract(symbolList[i]);
                IbClient.ClientSocket.reqContractDetails(i, Contract);
                IbClient.ClientSocket.reqRealTimeBars(i, Contract, 5, "MIDPOINT", false, null); // false om ook data buiten trading hours te krijgen
                //GetMarketData(Contract, i);
            }
            return Result;
        }

        static async Task AccountUpdates()
        {
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
            Contract Contract =  new Contract();
            Contract.Symbol = symbol;
            Contract.SecType = secType;
            Contract.Exchange = exchange;
            Contract.Currency = currency;

            return Contract;
        }

        static async Task GetMarketData(Contract contract, int id)
        {
            List<TagValue> MktDataOptions = new List<TagValue>();

            IbClient.ClientSocket.reqMktData(id, contract, "", false, false, MktDataOptions);
        }

        public static async Task MongoDBtest()
        {
            var db = MongoDBClient.GetDatabase("TradingBot");
            var collection = db.GetCollection<BsonDocument>("AccountInfo");
            var count = await collection.CountDocumentsAsync(new BsonDocument("Type", "cashbalance")); // ALWAYS USE AWAIT
            Console.WriteLine(count);
            Console.ReadKey();
            var filter = new BsonDocument();
            using (var cursor = await collection.Find(filter).ToCursorAsync())
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Console.WriteLine(doc);
                    }
                }
            }
            Console.ReadKey();
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

            //declist = await MovingAverage.MACD(intList, 26, 12);
            //declist2 = await MovingAverage.MACDsignal(declist, 9);
            //declist3 = await MovingAverage.MACDhist(declist, declist2);
            //foreach (decimal x in declist3)
            //{
            //    Console.WriteLine(x);
            //}

            //declist = await MovingAverage.RSI(intList, 14);
            //foreach (decimal x in declist)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.ReadKey();
        }
    }
}
