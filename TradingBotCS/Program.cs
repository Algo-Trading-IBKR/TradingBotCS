using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;

namespace TradingBotCS
{
    class Program
    {
        static EWrapperImpl IbClient = new EWrapperImpl();
        static EReader IbReader = new EReader(IbClient.ClientSocket, IbClient.Signal);
 

        static async Task Main(string[] args)
        {
            test();
            await Connect();
            Console.WriteLine("getting updates");
            await AccountUpdates();
            Console.WriteLine("got updates");
            //Order Order = await CreateOrder()

            Contract Contract = await CreateContract("AMD", "STK", "SMART", "USD");
            GetMarketData(Contract);


            Console.ReadKey();
            //await TestFunction();
        }

        static async Task Connect()
        {
            IbClient.ClientSocket.eConnect("192.168.50.107", 4002, 0);
            IbReader.Start();

            new Thread(() =>
            {
                while (IbClient.ClientSocket.IsConnected())
                {
                    IbClient.Signal.waitForSignal();
                    IbReader.processMsgs();
                }
            })
            { IsBackground = false }.Start();
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

        static async Task<Contract> CreateContract(string symbol, string secType, string exchange, string currency)
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




        //static async Task TestFunction()
        //{
            // Create the ibClient object to represent the connection
            //ibClient = new EWrapperImpl();

            // Connect to the IB Server through TWS/IB Gateway. Parameters are:
            // host       - Host name or IP address of the host running TWS
            // port       - The port TWS listens through for connections - 7496
            // clientId   - The identifier of the client application
            //ibClient.ClientSocket.eConnect("192.168.50.107", 4002, 0);

            // For IB TWS API version 9.72 and higher, implement this
            // signal-handling code. Otherwise comment it out.

            //var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
            //reader.Start();
            //new Thread(() =>
            //{
            //    while (ibClient.ClientSocket.IsConnected())
            //    {
            //        ibClient.Signal.waitForSignal();
            //        reader.processMsgs();
            //    }
            //})
            //{ IsBackground = true }.Start();


            // Create a new contract to specify the security we are searching for
            //Contract contract = new Contract();
            // Fill in the Contract properties
            //contract.Symbol = "AMD";
            //contract.SecType = "STK";
            //contract.Exchange = "SMART";
            //contract.Currency = "USD";
            // Create a new TagValue List object (for API version 9.71) 
            //List<TagValue> mktDataOptions = new List<TagValue>();

            // Kick off the request for market data for this
            // contract.  reqMktData Parameters are:
            // tickerId           - A unique id to represent this request
            // contract           - The contract object specifying the financial instrument
            // genericTickList    - A string representing special tick values
            // snapshot           - When true obtains only the latest price tick
            //                      When false, obtains data in a stream
            // regulatory snapshot - API version 9.72 and higher. Remove for earlier versions of API
            // mktDataOptions   - TagValueList of options 
            //IbClient.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);
            //IbClient.ClientSocket.reqMarketDepth(2, contract, 5, false, mktDataOptions);
            //IbClient.ClientSocket.reqRealTimeBars(1, contract, 5, "TRADES", false, mktDataOptions);

            // Pause so we can view the output
            //Console.ReadKey();

            // Cancel the subscription/request. Parameter is:
            // tickerId         - A unique id to represent the request 
            //ibClient.ClientSocket.cancelMktData(1);

            // Disconnect from TWS
            //ibClient.ClientSocket.eDisconnect();
        //}

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
