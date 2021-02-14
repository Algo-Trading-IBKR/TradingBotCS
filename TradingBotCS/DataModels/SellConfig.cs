using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class SellConfig
    {
        public bool SellEnabled { get; set; }
        public bool UseTrailLimitOrders { get; set; }
        public float MinimumProfit { get; set; }
        public float PriceOffset { get; set; }
        public float TrailingPercent { get; set; }

        public SellConfig() { }
    }
}
