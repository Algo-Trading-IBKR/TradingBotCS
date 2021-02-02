using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public class MarketScanner
    {

        public static async Task GapUp()
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
                temp.BelowPrice = i + 1;
                List<TagValue> test = new List<TagValue>();
                List<TagValue> test2 = new List<TagValue>();
                while (Program.ScannerReady == false)
                {
                    Thread.Sleep(1);
                }
                if (Program.ScannerReady) Program.IbClient.ClientSocket.reqScannerSubscription((int)i, temp, test, test2);
                if (i == 24) Thread.Sleep(1000);
                Program.ScannerReady = false;
            }
        }
    }
}
