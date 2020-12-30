using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;

namespace TradingBotCS
{
    public class Symbol
    {
        //Class Variables
        public static float CashBalance { get; set; }


        private string ticker { get; set; }
        private int id { get; set; }
        private Contract contract { get; set; }
        private ContractDetails contractDetails { get; set; }
        private Order latestOrder { get; set; }
        private float orderPrice { get; set; }
        private float latestPrice { get; set; }
        private List<RawData> rawDatalist { get; set; }


        public string Ticker{
            get { return ticker; }
            set { ticker = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public Contract Contract {
            get { return contract; }
            set { contract = value; }
        }

        public ContractDetails ContractDetails {
            get { return contractDetails; }
            set { contractDetails = value; }
        }

        public Order LatestOrder
        {
            get { return latestOrder; }
            set { latestOrder = value; }
        }

        public float OrderPrice
        {
            get { return orderPrice; }
            set { orderPrice = value; }
        }

        public float LatestPrice
        {
            get { return latestPrice; }
            set { latestPrice = value; }
        }

        public List<RawData> RawDatalist
        {
            get { return rawDatalist; }
            set { rawDatalist = value; }
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
