using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public class ContractManager
    {
        private static string Name = "Contract Manager";
        public static async Task<Contract> CreateContract(string symbol, string secType = "STK", string exchange = "SMART", string currency = "USD")
        {
            Logger.Verbose(Name, $"Creating Contract for {symbol}");
            Contract Contract = new Contract();
            Contract.Symbol = symbol;
            Contract.SecType = secType;
            Contract.Exchange = exchange;
            Contract.PrimaryExch = "ISLAND";
            Contract.Currency = currency;

            return Contract;
        }

        public static async Task RequestSymbolContracts(List<Symbol> symbolObjects)
        {
            Logger.Verbose(Name, "Creating Symbol Contracts");
            foreach (Symbol S in symbolObjects)
            {
                Contract Contract = await CreateContract(S.Ticker);
                S.Contract = Contract;
                Program.IbClient.ClientSocket.reqContractDetails(S.Id, Contract);
                Thread.Sleep(20);
                //GetMarketData(Contract, i);
            }
        }
    }
}
