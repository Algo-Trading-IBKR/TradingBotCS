using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class StrategyData
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
        public decimal RSI { get; set; }
        public decimal StochFRSIK { get; set; }
        public decimal StochFRSID { get; set; }
        public decimal Macd { get; set; }
        public decimal MacdHist { get; set; }
        public decimal MacdSignal { get; set; }
        public DateTime DateTime { get; set; }
        public StrategyData(string symbol, double price, decimal rsi, decimal stochFRSIK, decimal stochFRSID, decimal macd, decimal  macdHist, decimal macdSignal, DateTime dateTime )
        {
            Symbol = symbol;
            Price = price;
            RSI = rsi;
            StochFRSIK = stochFRSIK;
            StochFRSID = stochFRSID;
            Macd = macd;
            MacdHist = macdHist;
            MacdSignal = macdSignal;
            DateTime = dateTime;
        }
    }
}
