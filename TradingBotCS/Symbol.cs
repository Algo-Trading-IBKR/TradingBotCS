using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS
{
    public class Symbol
    {
        private string ticker { get; set; }
        private int id { get; set; }
        private IBApi.Contract contract { get; set; }
        private IBApi.ContractDetails contractDetails { get; set; }
        

        public string Ticker{
            get { return ticker; }
            set { ticker = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public IBApi.Contract Contract {
            get { return contract; }
            set { contract = value; }
        }

        public IBApi.ContractDetails ContractDetails {
            get { return contractDetails; }
            set { contractDetails = value; }
        }


        public Symbol(string ticker, int id)
        {
            this.Ticker = ticker;
            this.Id = id;
        }

        public override string ToString()
        {
            return Id + ": " + Ticker;
        }
    }
}
