using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;
using TradingBotCS.IBApi_OverRide;
using TradingBotCS.Strategies;

namespace TradingBotCS
{
    public class Symbol
    {
        //Class Variables
        private static string Name = "Symbol";
        public string Ticker { get; set; }
        public int Id { get; set; }
        public List<int> Ids { get; set; }
        public Contract Contract { get; set; }
        public ContractDetails ContractDetails { get; set; }
        public List<RawData> RawDataList { get; set; }
        public bool Enabled { get; set; }


        public Symbol(string ticker, int id)
        {
            this.Ticker = ticker;
            this.Id = id;
            this.Ids = new List<int>() { id };
            this.RawDataList = new List<RawData>();
            this.Enabled = true;
        }

        public override string ToString()
        {
            return Id + ": " + Ticker;
        }
    }
}
