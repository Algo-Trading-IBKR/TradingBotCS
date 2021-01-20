using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class StrategyData
    {
        public double Price { get; set; }
        public decimal StochFRSIK { get; set; }
        public decimal StochFRSID { get; set; }
        public decimal Macd { get; set; }
        public decimal MacdHist { get; set; }
        public decimal MacdSignal { get; set; }
        public DateTime DateTime { get; set; }
        public StrategyData(double price, decimal stochFRSIK, decimal stochFRSID, decimal macd, decimal  macdHist, decimal macdSignal, DateTime dateTime )
        {
            Price = price;
            StochFRSIK = stochFRSIK;
            StochFRSID = stochFRSID;
            Macd = macd;
            MacdHist = macdHist;
            MacdSignal = macdSignal;
            DateTime = dateTime;
        }
    }
}
