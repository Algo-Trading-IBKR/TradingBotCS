using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
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
        static WrapperOverride IbClient = new WrapperOverride();
        static EReader IbReader;
        public static List<Symbol> SymbolObjects;

        static List<string> SymbolList = new List<string>() { "ACHC", "ARAY", "ALVR", "ATEC", "ALXO", "AMTI", "ABUS", "AYTU", "BEAM", "BLFS", "CAN", "CRDF", "CDNA", "CELH", "CDEV", "CHFS", "CTRN", "CLSK", "CVGI", "CUTR", "DNLI", "FATE", "FPRX", "FRHC", "FNKO", "GEVO", "GDEN", "GRBK", "GRPN", "GRWG", "HMHC", "IMAB", "IMVT", "NTLA", "KURA", "LE", "LXRX", "LOB", "LAZR", "AMD", "RRR", "IBKR", "NIO", "MARA", "MESA", "MEOH", "MVIS", "COOP", "NNDM", "NSTG", "NNOX", "NFE", "NXGN", "OPTT", "OCUL", "ORBC", "OESX", "PEIX", "PENN", "PSNL", "PLUG", "PGEN", "QNST", "RRGB", "REGI", "SGMS", "RUTH", "RIOT", "SWTX", "SPWR", "SUNW", "SGRY", "SNDX", "TCBI", "TA", "UPWK", "VSTM", "WPRT", "WWR", "XPEL", "UAA" };

        static async Task Main(string[] args)
        {
            test();

            await Connect();
            Console.WriteLine("getting updates");
            await AccountUpdates();
            Console.WriteLine("got updates");
            //Order Order = await CreateOrder()

            SymbolObjects = await CreateSymbolObjects(SymbolList);

            //Contract Contract = await CreateContract("AMD");
            
            //IbClient.ClientSocket.reqContractDetails(0, Contract);

            //GetMarketData(Contract);

            //GetTradingHours();
            Console.ReadKey();
        }

        static async Task<List<Symbol>> CreateSymbolObjects(List<string> symbolList)
        {
            List<Symbol> Result = new List<Symbol>();
            for(int i = 0; i < symbolList.Count; i++)
            {
                Result.Add(new Symbol(symbolList[i], i));
                Contract Contract = await CreateContract(symbolList[i]);
                IbClient.ClientSocket.reqContractDetails(i, Contract);
            }
            return Result;
        }

        static async Task Connect()
        {
            IbClient.ClientSocket.eConnect(Ip, Port, ApiId);
            IbReader = new EReader(IbClient.ClientSocket, IbClient.Signal);
            IbReader.Start();
            Console.WriteLine(IbClient.NextOrderId);

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

        static async Task AccountUpdates()
        {
            new Thread(() =>
            {
                if (IbClient.ClientSocket.IsConnected())
                {
                    IbClient.ClientSocket.reqAccountUpdates(true, "DU2361307");
                }
            })
            { IsBackground = false }.Start();
        }

        static async Task<Order> CreateOrder()
        {
            return new Order();
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

        static async Task GetMarketData(Contract contract)
        {
            List<TagValue> MktDataOptions = new List<TagValue>();

            IbClient.ClientSocket.reqMktData(1, contract, "", false, false, MktDataOptions);
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
