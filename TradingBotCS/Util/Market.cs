using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;


namespace TradingBotCS.Util
{
    public class Market
    {
        private static string Name = "Market Util";

        public static async Task CheckMartketHours()
        {
            try
            {
                Program.TestSymbol = new Symbol("TSLA", 99999);

                Contract Contract = new Contract();
                Contract.Symbol = Program.TestSymbol.Ticker;
                Contract.SecType = "STK";
                Contract.Exchange = "SMART";
                Contract.Currency = "USD";

                Program.TestSymbol.Contract = Contract;
                Program.IbClient.ClientSocket.reqContractDetails(Program.TestSymbol.Id, Contract);
                
                while (Program.TestSymbol.ContractDetails == null)
                {
                    Thread.Sleep(100);
                }

                // check hours
                string hourstring = Program.TestSymbol.ContractDetails.LiquidHours;

                hourstring = hourstring.Split(';')[0];
                if (hourstring.Contains("CLOSED"))
                {
                    Program.MarketState = false;
                    Program.MarketHour = 0;
                    Program.MarketMinute = 0;
                }
                else
                {
                    Program.MarketState = true;

                    string startstring = hourstring.Split('-')[0];
                    startstring = startstring.Split(':')[1];

                    int hour = Convert.ToInt32(startstring.Substring(0, 2));
                    int minute = Convert.ToInt32(startstring.Substring(2, 2));
                    //Logger.Verbose(Name, $"{hour}, {minute}");
                    Program.MarketHour = hour;
                    Program.MarketMinute = minute;
                    //for testing change this
                    //MarketHour = 15;
                    //MarketMinute = 02;
                }
            }
            catch (Exception ex)
            {
                Logger.Critical(Name, $"{ex}");
                Program.MarketState = false;
                Program.MarketHour = 0;
                Program.MarketMinute = 0;
            }
        }
    }
}
