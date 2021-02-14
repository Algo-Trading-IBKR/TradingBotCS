using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class StrategyConfig
    {
        public int StartingHour { get; set; }
        public float MaxTradeValue { get; set; }
        public int MarketHour { get; set; }
        public int MarketMinute { get; set; }
        public StrategyConfig() { }
    }
}
