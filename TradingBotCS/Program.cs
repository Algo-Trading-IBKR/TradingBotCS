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

        static async Task Main(string[] args)
        {
            TestFunction();
        }


        static void TestFunction()
        {
            // Create the ibClient object to represent the connection
            EWrapperImpl ibClient = new EWrapperImpl();

            // Connect to the IB Server through TWS/IB Gateway. Parameters are:
            // host       - Host name or IP address of the host running TWS
            // port       - The port TWS listens through for connections - 7496
            // clientId   - The identifier of the client application
            ibClient.ClientSocket.eConnect("127.0.0.1", 7497, 0);

            // For IB TWS API version 9.72 and higher, implement this
            // signal-handling code. Otherwise comment it out.

            var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
            reader.Start();
            new Thread(() =>
            {
                while (ibClient.ClientSocket.IsConnected())
                {
                    ibClient.Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();


            // Create a new contract to specify the security we are searching for
            Contract contract = new Contract();
            // Fill in the Contract properties
            contract.Symbol = "AMD";
            contract.SecType = "STK";
            contract.Exchange = "SMART";
            contract.Currency = "USD";
            // Create a new TagValue List object (for API version 9.71) 
            List<TagValue> mktDataOptions = new List<TagValue>();

            // Kick off the request for market data for this
            // contract.  reqMktData Parameters are:
            // tickerId           - A unique id to represent this request
            // contract           - The contract object specifying the financial instrument
            // genericTickList    - A string representing special tick values
            // snapshot           - When true obtains only the latest price tick
            //                      When false, obtains data in a stream
            // regulatory snapshot - API version 9.72 and higher. Remove for earlier versions of API
            // mktDataOptions   - TagValueList of options 
            ibClient.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);
            ibClient.ClientSocket.reqMarketDepth(2, contract, 5, false, mktDataOptions);
            ibClient.ClientSocket.reqRealTimeBars(1, contract, 5, "TRADES", false, mktDataOptions);

            // Pause so we can view the output
            Console.ReadKey();

            // Cancel the subscription/request. Parameter is:
            // tickerId         - A unique id to represent the request 
            //ibClient.ClientSocket.cancelMktData(1);

            // Disconnect from TWS
            //ibClient.ClientSocket.eDisconnect();
        }
    }
}
